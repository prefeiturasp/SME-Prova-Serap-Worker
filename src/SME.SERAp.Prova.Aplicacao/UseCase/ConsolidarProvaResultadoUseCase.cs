using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Prova.Aplicacao.Queries.VerificaProvaPossuiTipoDeficiencia;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ConsolidarProvaResultadoUseCase : IConsolidarProvaResultadoUseCase
    {
        private readonly IMediator mediator;
        private readonly IServicoLog serviceLog;
        private readonly IModel model;

        public ConsolidarProvaResultadoUseCase(IMediator mediator, IServicoLog serviceLog, IModel model)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.serviceLog = serviceLog ?? throw new ArgumentNullException(nameof(serviceLog));
            this.model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var extracao = mensagemRabbit.ObterObjetoMensagem<ProvaExtracaoDto>();

            serviceLog.Registrar(LogNivel.Informacao, $"Consolidar dados prova:{extracao.ProvaSerapId}. msg: {mensagemRabbit.Mensagem}");

            var exportacaoResultado = await mediator.Send(new ObterExportacaoResultadoStatusQuery(extracao.ExtracaoResultadoId, extracao.ProvaSerapId));
            if (exportacaoResultado is null)
                throw new NegocioException("A exportação não foi encontrada");

            try
            {
                var checarProvaExiste = await mediator.Send(new VerificaProvaExistePorSerapIdQuery(extracao.ProvaSerapId));
                if (!checarProvaExiste)
                    throw new NegocioException("A prova informada não foi encontrada no serap estudantes");

                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Processando));

                var prova = await mediator.Send(new ObterProvaDetalhesPorProvaLegadoIdQuery(exportacaoResultado.ProvaSerapId));
                var possuiTipoDeficiencia = await mediator.Send(new VerificaProvaPossuiTipoDeficienciaQuery(exportacaoResultado.ProvaSerapId));

                var filtrosParaPublicar = new List<ExportacaoResultadoFiltroDto>();
                
                var dres = await mediator.Send(new ObterDresSerapQuery());
                foreach (var dre in dres)
                {
                    var ues = await mediator.Send(new ObterUesSerapPorProvaSerapEDreCodigoQuery(extracao.ProvaSerapId, dre.CodigoDre));
                    if (ues == null || !ues.Any()) 
                        continue;
                    
                    foreach (var ue in ues)
                    {
                        var ueIds = new[] { ue.CodigoUe };
                        
                        var exportacaoResultadoItem = new ExportacaoResultadoItem(exportacaoResultado.Id, dre.CodigoDre, ueIds);
                        exportacaoResultadoItem.Id = await mediator.Send(new InserirExportacaoResultadoItemCommand(exportacaoResultadoItem));

                        var filtro = new ExportacaoResultadoFiltroDto(exportacaoResultado.Id,
                            exportacaoResultado.ProvaSerapId, exportacaoResultadoItem.Id, dre.CodigoDre, ueIds,
                            !prova.AderirTodos, possuiTipoDeficiencia);
                        filtrosParaPublicar.Add(filtro);
                    }
                }

                if (!filtrosParaPublicar.Any())
                {
                    await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Erro));
                    serviceLog.Registrar(LogNivel.Critico, $"Não foi possível localizar escolas para consolidar os dados da prova: {extracao.ProvaSerapId}. msg: {mensagemRabbit.Mensagem}");
                    return false;
                }

                foreach (var filtro in filtrosParaPublicar)
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ConsolidarProvaResultadoFiltro, filtro));
            }
            catch (Exception ex)
            {
                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Erro));
                await mediator.Send(new ExcluirExportacaoResultadoItemCommand(0, exportacaoResultado.Id));
                serviceLog.Registrar(LogNivel.Critico, $"Erro -- Consolidar os dados da prova. msg: {mensagemRabbit.Mensagem}", ex.Message, ex.StackTrace);
                return false;
            }

            return true;
        }
    }
}