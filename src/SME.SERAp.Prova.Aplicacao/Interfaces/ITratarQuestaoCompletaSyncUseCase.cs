﻿using SME.SERAp.Prova.Infra;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public interface ITratarQuestaoCompletaSyncUseCase
    {
        public Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}
