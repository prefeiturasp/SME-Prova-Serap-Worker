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
            try
            {
                var grupos = await mediator.Send(new ObterGruposSerapCoreSsoQuery());
                foreach (GrupoSerapCoreSso grupo in grupos)
                {
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.UsuarioPorGrupoCoreSsoTratar, grupo));
                }
                return true;
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureMessage($"Sync usuários dos grupos.", SentryLevel.Error);
                SentrySdk.CaptureException(ex);
                return false;
            }            
        }
    }
}
