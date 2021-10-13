using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlternativaIncluirCommand : IRequest<long>
    {
        public AlternativaIncluirCommand(Alternativa alternativas)
        {
            Alternativa = alternativas;
        }

        public Alternativa Alternativa { get; set; }
    }
}