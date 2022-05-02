using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlterarAbrangenciaCommand : IRequest<bool>
    {
        public AlterarAbrangenciaCommand(Abrangencia abrangencia)
        {
            Abrangencia = abrangencia;
        }

        public Abrangencia Abrangencia { get; set; }
    }
}
