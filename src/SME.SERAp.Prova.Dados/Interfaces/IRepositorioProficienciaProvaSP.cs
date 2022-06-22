using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioProficienciaProvaSP
    {
        Task<decimal> ObterProficienciaAlunoPorAreaConhecimento(string alunoRa, long areaConhecimentoId);
        Task<decimal> ObterMediaProficienciaEscolaAluno(string alunoRa, long areaConhecimentoId);
        Task<decimal> ObterMediaProficienciaDre(string dreSigla, string anoEscolar, long areaConhecimentoId);
    }
}
