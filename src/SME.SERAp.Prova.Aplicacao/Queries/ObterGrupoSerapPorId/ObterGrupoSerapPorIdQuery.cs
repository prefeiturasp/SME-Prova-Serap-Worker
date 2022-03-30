using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterGrupoSerapPorIdQuery : IRequest<GrupoSerapCoreSso>
    {
        public ObterGrupoSerapPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }    
}
