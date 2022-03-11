using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioTurmaAlunoHistorico : IRepositorioBase<TurmaAlunoHistorico>
    {
        Task<IEnumerable<TurmaAlunoHistoricoDto>> ObterTurmaAlunoHistoricoPorAlunosRa(long[] alunosRa);
    }
}
