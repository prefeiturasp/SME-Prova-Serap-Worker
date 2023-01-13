using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SME.SERAp.Prova.Aplicaca;
using SME.SERAp.Prova.Dados.Interfaces;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AtualizarStatusArquivoResultadoPspCommandHandler : IRequestHandler<AtualizarStatusArquivoResultadoPspCommand, bool>
    {
        private readonly IRepositorioArquivoResultadoPsp repositorioArquivoResultadoPsp;

        public AtualizarStatusArquivoResultadoPspCommandHandler(IRepositorioArquivoResultadoPsp repositorioArquivoResultadoPsp)
        {
            this.repositorioArquivoResultadoPsp = repositorioArquivoResultadoPsp ?? throw new System.ArgumentNullException(nameof(repositorioArquivoResultadoPsp));
        }
        public async Task<bool> Handle(AtualizarStatusArquivoResultadoPspCommand request, CancellationToken cancellationToken)
        {
            await repositorioArquivoResultadoPsp.AtualizarStatusArquivoResultadoPspPorId(request.Id, request.Status);
            return true;
        }
    }
}