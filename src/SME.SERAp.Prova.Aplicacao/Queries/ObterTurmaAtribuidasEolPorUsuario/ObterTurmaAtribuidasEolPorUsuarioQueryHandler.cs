using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmaAtribuidasEolPorUsuarioQueryHandler : IRequestHandler<ObterTurmaAtribuidasEolPorUsuarioQuery, IEnumerable<TurmaAtribuicaoEolDto>>
    {
        private readonly IRepositorioParametroSistema repositorioParametroSistema;
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioGeralEol repositorioGeralEol;

        public ObterTurmaAtribuidasEolPorUsuarioQueryHandler(IRepositorioGeralEol repositorioGeralEol,
            IRepositorioParametroSistema repositorioParametroSistema,
            IRepositorioCache repositorioCache)
        {
            this.repositorioGeralEol = repositorioGeralEol ?? throw new ArgumentNullException(nameof(repositorioGeralEol));
            this.repositorioParametroSistema = repositorioParametroSistema ?? throw new ArgumentNullException(nameof(repositorioParametroSistema));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<IEnumerable<TurmaAtribuicaoEolDto>> Handle(ObterTurmaAtribuidasEolPorUsuarioQuery request, CancellationToken cancellationToken)
        {
            var parametrosDoSistema = await repositorioCache.ObterRedisAsync("parametros", async () => await repositorioParametroSistema.ObterTudoAsync(), 1440);

            if(parametrosDoSistema == null)
                throw new Exception("Obter turma atribuição EOL - Não foi possível obter os parâmetros de sistema.");

            int anoInicial = parametrosDoSistema.Min(t => t.Ano).GetValueOrDefault();

            int[] tiposEscola = parametrosDoSistema
                .Where(a => a.Ano == DateTime.Now.Year && TipoParametroSistema.TipoEscolaSerap == a.Tipo)
                .FirstOrDefault().Valor.Split(",")
                .Select(c => int.Parse(c))
                .ToArray();

           return await repositorioGeralEol.ObterTurmaAtribuicaoEol(anoInicial, request.Login, tiposEscola, request.TurmaCodigo, request.AnoLetivo);
        }
    }
}
