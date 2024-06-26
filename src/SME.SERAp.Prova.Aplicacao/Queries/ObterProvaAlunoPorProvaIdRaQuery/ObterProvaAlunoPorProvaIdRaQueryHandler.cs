﻿using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao 
{ 
    public class ObterProvaAlunoPorProvaIdRaQueryHandler : IRequestHandler<ObterProvaAlunoPorProvaIdRaQuery, ProvaAluno>
    {
        private readonly IRepositorioProvaAluno repositorioProvaAluno;

        public ObterProvaAlunoPorProvaIdRaQueryHandler(IRepositorioProvaAluno repositorioProvaAluno)
        {
            this.repositorioProvaAluno = repositorioProvaAluno ??
                                          throw new ArgumentNullException(nameof(repositorioProvaAluno));
        }

        public async Task<ProvaAluno> Handle(ObterProvaAlunoPorProvaIdRaQuery request,
            CancellationToken cancellationToken)
            => await repositorioProvaAluno.ObterPorProvaIdRaAsync(request.ProvaId, request.AlunoRa);
    }
}
