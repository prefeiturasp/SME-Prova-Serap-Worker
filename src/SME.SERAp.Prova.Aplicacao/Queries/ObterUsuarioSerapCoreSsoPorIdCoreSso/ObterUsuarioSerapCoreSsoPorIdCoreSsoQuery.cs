using MediatR;
using SME.SERAp.Prova.Dominio;
using System;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUsuarioSerapCoreSsoPorIdCoreSsoQuery : IRequest<UsuarioSerapCoreSso>
    {
        public ObterUsuarioSerapCoreSsoPorIdCoreSsoQuery(Guid idCoreSso)
        {
            IdCoreSso = idCoreSso;
        }

        public Guid IdCoreSso { get; set; }
    }
}
