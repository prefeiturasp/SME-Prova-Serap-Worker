using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmaSgpPorCodigoQueryHandler : IRequestHandler<ObterTurmaSgpPorCodigoQuery, TurmaSgpDto>
    {

        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmaSgpPorCodigoQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<TurmaSgpDto> Handle(ObterTurmaSgpPorCodigoQuery request, CancellationToken cancellationToken)
            => await repositorioTurma.ObterTurmaSgpPorCodigoAsync(request.CodigoTurma);
    }
}
