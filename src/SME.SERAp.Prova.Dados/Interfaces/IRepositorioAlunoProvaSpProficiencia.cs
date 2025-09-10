using SME.SERAp.Prova.Dominio.Entidades;
using SME.SERAp.Prova.Infra.Dtos;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados.Interfaces
{
    public interface IRepositorioAlunoProvaSpProficiencia : IRepositorioBase<AlunoProvaSpProficiencia>
    {
        Task<ResultadoAlunoProvaSpDto> ObterResultadoAlunoProvaSp(int edicao, int areaDoConhecimento, string alunoMatricula);

        Task<AlunoProvaSpProficiencia> ObterAlunoProvaSpProficiencia(int anoLetivo, long disciplinaId, long alunoRa);

        Task<int> ExcluirAlunoProvaSpProficiencia(int anoLetivo, long disciplinaId, long alunoRa);
    }
}
