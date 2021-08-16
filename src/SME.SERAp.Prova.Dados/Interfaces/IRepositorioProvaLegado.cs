using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioProvaLegado
    {
        Task<IEnumerable<long>> ObterProvasIdsParaSeremSincronizadasIds(DateTime ultimaAtualizacao);
        Task<ProvaLegadoDetalhesIdDto> ObterDetalhesPorId(long id);
    }
}