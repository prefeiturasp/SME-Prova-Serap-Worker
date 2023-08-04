using MediatR;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AjustarUeTurmasesUseCase : AbstractUseCase, IAjustarUeTurmasUseCase
    {
        public AjustarUeTurmasesUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var turmas = mensagemRabbit.ObterObjetoMensagem<List<TurmaParaSincronizacaoInstitucionalDto>>();
            
            if (turmas == null || !turmas.Any())
                throw new NegocioException("Não foi possível localizar as turmas para atualizar as ues.");

            var listaParaAlterar = new List<Turma>();

            foreach (var turma in turmas)
            {
                var turmaSgp = await mediator.Send(new ObterTurmaSgpPorCodigoQuery(turma.Codigo));
                
                if (turmaSgp == null)
                    continue;

                if (turma.UeCodigo == turmaSgp.UeCodigo)
                    continue;
                
                var ueCorretaSerap = await mediator.Send(new ObterUePorCodigoQuery(turma.UeCodigo));

                if (ueCorretaSerap == null)
                    continue;
                    
                var turmaParaAtualizarUe = await mediator.Send(new ObterTurmaSerapPorIdQuery(turma.Id));
                    
                if (turmaParaAtualizarUe == null)
                    continue;
                    
                turmaParaAtualizarUe.UeId = ueCorretaSerap.Id;
                turmaParaAtualizarUe.DataAtualizacao = DateTime.Now;
                    
                listaParaAlterar.Add(turmaParaAtualizarUe);
            }
            
            if (listaParaAlterar.Any())
                await mediator.Send(new AlterarTurmasCommand(listaParaAlterar));

            return true;
        }
    }
}
