using MediatR;
using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao
{
   public class IncluirProvaAlunoReaberturaCommand : IRequest<long>
    {
        public IncluirProvaAlunoReaberturaCommand(ProvaAlunoReabertura provaAlunoReabertura)
        {
            ProvaAlunoReabertura = provaAlunoReabertura;
        }
        public ProvaAlunoReabertura ProvaAlunoReabertura { get; set; }
    }
}
