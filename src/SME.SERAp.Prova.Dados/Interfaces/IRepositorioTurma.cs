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
        Task<IEnumerable<Turma>> ObterTurmasPorAnoEAnoLetivo(string ano, int anoLetivo);
        Task<long> InserirOuAtualizarTurmaAsync(TurmaSgpDto turmaSgp);
        Task<IEnumerable<TurmaSgpDto>> ObterTurmasSgpPorDreCodigoAsync(string dreCodigo);
        Task<IEnumerable<TurmaSgpDto>> ObterTurmasSerapPorDreCodigoAsync(string dreCodigo);
        Task<IEnumerable<Turma>> ObterTodasPorAnoAsync(int year);
        Task<IEnumerable<Turma>> ObterTurmasPorCodigoUeEAnoLetivo(string codigoUe, int anoLetivo);
    }
}