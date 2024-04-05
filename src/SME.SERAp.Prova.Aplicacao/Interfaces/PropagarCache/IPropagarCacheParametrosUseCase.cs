﻿using System.Threading.Tasks;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public interface IPropagarCacheParametrosUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);        
    }
}