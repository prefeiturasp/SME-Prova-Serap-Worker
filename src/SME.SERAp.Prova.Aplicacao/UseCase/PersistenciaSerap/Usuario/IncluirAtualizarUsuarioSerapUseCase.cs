using MediatR;
using SME.SERAp.Prova.Aplicacao.Commands;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class IncluirAtualizarUsuarioSerapUseCase : AbstractUseCase, IIncluirUsuarioSerapUseCase
    {
        IRepositorioCache repositorioCache;
        public IncluirAtualizarUsuarioSerapUseCase(IMediator mediator, IRepositorioCache repositorioCache) : base(mediator)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var usuarioDto = mensagemRabbit.ObterObjetoMensagem<UsuarioDto>();

            var usuario = await mediator.Send(new ObterUsuarioPorLoginQuery(usuarioDto.Login));
            if (usuario == null)
                usuario = new Dominio.Usuario(usuarioDto.Nome, usuarioDto.Login);
            else
            {
                usuario.AtualizaUltimoLogin();
            }
            var idUsuario = await mediator.Send(new IncluirUsuarioCommand(usuario));
            usuario.Id = idUsuario;

            await repositorioCache.SalvarRedisAsync(usuarioDto.Login.ToString(), usuario);
            return true;

        }
    }
}
