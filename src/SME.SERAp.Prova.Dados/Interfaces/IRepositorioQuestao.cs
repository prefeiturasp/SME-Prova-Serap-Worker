﻿using System.Threading.Tasks;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioQuestao : IRepositorioBase<Questao>
    {
        Task<Questao> ObterPorIdLegadoAsync(long id);
        Task<Questao> ObterPorIdEProvaIdLegadoAsync(long id, long provaId);
        Task<bool> RemoverPorProvaIdAsync(long provaId);
    }
}
