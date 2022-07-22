using MediatR;
using SME.SERAp.Prova.Dominio;
using System;

namespace SME.SERAp.Prova.Aplicacao
{ 
    public class IncluirProvaAlunoCommand : IRequest<long>
    {
        public IncluirProvaAlunoCommand(ProvaAluno provaAluno)
        {
            ProvaAluno = provaAluno;
        }
        public ProvaAluno ProvaAluno { get; set; }
    }
}
