using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InserirUsuarioGrupoSerapCoreSsoCommand : IRequest<long>
    {
        public InserirUsuarioGrupoSerapCoreSsoCommand(UsuarioGrupoSerapCoreSso usuarioGrupo)
        {
            UsuarioGrupo = usuarioGrupo;
        }

        public UsuarioGrupoSerapCoreSso UsuarioGrupo { get; set; }
    }
}
