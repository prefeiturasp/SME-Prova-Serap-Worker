using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Interfaces
{
    public interface ITratarAlunoResultadoProvaQuestoesUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}
