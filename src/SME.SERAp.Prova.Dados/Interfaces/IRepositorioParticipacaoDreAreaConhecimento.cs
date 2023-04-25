using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados.Interfaces
{
    public interface IRepositorioParticipacaoDreAreaConhecimento
    {
        Task<long> AlterarAsync(ParticipacaoDreAreaConhecimento participacao);
        Task<long> IncluirAsync(ParticipacaoDreAreaConhecimento participacao);
        Task<ParticipacaoDreAreaConhecimento> ObterParticipacaoDreAreaConhecimento(string edicao, int areaConhecimentoId, string uad_sigla, string anoEscolar);
    }
}
