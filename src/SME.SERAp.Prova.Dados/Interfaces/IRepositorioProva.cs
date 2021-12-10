using SME.SERAp.Prova.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioProva : IRepositorioBase<Dominio.Prova>
    {
        Task<Dominio.Prova> ObterPorIdLegadoAsync(long id);
        Task<bool> VerificaSeExisteRespostasPorId(long id);
        Task<IEnumerable<ProvaAlunoDto>> ObterProvasIniciadasPorModalidadeAsync(int modalidade);
        Task<bool> FinalizarProvaAsync(ProvaParaAtualizarDto provaParaAtualizar);
    }
}
