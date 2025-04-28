using MediatR;
using SME.SERAp.Prova.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao.Queries
{
    public class ObterAlunoComRespostasSemQuestoesQuery : IRequest<IEnumerable<AlunoProvaRespostaSemPerguntaDto>>
    {
        public ObterAlunoComRespostasSemQuestoesQuery(DateTime inicio, DateTime fim, long? alunoRa = null)
        {
            Inicio = inicio;
            Fim = fim;
            AlunoRa = alunoRa;
        }

        public DateTime Inicio { get; set; }

        public DateTime Fim { get; set; }

        public long? AlunoRa { get; set; }
    }
}
