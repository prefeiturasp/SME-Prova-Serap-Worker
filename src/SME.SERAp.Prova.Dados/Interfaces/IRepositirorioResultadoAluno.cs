using SME.SERAp.Prova.Dominio;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados.Interfaces
{
    public interface IRepositorioResultadoAluno
    {
        Task<ResultadoAluno> ObterProficienciaAluno(string edicao, string alunoMatricula, long turId, long areaConhecimentoId);
        Task<long> IncluirAsync(ResultadoAluno resultado);
        Task<long> AlterarAsync(ResultadoAluno resultado);
    };
}
