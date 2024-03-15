using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using SME.SERAp.Prova.Aplicacao.Queries.ObterAlunosResultadoProvaDeficiencia;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ConsolidarProvaRespostaPorFiltroTurmaUseCase : IConsolidarProvaRespostaPorFiltroTurmaUseCase
    {
        private readonly IMediator mediator;
        private readonly IServicoLog servicoLog;

        public ConsolidarProvaRespostaPorFiltroTurmaUseCase(IMediator mediator, IServicoLog servicoLog)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<ExportacaoResultadoFiltroDto>();
            
            var exportacaoResultado = await mediator.Send(new ObterExportacaoResultadoStatusQuery(filtro.ProcessoId, filtro.ProvaId));
            if (exportacaoResultado is null)
                throw new NegocioException("A exportação não foi encontrada");            

            try
            {
                if (filtro is null)
                    throw new NegocioException("O filtro precisa ser informado");

                if (exportacaoResultado.Status != ExportacaoResultadoStatus.Processando) 
                    return true;
                
                var consolidadoProvaResposta = Enumerable.Empty<ConsolidadoProvaRespostaDto>();
                
                if (filtro.AlunosComDeficiencia)
                    consolidadoProvaResposta = await mediator.Send(new ObterAlunosResultadoProvaDeficienciaQuery(filtro.ProvaId, filtro.TurmaEolIds));
                else if (filtro.AdesaoManual)
                    ConsolidarProvaRespostaAdesaoManual();
                else
                    ConsolidarProvaRespostaAdesaoTodos();
                
                /* todo:
                await mediator.Send(new ConsolidarProvaRespostaPorFiltroCommand(filtro.ProvaId, filtro.DreEolId, filtro.UeEolIds, filtro.TurmaEolIds));
                await mediator.Send(new ExcluirExportacaoResultadoItemCommand(filtro.ItemId));
                */

                var existeItemProcesso = await mediator.Send(new ConsultarSeExisteItemProcessoPorIdQuery(exportacaoResultado.Id));
                if (existeItemProcesso)
                    return true;

                var extracao = new ProvaExtracaoDto { ExtracaoResultadoId = filtro.ProcessoId, ProvaSerapId = filtro.ProvaId };
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ExtrairResultadosProva, extracao));

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Erro));
                await mediator.Send(new ExcluirExportacaoResultadoItemCommand(0, exportacaoResultado.Id));

                servicoLog.Registrar($"Erro ao consolidar os dados da prova por filtro. msg: {mensagemRabbit.Mensagem}", ex);
                return false;
            }
        }
    }
}
