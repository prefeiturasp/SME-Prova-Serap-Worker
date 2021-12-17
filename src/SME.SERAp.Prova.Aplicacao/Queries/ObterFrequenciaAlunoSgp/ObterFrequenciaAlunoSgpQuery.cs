using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using System;
using System.Collections.Generic;

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
