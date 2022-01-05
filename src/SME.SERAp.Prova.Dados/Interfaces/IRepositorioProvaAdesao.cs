using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioProvaAdesao : IRepositorioBase<ProvaAdesao>
    {
        Task<IEnumerable<ProvaAdesao>> ObterDadosAlunosParaAdesaoPorRa(long[] alunosRa);
        Task ExcluirAdesaoPorProvaId(long provaId);
    }
}
