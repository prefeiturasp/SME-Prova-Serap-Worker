using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
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
        private readonly IServicoLog servicoLog;

        public TratarProvaResultadoExtracaoUseCase(IMediator mediator, IServicoLog servicoLog)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var extracao = mensagemRabbit.ObterObjetoMensagem<ProvaExtracaoDto>();
            if (extracao is null)
                throw new NegocioException("O id da prova serap precisa ser informado");
            
            var exportacaoResultado = await mediator.Send(new ObterExportacaoResultadoPorIdQuery(extracao.ExtracaoResultadoId));
            if (exportacaoResultado is null)
                throw new NegocioException("A exportação não foi encontrada");            

            try
            {
                var checarProvaExiste = await mediator.Send(new VerificaProvaExistePorSerapIdQuery(extracao.ProvaSerapId));
                if (!checarProvaExiste)
                    throw new NegocioException("A prova informada não foi encontrada no serap estudantes");

                if (exportacaoResultado.Status != ExportacaoResultadoStatus.Processando) 
                    return true;
                
                var resultadoExtracaoProva = await mediator.Send(new VerificaResultadoExtracaoProvaExisteQuery(extracao.ProvaSerapId));
                if (!resultadoExtracaoProva)
                    throw new NegocioException($"Os resultados da prova {extracao.ProvaSerapId} ainda não foram gerados");

                var caminhoCompletoArquivo = ObterCaminhoCompletoArquivo(exportacaoResultado.NomeArquivo);
                if (string.IsNullOrEmpty(caminhoCompletoArquivo))
                    throw new NegocioException("O caminho do arquivo da exportação dos resultados não foi localizado.");
                
                VerificarERemoverArquivoExistente(caminhoCompletoArquivo);

                var prova = await mediator.Send(new ObterProvaDetalhesPorProvaLegadoIdQuery(exportacaoResultado.ProvaSerapId));                
                await mediator.Send(new GerarCSVExtracaoProvaCommand(prova.TotalItens, caminhoCompletoArquivo));

                var filtrosParaPublicar = new List<ExportacaoResultadoFiltroDto>();
                var dres = await mediator.Send(new ObterDresSerapQuery());

                foreach (var dre in dres)
                {
                    var ues = await mediator.Send(new ObterUesSerapPorProvaSerapEDreCodigoQuery(extracao.ProvaSerapId, dre.CodigoDre));
                    foreach (var ue in ues)
                    {
                        var turmasUe = await mediator.Send(new ObterTurmasPorCodigoUeEProvaSerapQuery(ue.CodigoUe, prova.LegadoId));
                        if (turmasUe == null || !turmasUe.Any())
                            continue;
                        
                        var paginasTurmas = Paginar(turmasUe.ToList());
                        foreach (var turmas in paginasTurmas)
                        {
                            var ueIds = new[] { ue.CodigoUe };

                            var exportacaoResultadoItem = new ExportacaoResultadoItem(exportacaoResultado.Id, dre.CodigoDre, ueIds);
                            exportacaoResultadoItem.Id = await mediator.Send(new InserirExportacaoResultadoItemCommand(exportacaoResultadoItem));

                            var filtro = new ExportacaoResultadoFiltroDto(exportacaoResultado.Id,
                                exportacaoResultado.ProvaSerapId, exportacaoResultadoItem.Id, dre.CodigoDre, ueIds)
                            {
                                TurmaEolIds = turmas.Select(t => t.Codigo).ToArray(),
                                CaminhoArquivo = caminhoCompletoArquivo
                            };
                            filtrosParaPublicar.Add(filtro);
                        }
                    }
                }

                foreach (var filtro in filtrosParaPublicar)
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ExtrairResultadosProvaFiltro, filtro));
                
                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Finalizado));

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Erro));
                await mediator.Send(new ExcluirExportacaoResultadoItemCommand(0, exportacaoResultado.Id));

                servicoLog.Registrar($"Escrever dados no arquivo CSV. msg: {mensagemRabbit.Mensagem}", ex);

                return false;
            }
        }

        private static List<List<Turma>> Paginar(IEnumerable<Turma> turmas)
        {
            var paginacao = new ListaPaginada<Turma>(turmas.ToList(), 5);
            return paginacao.ObterTodasAsPaginas();
        }

        private static string ObterCaminhoCompletoArquivo(string nomeArquivo)
        {
            var pathResultados = Environment.GetEnvironmentVariable("PathResultadosExportacaoSerap");
            if (string.IsNullOrEmpty(pathResultados))
                return string.Empty;
            
            var caminhoCompleto = Path.Combine(pathResultados, nomeArquivo);
            return caminhoCompleto;
        }

        private static void VerificarERemoverArquivoExistente(string caminhoArquivo)
        {
            if (File.Exists(caminhoArquivo))
                File.Delete(caminhoArquivo);
        }
    }
}