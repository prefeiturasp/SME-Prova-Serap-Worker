using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Sentry;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;

namespace SME.SERAp.Prova.Aplicacao
{
    public class IncluirPreferenciasAlunoUseCase : IIncluirPreferenciasAlunoUseCase
    {

        private readonly IMediator mediator;

        public IncluirPreferenciasAlunoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dto = mensagemRabbit.ObterObjetoMensagem<PreferenciaUsuarioDto>();

            var usuario = await mediator.Send(new ObterUsuarioPorLoginQuery(dto.AlunoRA));
            if (usuario == null)
                throw new NegocioException("Usuário não encontrado");

            var preferenciasUsuario = await mediator.Send(new ObterPreferenciasUsuarioPorLoginQuery(dto.AlunoRA));

            if (preferenciasUsuario == null)
            {

                return await mediator.Send(new IncluirPreferenciasUsuarioCommand(dto.TamanhoFonte,
                    (FamiliaFonte)dto.FamiliaFonte, usuario.Id));
            }
            else
            {
                preferenciasUsuario.FamiliaFonte = (FamiliaFonte)dto.FamiliaFonte;
                preferenciasUsuario.TamanhoFonte = dto.TamanhoFonte;

                return await mediator.Send(new AtualizarPreferenciasUsuarioCommand(preferenciasUsuario));
            }
        }
    }
}