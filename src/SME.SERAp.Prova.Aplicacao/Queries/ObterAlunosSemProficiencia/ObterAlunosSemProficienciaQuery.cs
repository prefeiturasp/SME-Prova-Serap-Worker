using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunosSemProficienciaQuery : IRequest<IEnumerable<AlunoProvaDto>>
    {
    }
}
