using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados.Interfaces
{
    public interface IRepositorioParticipacaoTurma : IRepositorioProvaSpBase
    {
        Task<ParticipacaoTurma> ObterParticipacaoTurma(string edicao, string uad_sigla, string esc_codigo, string anoEscolar, string tur_codigo);
        Task<long> IncluirAsync(ParticipacaoTurma resultado);
        Task<long> AlterarAsync(ParticipacaoTurma resultado);
    }
}
