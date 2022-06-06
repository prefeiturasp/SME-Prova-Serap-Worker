using MediatR;
using Sentry;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExecutarSincronizacaoUsuarioCoreSsoUseCase : AbstractUseCase, IExecutarSincronizacaoUsuarioCoreSsoUseCase
    {
        public ExecutarSincronizacaoUsuarioCoreSsoUseCase(IMediator mediator) : base(mediator){}

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var grupos = await mediator.Send(new ObterGruposSerapCoreSsoQuery());
            foreach (GrupoSerapCoreSso grupo in grupos)
            {
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.UsuarioPorGrupoCoreSsoTratar, grupo));
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.UsuarioGrupoCoreSsoExcluirTratar, grupo));
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.GrupoAbrangenciaExcluir, grupo));
            }
            return true;
        }
    }
}
