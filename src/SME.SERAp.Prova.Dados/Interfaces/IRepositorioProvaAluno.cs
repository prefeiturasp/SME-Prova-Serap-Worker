﻿using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioProvaAluno : IRepositorioBase<ProvaAluno>
    {
        Task<ProvaAluno> ObterPorProvaIdRaAsync(long provaId, long alunoRa);
        Task<ProvaAluno> ObterPorProvaIdRaStatusAsync(long provaId, long alunoRa, int status);
        Task<ProvaAluno> ObterPorQuestaoIdRaAsync(long questaoId, long alunoRa);
        Task<IEnumerable<ProvaAluno>> ObterPorProvaIdsRaAsync(long[] provaIds, long alunoRa);
        Task<IEnumerable<ProvaAlunoReduzidaDto>> ObterAlunosProvasFinalizadasReduzido();
        Task AtualizarFrequenciaAlunoAsync(long id, FrequenciaAluno frequencia);
        Task<int> ExcluirProvaAlunoAsync(long provaId, long alunoRa);


    }
}
