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
    public class ExtracaoProvaResultadoUseCase : IExtracaoProvaResultadoUseCase
    {

        private readonly IMediator mediator;
        private readonly IServicoLog servicoLog;

        public ExtracaoProvaResultadoUseCase(IMediator mediator, IServicoLog servicoLog)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var extracao = mensagemRabbit.ObterObjetoMensagem<ProvaExtracaoDto>();
            var exportacaoResultado = await mediator.Send(new ObterExportacaoResultadoStatusQuery(extracao.ExtracaoResultadoId, extracao.ProvaSerapId));

            try
            {
                if (exportacaoResultado is null)
                    throw new NegocioException("A exportação não foi encontrada");

                if (exportacaoResultado.Status == ExportacaoResultadoStatus.Processando)
                {

                    if (string.IsNullOrEmpty(extracao.CaminhoArquivo))
                    {
                        string caminhoCompletoArquivo = ObterCaminhoCompletoArquivo(exportacaoResultado.NomeArquivo);
                        VerificarERemoverArquivoExistente(caminhoCompletoArquivo);
                        extracao.CaminhoArquivo = caminhoCompletoArquivo;
                        var prova = await mediator.Send(new ObterProvaDetalhesPorIdQuery(exportacaoResultado.ProvaSerapId));
                        await mediator.Send(new GerarCSVExtracaoProvaCommand(prova.TotalItens, extracao.CaminhoArquivo));
                    }

                    if (!ExisteArquivo(extracao.CaminhoArquivo))
                        throw new NegocioException($"Arquivo não foi encontrado: {extracao.CaminhoArquivo}");

                    var resultado = await mediator.Send(new ObterExtracaoProvaRespostaQuery(extracao.ProvaSerapId, extracao.Take, extracao.Skip));
                    if (resultado != null && resultado.Any())
                    {
                        await mediator.Send(new EscreverDadosCSVExtracaoProvaCommand(resultado, extracao.CaminhoArquivo));
                        extracao.Skip += extracao.Take;
                        await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ExtrairResultadosProva, extracao));
                    }
                    else
                        await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Finalizado));

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Erro));
                servicoLog.Registrar($"Escrever dados no arquivo CSV. msg: {mensagemRabbit.Mensagem}", ex);
                return false;
            }
        }

        private bool ExisteArquivo(string caminhoArquivo)
        {
            return File.Exists(caminhoArquivo);
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
