using MediatR;
using SME.SERAp.Prova.Dominio;
using System;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterFrequenciaAlunoSgpQuery : IRequest<FrequenciaAluno>
    {
        public ObterFrequenciaAlunoSgpQuery(long alunoRa, DateTime data)
        {
            AlunoRa = alunoRa;
            Data = data;
        }

        public long AlunoRa { get; set; }
        public DateTime Data { get; set; }
    }
}
