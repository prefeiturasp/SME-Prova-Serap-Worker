using SME.SERAp.Prova.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioTurmaEol
    {
        Task<IEnumerable<TurmaEolDto>> ObterTurmasAlunoHistoricoPorAlunosRa(long[] alunosRa);
    }
}
