using MediatR;
using SME.SERAp.Prova.Dominio;


namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUsuarioSerapPorIdQuery : IRequest<UsuarioSerapCoreSso>
    {
        
        public ObterUsuarioSerapPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }

    }
}
