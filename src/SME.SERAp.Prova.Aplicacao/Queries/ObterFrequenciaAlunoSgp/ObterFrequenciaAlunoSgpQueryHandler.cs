using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterFrequenciaAlunoSgpQueryHandler : IRequestHandler<ObterFrequenciaAlunoSgpQuery, FrequenciaAluno>
    {
        private readonly IRepositorioFrequenciaAlunoSgp repositorioFrequenciaAlunoSgp;

        public ObterFrequenciaAlunoSgpQueryHandler(IRepositorioFrequenciaAlunoSgp repositorioFrequenciaAlunoSgp)
        {
            this.repositorioFrequenciaAlunoSgp = repositorioFrequenciaAlunoSgp ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoSgp));
        }

        public async Task<FrequenciaAluno> Handle(ObterFrequenciaAlunoSgpQuery request, CancellationToken cancellationToken)
            => await repositorioFrequenciaAlunoSgp.ObterFrequenciaAlunoPorRAEData(request.AlunoRa, request.Data);
    }
}
