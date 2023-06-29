using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExcluirDadosConsolidadoUseCase : IExcluirDadosConsolidadoUseCase
    {
        private readonly IMediator mediator;
        private readonly IServicoLog serviceLog;
        public ExcluirDadosConsolidadoUseCase(IMediator mediator, IServicoLog serviceLog)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.serviceLog = serviceLog ?? throw new ArgumentNullException(nameof(serviceLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var extracao = mensagemRabbit.ObterObjetoMensagem<ProvaExtracaoDto>();
            var exportacaoResultado = await mediator.Send(new ObterExportacaoResultadoStatusQuery(extracao.ExtracaoResultadoId, extracao.ProvaSerapId));

            try
            {
                if (exportacaoResultado is null)
                    throw new NegocioException("A exportação não foi encontrada");

                var checarProvaExiste = await mediator.Send(new VerificaProvaExistePorSerapIdQuery(extracao.ProvaSerapId));

                if (!checarProvaExiste)
                    throw new NegocioException("A prova informada não foi encontrada no serap estudantes");

                if (exportacaoResultado.Status == ExportacaoResultadoStatus.Erro) return false;

                if (exportacaoResultado.Status == ExportacaoResultadoStatus.Iniciado)
                {                    
                    serviceLog.Registrar(LogNivel.Informacao, $"Excluir dados consolidado prova:{extracao.ProvaSerapId}. msg: {mensagemRabbit.Mensagem}");
                    await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Processando));
                    extracao.Take = 500;
                    extracao.Skip = 0;
                }

                string fila = string.Empty;
                await mediator.Send(new ExcluirDadosConsolidadoCommand(extracao.ProvaSerapId, extracao.Take, extracao.Skip));
                var existeDadosConsolidado = await mediator.Send(new VerificaResultadoExtracaoProvaExisteQuery(extracao.ProvaSerapId));
                if (existeDadosConsolidado)
                {
                    extracao.Skip += extracao.Take;
                    fila = RotasRabbit.ExcluirDadosConsolidado;
                }
                else
                {
                    extracao.Skip = 0;
                    fila = RotasRabbit.ConsolidarProvaResultado;
                }

                await mediator.Send(new PublicaFilaRabbitCommand(fila, extracao));
                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Erro));
                serviceLog.Registrar(LogNivel.Critico, $"Erro -- Excluir dados consolidado prova. msg: {mensagemRabbit.Mensagem}", ex.Message, ex.StackTrace);
                return false;
            }
        }
    }
}
