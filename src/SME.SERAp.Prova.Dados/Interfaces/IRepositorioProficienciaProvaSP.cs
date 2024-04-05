using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioProficienciaProvaSP
    {
        Task<decimal> ObterProficienciaAluno(string alunoRa, string codigoAnoTurma, string anoTurma, string codigoEscola, long areaConhecimentoId);
        Task<decimal> ObterMediaProficienciaEscolaAluno(string alunoRa, long areaConhecimentoId);
        Task<decimal> ObterMediaProficienciaDre(string dreSigla, string anoEscolar, long areaConhecimentoId);
    }
}
