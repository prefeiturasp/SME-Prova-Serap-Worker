using SME.SERAp.Prova.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioResultadoProvaConsolidado
    {
        Task<IEnumerable<ConsolidadoProvaRespostaDto>> ObterExtracaoProvaRespostaFuncao(long provaSerapId, string dreCodigoEol, string ueCodigoEol);
        Task<IEnumerable<ConsolidadoProvaRespostaDto>> ObterExtracaoProvaRespostaQuery(long provaSerapId, string dreCodigoEol, string ueCodigoEol, string[] turmasCodigosEol = null);
        Task<bool> VerificaResultadoExtracaoProvaExiste(long provaLegadoId);
        Task ExcluirResultadoProvaAlunoTurma(long provaLegadoId, long alunoCodigoEol, string turmaCodigo);
        Task<IEnumerable<string>> ObterTurmasResultadoProvaAluno(long provaLegadoId, long alunoCodigoEol);
    }
}
