using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmaPorCodigoQuery : IRequest<TurmaAtribuicaoDto>
    {
        public ObterTurmaPorCodigoQuery(int anoLetivo, string codigo)
        {
            AnoLetivo = anoLetivo;
            Codigo = codigo;
        }

        public int AnoLetivo { get; set; }
        public string Codigo { get; set; }
    }
}
