using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            
            if (exportacaoResultado.Status != ExportacaoResultadoStatus.Processando) 
                return true;
            
            var checarProvaExiste = await mediator.Send(new VerificaProvaExistePorSerapIdQuery(extracao.ProvaSerapId));
            if (!checarProvaExiste)
                throw new NegocioException("A prova informada não foi encontrada no serap estudantes");
            
            var resultadoExtracaoProva = await mediator.Send(new VerificaResultadoExtracaoProvaExisteQuery(extracao.ProvaSerapId));
            if (!resultadoExtracaoProva)
                throw new NegocioException($"Os resultados da prova {extracao.ProvaSerapId} ainda não foram gerados");
            
            var caminhoCompletoArquivo = ObterCaminhoCompletoArquivo(exportacaoResultado.NomeArquivo);
            if (string.IsNullOrEmpty(caminhoCompletoArquivo))
                throw new NegocioException("O caminho do arquivo da exportação dos resultados não foi localizado.");

            try
            {
                VerificarERemoverArquivoExistente(caminhoCompletoArquivo);

                var prova = await mediator.Send(new ObterProvaDetalhesPorProvaLegadoIdQuery(exportacaoResultado.ProvaSerapId));
                if (prova == null)
                    return false;
                
                await mediator.Send(new GerarCSVExtracaoProvaCommand(prova.TotalItens, caminhoCompletoArquivo));
                
                var dres = await mediator.Send(new ObterDresSerapQuery());
                if (dres == null || !dres.Any())
                    return false;
                
                foreach (var dre in dres)
                {
                    var ues = await mediator.Send(new ObterUesSerapPorProvaSerapEDreCodigoQuery(extracao.ProvaSerapId, dre.CodigoDre));
                    if (ues == null || !ues.Any())
                        continue;

                    foreach (var ue in ues)
                    {
                        await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ExtrairResultadosProvaFiltro,
                            new TratarProvaResultadoExtracaoFiltroDto(exportacaoResultado.Id,
                                extracao.ProvaSerapId, caminhoCompletoArquivo, dre.CodigoDre, ue.CodigoUe)));                        
                    }
                }

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