using MediatR;
using SME.SERAp.Prova.Aplicacao.Queries.VerificaProvaPossuiTipoDeficiencia;
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
    public class ConsolidarProvaResultadoUseCase : IConsolidarProvaResultadoUseCase
    {
        private readonly IMediator mediator;
        private readonly IServicoLog serviceLog;

        public ConsolidarProvaResultadoUseCase(IMediator mediator, IServicoLog serviceLog)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.serviceLog = serviceLog ?? throw new ArgumentNullException(nameof(serviceLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var extracao = mensagemRabbit.ObterObjetoMensagem<ProvaExtracaoDto>();
            if (extracao is null)
                throw new NegocioException("O id da prova serap precisa ser informado");            

            serviceLog.Registrar(LogNivel.Informacao, $"Consolidar dados prova:{extracao.ProvaSerapId}. msg: {mensagemRabbit.Mensagem}");

            var exportacaoResultado = await mediator.Send(new ObterExportacaoResultadoStatusQuery(extracao.ExtracaoResultadoId, extracao.ProvaSerapId));
            if (exportacaoResultado is null)
                throw new NegocioException("A exportação não foi encontrada");
            
            var checarProvaExiste = await mediator.Send(new VerificaProvaExistePorSerapIdQuery(extracao.ProvaSerapId));
            if (!checarProvaExiste)
                throw new NegocioException("A prova informada não foi encontrada no serap estudantes");            

            try
            {
                await mediator.Send(new ExcluirResultadoProvaConsolidadosPorProvaLegadoIdCommand(extracao.ProvaSerapId));
                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Processando));

                var prova = await mediator.Send(new ObterProvaDetalhesPorProvaLegadoIdQuery(exportacaoResultado.ProvaSerapId));
                if (prova == null)
                    return false;
                
                var adesaoManual = !prova.AderirTodos;
                
                var possuiTipoDeficiencia = await mediator.Send(new VerificaProvaPossuiTipoDeficienciaQuery(exportacaoResultado.ProvaSerapId));

                var dres = await mediator.Send(new ObterDresSerapQuery());
                foreach (var dre in dres)
                {
                    var ues = await mediator.Send(new ObterUesSerapPorProvaSerapEDreCodigoQuery(exportacaoResultado.ProvaSerapId, dre.CodigoDre));
                    if (ues == null || !ues.Any())
                        continue;

                    foreach (var ue in ues)
                    {
                        await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ConsolidarProvaResultadoFiltro,
                            new ExportacaoResultadoFiltroDto(exportacaoResultado.Id,
                                exportacaoResultado.ProvaSerapId, dre.CodigoDre, ue.CodigoUe, adesaoManual,
                                possuiTipoDeficiencia)));
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Erro));
                await mediator.Send(new ExcluirExportacaoResultadoItemCommand(0, exportacaoResultado.Id));
                serviceLog.Registrar(LogNivel.Critico, $"Erro -- Consolidar os dados da prova. msg: {mensagemRabbit.Mensagem}", ex.Message, ex.StackTrace);
                return false;
            }
        }
    }
}