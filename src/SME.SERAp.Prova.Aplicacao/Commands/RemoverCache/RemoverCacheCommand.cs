using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class RemoverCacheCommand : IRequest<bool>
    {
        public RemoverCacheCommand(string nomeChave)
        {
            NomeChave = nomeChave;
        }

        public string NomeChave { get; set; }
    }
}
