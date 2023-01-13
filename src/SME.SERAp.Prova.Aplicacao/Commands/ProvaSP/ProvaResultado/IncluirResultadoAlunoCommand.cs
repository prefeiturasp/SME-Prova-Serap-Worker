using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao.Commands.ProvaSP.ProvaResultado
{
    public class IncluirResultadoAlunoCommand : IRequest<long>
    {
        public IncluirResultadoAlunoCommand(ResultadoAluno resultadoAluno)
        {
            ResultadoAluno = resultadoAluno;
        }

        public ResultadoAluno ResultadoAluno { get; set; }
    }
}