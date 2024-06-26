﻿using SME.SERAp.Prova.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioGeralEol
    {
        Task<IEnumerable<TurmaAtribuicaoEolDto>> ObterTurmaAtribuicaoEol(int anoInicial, string codigoRf, int[] tiposEscola, long? turmaCodigo, int? anoLetivo);
        Task<IEnumerable<string>> ObterUeDreAtribuidasEolAsync(string codigoRf, int[] tiposEscola);
        Task<IEnumerable<TurmaAtribuicaoEolDto>> ObterAtribuicoesEolAsync(string codigoRf, int anoInicial, int[] tiposEscola);        
    }
}
