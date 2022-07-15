using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AjustarUeTurmasUseCase : AbstractUseCase, IAjustarUeTurmasUseCase
    {
        public AjustarUeTurmasUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {

            var anoLetivo = DateTime.Now.Year;
            var dre = mensagemRabbit.ObterObjetoMensagem<DreParaSincronizacaoInstitucionalDto>();
            if (dre == null)
            {
                var mensagem = $"Não foi possível fazer parse da mensagem para atualizar ue das turmas da dre {mensagemRabbit.Mensagem}.";
                throw new NegocioException(mensagem);
            }

            var uesSerap = await mediator.Send(new ObterUesSerapPorDreCodigoQuery(dre.DreCodigo));
            if (uesSerap != null && uesSerap.Any())
            {
                foreach (Ue ue in uesSerap)
                {
                    var turmasSerap = await mediator.Send(new ObterTurmasPorCodigoUeEAnoLetivoQuery(ue.CodigoUe, anoLetivo));
                    var listaParaAlterar = new List<Turma>();

                    foreach (Turma turma in turmasSerap)
                    {
                        var turmaSgp = await mediator.Send(new ObterTurmaSgpPorCodigoQuery(turma.Codigo));
                        if (turmaSgp == null)
                            continue;

                        if (ue.CodigoUe != turmaSgp.UeCodigo)
                        {
                            var ueCorretaSerap = await mediator.Send(new ObterUePorCodigoQuery(turmaSgp.UeCodigo));

                            if (ueCorretaSerap == null)
                                continue;

                            var turmaParaAtualizarUe = await mediator.Send(new ObterTurmaSerapPorIdQuery(turma.Id));

                            turmaParaAtualizarUe.UeId = ueCorretaSerap.Id;
                            turmaParaAtualizarUe.DataAtualizacao = DateTime.Now;

                            listaParaAlterar.Add(turmaParaAtualizarUe);
                        }
                    }

                    if (listaParaAlterar.Any())
                        await mediator.Send(new AlterarTurmasCommand(listaParaAlterar));
                }

            }

            return true;
        }
    }
}
