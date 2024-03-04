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
    public class TratarProvaResultadoExtracaoFiltroUseCase : ITratarProvaResultadoExtracaoFiltroUseCase
    {

        private readonly IMediator mediator;
        private readonly IServicoLog servicoLog;

        public TratarProvaResultadoExtracaoFiltroUseCase(IMediator mediator, IServicoLog servicoLog)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<ExportacaoResultadoFiltroDto>();
            var exportacaoResultado = await mediator.Send(new ObterExportacaoResultadoStatusQuery(filtro.ProcessoId, filtro.ProvaId));

            try
            {
                if (filtro is null)
                    throw new NegocioException("O filtro precisa ser informado");
                if (exportacaoResultado is null)
                    throw new NegocioException("A exportação não foi encontrada");

                if (exportacaoResultado.Status == ExportacaoResultadoStatus.Processando)
                {
                    if (!ExisteArquivo(filtro.CaminhoArquivo))
                        throw new NegocioException($"Arquivo não foi encontrado: {filtro.CaminhoArquivo}");

                    var prova = await mediator.Send(new ObterProvaDetalhesPorProvaLegadoIdQuery(exportacaoResultado.ProvaSerapId));


                    var resultado = await mediator.Send(new ObterExtracaoProvaRespostaQuery(filtro.ProvaId, prova.AderirTodos));

                    if (resultado != null && resultado.Any())
                        await mediator.Send(new EscreverDadosCSVExtracaoProvaCommand(resultado, filtro.CaminhoArquivo));

                    if (prova.AderirTodos == false)
                        await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Finalizado));

                    else
                    {

                        await mediator.Send(new ExcluirExportacaoResultadoItemCommand(filtro.ItemId));
                        bool existeItemProcesso = await mediator.Send(new ConsultarSeExisteItemProcessoPorIdQuery(exportacaoResultado.Id));
                        if (!existeItemProcesso)
                        {

                            await mediator.Send(new ExcluirExportacaoResultadoItemCommand(0, exportacaoResultado.Id));
                        }

                    }




                }
            }
            catch (Exception ex)
            {
                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Erro));
                await mediator.Send(new ExcluirExportacaoResultadoItemCommand(0, exportacaoResultado.Id));
                servicoLog.Registrar($"Escrever dados no arquivo CSV. msg: {mensagemRabbit.Mensagem}", ex);
                return false;
            }
            return true;
        }

        private bool ExisteArquivo(string caminhoArquivo)
        {
            return File.Exists(caminhoArquivo);
        }
    }
}
