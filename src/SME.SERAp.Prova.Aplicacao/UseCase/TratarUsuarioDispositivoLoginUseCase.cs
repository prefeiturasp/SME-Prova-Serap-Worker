using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.UseCase
{
    public class TratarUsuarioDispositivoLoginUseCase : AbstractUseCase, ITratarUsuarioDispositivoLoginUseCase
    {

        public TratarUsuarioDispositivoLoginUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dto = mensagemRabbit.ObterObjetoMensagem<UsuarioDispositivoLoginDto>();
            if (dto == null) return false;

            var usuarioDispositivo = new UsuarioDispositivo(dto.Ra, 
                                                            dto.DispositivoId != null ? dto.DispositivoId : string.Empty,
                                                            dto.CriadoEm, 
                                                            dto.TurmaId);

            return await mediator.Send(new InserirUsuarioDispositivoCommand(usuarioDispositivo));
        }
    }
}
