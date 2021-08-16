using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaAnoIncluirCommand : IRequest<long>
    {
        public ProvaAnoIncluirCommand(ProvaAno provaAno)
        {
            ProvaAno = provaAno;
        }

        public ProvaAno ProvaAno { get; set; }
    }
}
