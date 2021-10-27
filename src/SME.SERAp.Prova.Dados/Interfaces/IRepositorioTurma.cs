using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioTurma : IRepositorioBase<Turma>
    {
        Task<IEnumerable<TurmaSgpDto>> ObterturmasSgpPorUeCodigo(string ueCodigo);
        Task<Turma> ObterturmaPorCodigo(string uecodigo);
        Task<IEnumerable<Turma>> ObterTurmasPorAnoEAnoLetivo(int ano, int anoLetivo);
    }
}