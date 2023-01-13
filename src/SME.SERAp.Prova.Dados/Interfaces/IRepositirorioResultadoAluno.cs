using SME.SERAp.Prova.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados.Interfaces
{
    public interface IRepositorioResultadoAluno
    {
        Task<ResultadoAluno> ObterProficienciaAluno(string edicao, string alunoMatricula, long turId, long areaConhecimentoId);
        Task<long> IncluirAsync(ResultadoAluno resultado);
    };
}
