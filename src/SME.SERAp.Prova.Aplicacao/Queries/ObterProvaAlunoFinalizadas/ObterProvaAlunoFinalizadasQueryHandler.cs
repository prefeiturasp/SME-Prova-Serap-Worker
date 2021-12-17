using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvaAlunoFinalizadasQueryHandler : IRequestHandler<ObterProvaAlunoFinalizadasQuery, IEnumerable<ProvaAlunoReduzidaDto>>
    {
        private readonly IRepositorioProvaAluno repositorioProvaAluno;

        public ObterProvaAlunoFinalizadasQueryHandler(IRepositorioProvaAluno repositorioProvaAluno)
        {
            this.repositorioProvaAluno = repositorioProvaAluno ??
                                          throw new ArgumentNullException(nameof(repositorioProvaAluno));
        }

        public async Task<IEnumerable<ProvaAlunoReduzidaDto>> Handle(ObterProvaAlunoFinalizadasQuery request,
            CancellationToken cancellationToken)
            => await repositorioProvaAluno.ObterAlunosProvasFinalizadasReduzido();
    }
}