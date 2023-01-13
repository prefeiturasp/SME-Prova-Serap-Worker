using MediatR;
using SME.SERAp.Prova.Dominio.Entidades;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTipoResultadoPspQuery : IRequest<ArquivoResultadoPspDto>
    {
        public ObterTipoResultadoPspQuery(long id)
        {
            Id = id;
        }
        public long Id { get; set; }
    }
}