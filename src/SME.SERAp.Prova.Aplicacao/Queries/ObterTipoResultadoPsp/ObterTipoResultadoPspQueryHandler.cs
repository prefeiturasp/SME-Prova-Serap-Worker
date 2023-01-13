using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SME.SERAp.Prova.Dominio.Entidades;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Dados.Interfaces;

namespace SME.SERAp.Prova.Aplicacao
{
    internal class ObterTipoResultadoPspQueryHandler : IRequestHandler<ObterTipoResultadoPspQuery, ArquivoResultadoPspDto>
    {

        private readonly IRepositorioArquivoResultadoPsp repositorioArquivoResultadoPsp;

        public ObterTipoResultadoPspQueryHandler(IRepositorioArquivoResultadoPsp repositorioArquivoResultadoPsp)
        {
            this.repositorioArquivoResultadoPsp = repositorioArquivoResultadoPsp ?? throw new System.ArgumentNullException(nameof(repositorioArquivoResultadoPsp));
        }

        public async Task<ArquivoResultadoPspDto> Handle(ObterTipoResultadoPspQuery request, CancellationToken cancellationToken)
        {
            return await repositorioArquivoResultadoPsp.ObterArquivoResultadoPspPorId(request.Id);
        }
    }
}