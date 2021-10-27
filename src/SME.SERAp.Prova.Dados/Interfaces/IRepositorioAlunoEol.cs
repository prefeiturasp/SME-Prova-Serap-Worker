﻿using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioAlunoEol
    {
        //Task<ObterAlunoAtivoEolRetornoDto> ObterAlunoAtivoAsync(long alunoRA);

        Task<IEnumerable<AlunoEolDto>> ObterAlunosPorTurmaCodigoAsync(long turmaCodigo);
    }
}
