using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InserirUsuarioGrupoSerapCoreSsoCommandHandler : IRequestHandler<InserirUsuarioGrupoSerapCoreSsoCommand, long>
    {

        private readonly IRepositorioUsuarioGrupoSerapCoreSso usuarioGrupoSerapCoreSso;

        public InserirUsuarioGrupoSerapCoreSsoCommandHandler(IRepositorioUsuarioGrupoSerapCoreSso usuarioGrupoSerapCoreSso)
        {
            this.usuarioGrupoSerapCoreSso = usuarioGrupoSerapCoreSso ?? throw new System.ArgumentNullException(nameof(usuarioGrupoSerapCoreSso));
        }

        public async Task<long> Handle(InserirUsuarioGrupoSerapCoreSsoCommand request, CancellationToken cancellationToken)
        {
            return await usuarioGrupoSerapCoreSso.IncluirAsync(request.UsuarioGrupo);
        }
    }
}
