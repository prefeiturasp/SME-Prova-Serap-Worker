using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Dados.Interfaces
{
    public interface IRepositorioElasticTurma : IRepositorioElasticBase<DocumentoElasticTurmaDto>
    {
        Task<IEnumerable<AlunoMatriculaTurmaDreDto>> ObterTurmasAlunoHistoricoPorAlunosRa(long[] alunosRa);
    }
}