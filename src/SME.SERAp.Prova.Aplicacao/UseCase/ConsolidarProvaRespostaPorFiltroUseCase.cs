using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ConsolidarProvaRespostaPorFiltroUseCase : IConsolidarProvaRespostaPorFiltroUseCase
    {
        private readonly IMediator mediator;
        private readonly IServicoLog serviceLog;

        public ConsolidarProvaRespostaPorFiltroUseCase(IMediator mediator, IServicoLog serviceLog)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.serviceLog = serviceLog ?? throw new ArgumentNullException(nameof(serviceLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<ExportacaoResultadoFiltroDto>();
            if (filtro is null)
                throw new NegocioException("O filtro precisa ser informado");            

            serviceLog.Registrar(LogNivel.Informacao, $"Consolidar dados prova por filtro. msg: {mensagemRabbit.Mensagem}");

            var exportacaoResultado = await mediator.Send(new ObterExportacaoResultadoStatusQuery(filtro.ProcessoId, filtro.ProvaSerapId));
            if (exportacaoResultado is null)
                throw new NegocioException("A exportação não foi encontrada");
            
            if (exportacaoResultado.Status != ExportacaoResultadoStatus.Processando)
                return true;            

            try
            {
                var ues = await mediator.Send(new ObterUesSerapPorProvaSerapEDreCodigoQuery(filtro.ProvaSerapId, filtro.DreEolId));
                if (ues == null || !ues.Any())
                    return false;

                var turmas = await mediator.Send(new ObterTurmasPorCodigoDreEProvaSerapQuery(filtro.DreEolId, filtro.ProvaSerapId));
                if (turmas == null || !turmas.Any())
                    return false;                

                var ueEolIds = ues.Select(c => c.CodigoUe).Distinct().ToArray();
                var turmaEolIds = turmas.Select(c => c.Codigo).Distinct().ToArray();
                
                var exportacaoResultadoItem = new ExportacaoResultadoItem(filtro.ProcessoId, filtro.DreEolId, ueEolIds, turmaEolIds);
                exportacaoResultadoItem.Id = await mediator.Send(new InserirExportacaoResultadoItemCommand(exportacaoResultadoItem));

                foreach (var turma in turmas)
                {
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ConsolidarProvaResultadoFiltroTurma,
                        new ExportacaoResultadoFiltroTurmaDto(filtro.ProcessoId, filtro.ProvaSerapId,
                            exportacaoResultadoItem.Id, filtro.DreEolId, turma.Codigo, filtro.AdesaoManual,
                            filtro.AlunosComDeficiencia)));
                }                    

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Erro));
                await mediator.Send(new ExcluirExportacaoResultadoItemCommand(0, exportacaoResultado.Id));
                serviceLog.Registrar($"Erro ao consolidar os dados da prova por filtro. msg: {mensagemRabbit.Mensagem}", ex);
                return false;
            }
        }
    }
}
