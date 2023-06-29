using MediatR;
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
        public ConsolidarProvaResultadoUseCase(IMediator mediator, IServicoLog serviceLog)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.serviceLog = serviceLog ?? throw new ArgumentNullException(nameof(serviceLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var extracao = mensagemRabbit.ObterObjetoMensagem<ProvaExtracaoDto>();
            serviceLog.Registrar(LogNivel.Informacao, $"Consolidar dados prova:{extracao.ProvaSerapId}. msg: {mensagemRabbit.Mensagem}");
            var exportacaoResultado = await mediator.Send(new ObterExportacaoResultadoStatusQuery(extracao.ExtracaoResultadoId, extracao.ProvaSerapId));

            try
            {
                if (exportacaoResultado is null)
                    throw new NegocioException("A exportação não foi encontrada");

                var checarProvaExiste = await mediator.Send(new VerificaProvaExistePorSerapIdQuery(extracao.ProvaSerapId));

                if (!checarProvaExiste)
                    throw new NegocioException("A prova informada não foi encontrada no serap estudantes");

                if (exportacaoResultado.Status == ExportacaoResultadoStatus.Erro || exportacaoResultado.Status != ExportacaoResultadoStatus.Processando) return false;

                string fila = string.Empty;
                var linhasAfetadas = await mediator.Send(new ConsolidarProvaRespostaCommand(extracao.ProvaSerapId, extracao.AderirTodos, extracao.ParaEstudanteComDeficiencia, extracao.Take, extracao.Skip));
                if (linhasAfetadas > 0)
                {
                    extracao.Skip += extracao.Take;
                    fila = RotasRabbit.ConsolidarProvaResultado;
                }
                else
                {
                    extracao.Skip = 0;
                    fila = RotasRabbit.ExtrairResultadosProva;
                }

                await mediator.Send(new PublicaFilaRabbitCommand(fila, extracao));
                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Erro));
                serviceLog.Registrar(LogNivel.Critico, $"Erro -- Consolidar os dados da prova. msg: {mensagemRabbit.Mensagem}", ex.Message, ex.StackTrace);
                return false;
            }
        }

        public async Task<bool> Executar_hold(MensagemRabbit mensagemRabbit)
        {
            var extracao = mensagemRabbit.ObterObjetoMensagem<ProvaExtracaoDto>();

            serviceLog.Registrar(LogNivel.Informacao, $"Consolidar dados prova:{extracao.ProvaSerapId}. msg: {mensagemRabbit.Mensagem}");

            var exportacaoResultado = await mediator.Send(new ObterExportacaoResultadoStatusQuery(extracao.ExtracaoResultadoId, extracao.ProvaSerapId));
            try
            {
                if (exportacaoResultado is null)
                    throw new NegocioException("A exportação não foi encontrada");

                var checarProvaExiste = await mediator.Send(new VerificaProvaExistePorSerapIdQuery(extracao.ProvaSerapId));

                if (!checarProvaExiste)
                    throw new NegocioException("A prova informada não foi encontrada no serap estudantes");

                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Processando));

                var filtrosParaPublicar = new List<ExportacaoResultadoFiltroDto>();
                var dres = await mediator.Send(new ObterDresSerapQuery());
                foreach (Dre dre in dres)
                {
                    var ues = await mediator.Send(new ObterUesSerapPorProvaSerapEDreCodigoQuery(extracao.ProvaSerapId, dre.CodigoDre));
                    if (ues != null && ues.Any())
                    {
                        foreach (Ue ue in ues)
                        {
                            var ueIds = new string[] { ue.CodigoUe };
                            var exportacaoResultadoItem = new ExportacaoResultadoItem(exportacaoResultado.Id, dre.CodigoDre, ueIds);
                            exportacaoResultadoItem.Id = await mediator.Send(new InserirExportacaoResultadoItemCommand(exportacaoResultadoItem));
                            var filtro = new ExportacaoResultadoFiltroDto(exportacaoResultado.Id, exportacaoResultado.ProvaSerapId, exportacaoResultadoItem.Id, dre.CodigoDre, ueIds);
                            filtrosParaPublicar.Add(filtro);
                        }
                    }
                }

                if (!filtrosParaPublicar.Any())
                {
                    await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Erro));
                    serviceLog.Registrar(LogNivel.Critico, $"Não foi possível localizar escolas para consolidar os dados da prova: {extracao.ProvaSerapId}. msg: {mensagemRabbit.Mensagem}");
                    return false;
                }

                //foreach (ExportacaoResultadoFiltroDto filtro in filtrosParaPublicar)
                //{
                //    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ConsolidarProvaResultadoFiltro, filtro));
                //}
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