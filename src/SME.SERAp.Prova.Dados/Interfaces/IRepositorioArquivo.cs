﻿using SME.SERAp.Prova.Dominio;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioArquivo : IRepositorioBase<Arquivo>
    {
        Task<bool> RemoverPorIdsAsync(long[] idsArquivos);
        Task<long> ObterIdArquivoPorCaminho(string caminho);
        Task<long> ObterIdArquivoPorCaminhoLegadoId(string caminho, long legadoId);
    }
}
