﻿using SME.SERAp.Prova.Infra;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public interface ITratarProvaResultadoExtracaoUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}
