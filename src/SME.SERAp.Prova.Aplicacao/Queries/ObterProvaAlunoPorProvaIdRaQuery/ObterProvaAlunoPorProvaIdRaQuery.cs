using MediatR;
using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvaAlunoPorProvaIdRaQuery : IRequest<ProvaAluno>
    {
        public long ProvaId { get; set; }
        public long AlunoRa { get; set; }
        public ObterProvaAlunoPorProvaIdRaQuery(long provaId, long alunoRa)
        {
            ProvaId = provaId;
            AlunoRa = alunoRa;
        }
    }
}