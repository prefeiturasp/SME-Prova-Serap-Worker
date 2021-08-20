using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlternativasParaIncluirCommand : IRequest<long>
    {
        public AlternativasParaIncluirCommand(Alternativas alternativas)
        {
            Alternativa = alternativas;
        }

        public Alternativas Alternativa { get; set; }
    }
}