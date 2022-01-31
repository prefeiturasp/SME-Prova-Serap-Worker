using MediatR;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvasIniciadasPorModalidadeQuery : IRequest<IEnumerable<ProvaAlunoDto>>
    {
        public ObterProvasIniciadasPorModalidadeQuery(int modalidade)
        {
            Modalidade = modalidade;
        }
        public int Modalidade { get; set; }
    }
}
