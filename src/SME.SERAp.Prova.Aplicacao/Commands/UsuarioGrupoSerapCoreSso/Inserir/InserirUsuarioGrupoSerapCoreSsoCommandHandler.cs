using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InserirUsuarioGrupoSerapCoreSsoCommandHandler : IRequestHandler<InserirUsuarioGrupoSerapCoreSsoCommand, long>
    {

        private readonly IRepositorioUsuarioGrupoSerapCoreSso repositorioUsuarioGrupoSerapCoreSso;

        public InserirUsuarioGrupoSerapCoreSsoCommandHandler(IRepositorioUsuarioGrupoSerapCoreSso repositorioUsuarioGrupoSerapCoreSso)
        {
            this.repositorioUsuarioGrupoSerapCoreSso = repositorioUsuarioGrupoSerapCoreSso ?? throw new System.ArgumentNullException(nameof(repositorioUsuarioGrupoSerapCoreSso));
        }

        public async Task<long> Handle(InserirUsuarioGrupoSerapCoreSsoCommand request, CancellationToken cancellationToken)
        {
            return await repositorioUsuarioGrupoSerapCoreSso.IncluirAsync(request.UsuarioGrupo);
        }
    }
}
