using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmasPorCodigoUeEAnoLetivoQuery : IRequest<IEnumerable<Turma>>
    {
        public ObterTurmasPorCodigoUeEAnoLetivoQuery(string codigoUe, int anoLetivo)
        {
            CodigoUe = codigoUe;
            AnoLetivo = anoLetivo;
        }

        public string CodigoUe { get; set; }
        public int AnoLetivo { get; set; }
    }
}
