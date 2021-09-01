using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class EnviarNotificacaoPorAnoCommand : IRequest<bool>
    {
        public EnviarNotificacaoPorAnoCommand(int ano, string mensagem)
        {
            Ano = ano;
            Mensagem = mensagem;
        }

        public int Ano { get; set; }
        public string Mensagem { get; set; }
    }
}
