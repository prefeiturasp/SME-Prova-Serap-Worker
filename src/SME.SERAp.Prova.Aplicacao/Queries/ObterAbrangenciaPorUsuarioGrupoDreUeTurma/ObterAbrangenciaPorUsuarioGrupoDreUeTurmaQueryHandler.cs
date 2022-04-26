using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAbrangenciaPorUsuarioGrupoDreUeTurmaQueryHandler : IRequestHandler<ObterAbrangenciaPorUsuarioGrupoDreUeTurmaQuery, Abrangencia>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterAbrangenciaPorUsuarioGrupoDreUeTurmaQueryHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task<Abrangencia> Handle(ObterAbrangenciaPorUsuarioGrupoDreUeTurmaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAbrangencia.ObterPorUsuarioGrupoDreUeTurmaAsync(request.UsuarioId, request.GrupoId, request.DreId, request.UeId, request.TurmaId);
        }
    }
}
