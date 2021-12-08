using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlternativaParaAtualizarCommand : IRequest<long>
    {
        public Alternativa Alternativa { get; set; }

        public AlternativaParaAtualizarCommand(Alternativa alternativa)
        {
            Alternativa = alternativa;
        }
    }
}