using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class GerarCSVExtracaoProvaCommand : IRequest<bool>
    {
        public GerarCSVExtracaoProvaCommand(int quantidadeQuestoes, string nomeArquivo)
        {
            QuantidadeQuestoes = quantidadeQuestoes;
            NomeArquivo = nomeArquivo;
        }

        public int QuantidadeQuestoes { get; set; }
        public string NomeArquivo { get; set; }
    }
}
