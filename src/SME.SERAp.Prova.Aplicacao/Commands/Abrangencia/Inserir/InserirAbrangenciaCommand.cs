using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InserirAbrangenciaCommand : IRequest<bool>
    {
        
        public InserirAbrangenciaCommand(Abrangencia abrangencia)
        {
            Abrangencia = abrangencia;
        }

        public Abrangencia Abrangencia { get; set; }

    }
}
