using MediatR;
using SME.SERAp.Prova.Aplicacao.Commands.ConsolidarProvaRespostaAdesaoManual;
using SME.SERAp.Prova.Aplicacao.Commands.ResultadoProvaConsolidado.Incluir;
using SME.SERAp.Prova.Aplicacao.Queries.ObterAlunosResultadoProvaAdesao;
using SME.SERAp.Prova.Aplicacao.Queries.ObterAlunosResultadoProvaDeficiencia;
using SME.SERAp.Prova.Aplicacao.Queries.ObterQuestaoAlunoRespostaPorProvaIdEAlunoRa;
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
        public ConsolidarProvaResultadoUseCase(IMediator mediator, IServicoLog serviceLog)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.serviceLog = serviceLog ?? throw new ArgumentNullException(nameof(serviceLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            // var extracao = mensagemRabbit.ObterObjetoMensagem<ProvaExtracaoDto>();

            var extracao = new ProvaExtracaoDto { ProvaSerapId = 1494, ExtracaoResultadoId = 32 };
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


                var prova = await mediator.Send(new ObterProvaDetalhesPorProvaLegadoIdQuery(exportacaoResultado.ProvaSerapId));
                var possuiTipoDeficiencia = await mediator.Send(new VerificaProvaPossuiTipoDeficienciaQuery(exportacaoResultado.ProvaSerapId));

                // Podia excluir tudo da prova logo de uma vez 

                //IEnumerable<ConsolidadoProvaRespostaDto> AlunosResultadoProva;


                //if (possuiTipoDeficiencia)
                //{
                //    AlunosResultadoProva = await mediator.Send(new ObterAlunosResultadoProvaDeficienciaQuery(exportacaoResultado.ProvaSerapId)); // queryAdesao

                //    foreach(var alunoResult in AlunosResultadoProva)
                //    {
                //        var respostas =  await mediator.Send(new ObterQuestaoAlunoRespostaPorProvaIdEAlunoRaQuery(exportacaoResultado.ProvaSerapId, alunoResult.AlunoCodigoEol)); // queryAdesao 

                //        foreach(var resposta in respostas)
                //        {
                //            var res = new ResultadoProvaConsolidado()
                //            {
                //                AlunoCodigoEol = alunoResult.AlunoCodigoEol,
                //                AlunoDataNascimento = alunoResult.AlunoDataNascimento,
                //                AlunoFrequencia = alunoResult.AlunoFrequencia,
                //                AlunoNome = alunoResult.AlunoNome,
                //                AlunoSexo = alunoResult.AlunoSexo,
                //                DataFim = alunoResult.DataFim,
                //                DataInicio = alunoResult.DataInicio,
                //                DreCodigoEol = alunoResult.DreCodigoEol,
                //                DreNome = alunoResult.DreNome,
                //                DreSigla = alunoResult.DreSigla,
                //                ProvaCaderno = alunoResult.ProvaCaderno,
                //                ProvaComponente = alunoResult.ProvaComponente,
                //                ProvaQuantidadeQuestoes = alunoResult.ProvaQuantidadeQuestoes,
                //                ProvaSerapEstudantesId = alunoResult.ProvaSerapEstudantesId,
                //                ProvaSerapId = alunoResult.ProvaSerapId,
                //                TurmaAnoEscolar = alunoResult.TurmaAnoEscolar,
                //                TurmaAnoEscolarDescricao = alunoResult.TurmaAnoEscolarDescricao,
                //                TurmaCodigo = alunoResult.TurmaCodigo,
                //                TurmaDescricao = alunoResult.TurmaDescricao,
                //                UeCodigoEol = alunoResult.UeCodigoEol,
                //                UeNome = alunoResult.UeNome,
                //                QuestaoId = resposta.QuestaoId,
                //                QuestaoOrdem = resposta.QuestaoOrdem,
                //                Resposta = resposta.Resposta
                //            };

                //            await mediator.Send(new ResultadoProvaConsolidadoCommand(res));
                //        }

                //    }
                   
                //}

                //else if (prova.AderirTodos == false)
                //{
                //    AlunosResultadoProva = await mediator.Send(new ObterAlunosResultadoProvaAdesaoQuery(exportacaoResultado.ProvaSerapId)); // queryAdesao 
                //}

                //else
                //{

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
                                var filtro = new ExportacaoResultadoFiltroDto(exportacaoResultado.Id, exportacaoResultado.ProvaSerapId, exportacaoResultadoItem.Id, dre.CodigoDre, ueIds, prova.AderirTodos, possuiTipoDeficiencia);
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

                    foreach (ExportacaoResultadoFiltroDto filtro in filtrosParaPublicar)
                    {
                        await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ConsolidarProvaResultadoFiltro, filtro));
                    }
                }
            //}
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