using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class QuestaoParaAtualizarCommand : IRequest<long>
    {
        public Questao Questao { get; set; }

        public QuestaoParaAtualizarCommand(Questao questao)
        {
            Questao = questao;
        }
    }
}