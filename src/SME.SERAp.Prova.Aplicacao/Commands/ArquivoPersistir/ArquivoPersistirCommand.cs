using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ArquivoPersistirCommand : IRequest<long>
    {
        public ArquivoPersistirCommand(Arquivo arquivo)
        {
            Arquivo = arquivo;
        }

        public Arquivo Arquivo { get; set; }
    }
}
