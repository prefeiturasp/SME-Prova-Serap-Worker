using System.Collections.Generic;
using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterDadosAlunosParaAdesaoPorRaQuery : IRequest<IEnumerable<ProvaAdesao>>
    {
        public long[] AlunosRa { get; private set; }
        public ObterDadosAlunosParaAdesaoPorRaQuery(long[] alunosRa)
        {
            AlunosRa = alunosRa;
        }
    }
}
