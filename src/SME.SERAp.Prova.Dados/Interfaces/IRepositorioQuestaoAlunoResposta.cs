using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.Dtos;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioQuestaoAlunoResposta : IRepositorioBase<QuestaoAlunoResposta>
    {
        Task<QuestaoAlunoResposta> ObterPorIdRaAsync(long questaoId, long alunoRa);
        Task<IEnumerable<AlunoProvaRespostaSemPerguntaDto>> ObterAlunoComRespostasSemQuestoes(DateTime inicio, DateTime fim, long? alunoRa);
        Task<IEnumerable<QuestaoAlunoRespostaComOrdemDto>> ObterQuestoesAlunoRespostaComOrdem(long alunoRa, long provaId);
    }
}