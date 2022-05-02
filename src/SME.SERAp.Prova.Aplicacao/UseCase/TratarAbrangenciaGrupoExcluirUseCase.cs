using MediatR;
using Sentry;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarAbrangenciaGrupoExcluirUseCase : AbstractUseCase, ITratarAbrangenciaGrupoExcluirUseCase
    {
        public TratarAbrangenciaGrupoExcluirUseCase(IMediator mediator) : base(mediator) { }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var grupo = mensagemRabbit.ObterObjetoMensagem<GrupoSerapCoreSso>();

            var abrangenciaGrupo = await mediator.Send(new ObterAbrangenciaPorGrupoIdQuery(grupo.Id));
            foreach (Abrangencia abrangencia in abrangenciaGrupo)
            {
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.UsuarioGrupoAbrangenciaExcluirTratar, abrangencia));
            }

            return true;
        }
    }
}
