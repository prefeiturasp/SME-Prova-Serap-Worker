using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados.Interfaces
{
    public interface IRepositorioParticipacaoEscola : IRepositorioProvaSpBase
    {
        Task<ParticipacaoEscola> ObterParticipacaoEscola(string edicao, string uad_sigla, string esc_codigo, string anoEscolar);
        Task<long> IncluirAsync(ParticipacaoEscola resultado);
        Task<long> AlterarAsync(ParticipacaoEscola resultado);
    }
}
