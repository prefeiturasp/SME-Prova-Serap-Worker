using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class QuestaoParaIncluirCommand : IRequest<long>
    {
        public Questao Questao { get; set; }

        public QuestaoParaIncluirCommand(Questao questao)
        {
            Questao = questao;
        }
    }
}