using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAtribuicoesEolQueryHandler : IRequestHandler<ObterAtribuicoesEolQuery, IEnumerable<TurmaAtribuicaoEolDto>>
    {
        private readonly IRepositorioGeralEol repositorioGeralEol;
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioParametroSistema repositorioParametroSistema;

        public ObterAtribuicoesEolQueryHandler(IRepositorioGeralEol repositorioGeralEol, IRepositorioCache repositorioCache, IRepositorioParametroSistema repositorioParametroSistema)
        {
            this.repositorioGeralEol = repositorioGeralEol ?? throw new ArgumentNullException(nameof(repositorioGeralEol));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.repositorioParametroSistema = repositorioParametroSistema ?? throw new ArgumentNullException(nameof(repositorioParametroSistema));
        }

        public async Task<IEnumerable<TurmaAtribuicaoEolDto>> Handle(ObterAtribuicoesEolQuery request, CancellationToken cancellationToken)
        {
            var parametrosDoSistema = await repositorioCache
                .ObterRedisAsync(CacheChave.Parametros, async () => await repositorioParametroSistema.ObterTudoAsync(), 1440);
            
            if (parametrosDoSistema == null)
                throw new Exception("Obter atribuições EOL - Não foi possível obter os parâmetros de sistema.");

            var anoInicial = parametrosDoSistema.Min(t => t.Ano).GetValueOrDefault();

            var tiposEscola = parametrosDoSistema
                .FirstOrDefault(a => a.Ano == DateTime.Now.Year && TipoParametroSistema.TipoEscolaSerap == a.Tipo).Valor
                .Split(",")
                .Select(c => int.Parse(c))
                .ToArray();

            var nomeChave = string.Format(CacheChave.AtribuicoesEolUsuario, request.CodigoRf);
            var atribuicoes = await repositorioCache.ObterRedisAsync(nomeChave, async () => await repositorioGeralEol.ObterAtribuicoesEolAsync(request.CodigoRf, anoInicial, tiposEscola), 60);
            
            if (request.TurmaCodigo.HasValue && request.AnoLetivo.HasValue)
                atribuicoes = atribuicoes.Where(c =>c.TurmaCodigo == request.TurmaCodigo && c.AnoLetivo == request.AnoLetivo);

            return atribuicoes;
        }
    }
}