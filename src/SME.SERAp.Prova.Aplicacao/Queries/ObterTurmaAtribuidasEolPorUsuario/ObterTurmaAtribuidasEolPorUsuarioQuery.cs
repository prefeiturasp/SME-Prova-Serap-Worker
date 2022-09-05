using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmaAtribuidasEolPorUsuarioQuery : IRequest<IEnumerable<TurmaAtribuicaoEolDto>>
    {
        public ObterTurmaAtribuidasEolPorUsuarioQuery(string login, long? turmaCodigo = null, int? anoLetivo = null)
        {
            Login = login;
            TurmaCodigo = turmaCodigo;
            AnoLetivo = anoLetivo;
        }

        public string Login { get; set; }
        public long? TurmaCodigo { get; set; }
        public int? AnoLetivo { get; set; }
    }
}
