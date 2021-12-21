using SME.SERAp.Prova.Dominio;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioFrequenciaAlunoSgp
    {
        Task<FrequenciaAluno> ObterFrequenciaAlunoPorRAEData(long alunoRa, DateTime data);
    }
}