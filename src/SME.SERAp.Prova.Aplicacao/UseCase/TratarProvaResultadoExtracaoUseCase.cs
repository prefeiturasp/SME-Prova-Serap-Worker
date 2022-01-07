using MediatR;
using Sentry;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static SME.SERAp.Prova.Infra.Utils.Paginacao;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProvaResultadoExtracaoUseCase : ITratarProvaResultadoExtracaoUseCase
    {
        private readonly IMediator mediator;

        public TratarProvaResultadoExtracaoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var extracao = mensagemRabbit.ObterObjetoMensagem<ProvaExtracaoDto>();
            var exportacaoResultado = await mediator.Send(new ObterExportacaoResultadoStatusQuery(extracao.ExtracaoResultadoId, extracao.ProvaSerapId));
            try
            {
                if (extracao is null)
                    throw new NegocioException("O id da prova serap precisa ser informado");

                if (exportacaoResultado is null)
                    throw new NegocioException("A exportação não foi encontrada");

                var checarProvaExiste = await mediator.Send(new VerificaProvaExistePorSerapIdQuery(extracao.ProvaSerapId));

                if (!checarProvaExiste)
                    throw new NegocioException("A prova informada não foi encontrada no serap estudantes");

                if (exportacaoResultado.Status == ExportacaoResultadoStatus.Processando)
                {

                    var resultadoExtracaoProva = await mediator.Send(new VerificaResultadoExtracaoProvaExisteQuery(extracao.ProvaSerapId));
                    if (!resultadoExtracaoProva)
                        throw new NegocioException($"Os resultados da prova {extracao.ProvaSerapId} ainda não foram gerados");

                    exportacaoResultado = await mediator.Send(new ObterExportacaoResultadoPorIdQuery(exportacaoResultado.Id));
                    var prova = await mediator.Send(new ObterProvaDetalhesPorIdQuery(exportacaoResultado.ProvaSerapId));

                    string caminhoCompletoArquivo = ObterCaminhoCompletoArquivo(exportacaoResultado.NomeArquivo);
                    VerificarERemoverArquivoExistente(caminhoCompletoArquivo);

                    await mediator.Send(new GerarCSVExtracaoProvaCommand(prova.TotalItens, caminhoCompletoArquivo));

                    var filtrosParaPublicar = new List<ExportacaoResultadoFiltroDto>();
                    var dres = await mediator.Send(new ObterDresSerapQuery());
                    foreach (Dre dre in dres)
                    {
                        var ues = await mediator.Send(new ObterUesSerapPorDreCodigoQuery(dre.CodigoDre));
                        foreach (Ue ue in ues)
                        {
                            var turmasUe = await mediator.Send(new ObterTurmasPorCodigoUeEAnoLetivoQuery(ue.CodigoUe, prova.Inicio.Year));
                            var paginasTurmas = Paginar(turmasUe.ToList());
                            foreach (List<Turma> turmas in paginasTurmas)
                            {
                                var codigosTurmas = turmas.Select(t => t.Codigo).ToArray();

                                var ueIds = new string[] { ue.CodigoUe };
                                var exportacaoResultadoItem = new ExportacaoResultadoItem(exportacaoResultado.Id, dre.CodigoDre, ueIds);
                                exportacaoResultadoItem.Id = await mediator.Send(new InserirExportacaoResultadoItemCommand(exportacaoResultadoItem));

                                var filtro = new ExportacaoResultadoFiltroDto(exportacaoResultado.Id, exportacaoResultado.ProvaSerapId, exportacaoResultadoItem.Id, dre.CodigoDre, ueIds);
                                filtro.TurmaEolIds = codigosTurmas;
                                filtro.CaminhoArquivo = caminhoCompletoArquivo;
                                filtrosParaPublicar.Add(filtro);
                            }
                        }
                    }

                    foreach(ExportacaoResultadoFiltroDto filtro in filtrosParaPublicar)
                    {
                        await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ExtrairResultadosProvaFiltro, filtro));
                    }
                }
            }
            catch (Exception ex)
            {
                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Erro));
                SentrySdk.CaptureMessage($"Erro ao gerar CSV da prova. msg: {mensagemRabbit.Mensagem}", SentryLevel.Error);
                SentrySdk.CaptureException(ex);
                throw ex;
            }
            return true;
        }

        private List<List<Turma>> Paginar(List<Turma> turmas)
        {
            var paginacao = new ListaPaginada<Turma>(turmas.ToList(), 5);
            return paginacao.ObterTodasAsPaginas();
        }

        private string ObterCaminhoCompletoArquivo(string nomeArquivo)
        {
            var pathResultados = Environment.GetEnvironmentVariable("PathResultadosExportacaoSerap");
            string caminhoCompleto = Path.Combine(pathResultados, nomeArquivo);
            return caminhoCompleto;
        }

        private void VerificarERemoverArquivoExistente(string caminhoArquivo)
        {
            if (File.Exists(caminhoArquivo))
                File.Delete(caminhoArquivo);
        }
    }
}