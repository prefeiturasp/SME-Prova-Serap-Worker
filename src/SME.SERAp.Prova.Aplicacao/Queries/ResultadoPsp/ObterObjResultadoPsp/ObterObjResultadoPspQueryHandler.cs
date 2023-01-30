using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterObjResultadoPspQueryHandler : IRequestHandler<ObterObjResultadoPspQuery, ObjResultadoPspDto>
    {
        private readonly IRepositorioResultadoSme repositorioResultadoSme;
        private readonly IRepositorioResultadoDre repositorioResultadoDre;
        private readonly IRepositorioResultadoEscola repositorioResultadoEscola;

        private ObjResultadoPspDto ObjResultado;

        public ObterObjResultadoPspQueryHandler(IRepositorioResultadoSme repositorioResultadoSme,
                                                IRepositorioResultadoDre repositorioResultadoDre,
                                                IRepositorioResultadoEscola repositorioResultadoEscola)
        {
            this.repositorioResultadoSme = repositorioResultadoSme ?? throw new System.ArgumentNullException(nameof(repositorioResultadoSme));
            this.repositorioResultadoDre = repositorioResultadoDre ?? throw new System.ArgumentNullException(nameof(repositorioResultadoDre));
            this.repositorioResultadoEscola = repositorioResultadoEscola;
        }

        public async Task<ObjResultadoPspDto> Handle(ObterObjResultadoPspQuery request, CancellationToken cancellationToken)
        {
            ObjResultado = request.Resultado;
            switch (request.Resultado.TipoResultado)
            {
                case TipoResultadoPsp.ResultadoSme:
                    return await ObterResultadoSME();
                case TipoResultadoPsp.ResultadoDre:
                    return await ObterResultadoDre();
                case TipoResultadoPsp.ResultadoEscola:
                    return await ObterResultadoEscola();
                default:
                    return null;
            }
        }

        private async Task<ObjResultadoPspDto> ObterResultadoSME()
        {
            var resultadoBusca = (ResultadoSmeDto)ObjResultado.Resultado;
            var result = await repositorioResultadoSme.ObterResultadoSme(resultadoBusca.Edicao, resultadoBusca.AreaConhecimentoID, resultadoBusca.AnoEscolar);
            if (result == null) return null;
            return new ObjResultadoPspDto(ObjResultado.TipoResultado, result);
        }

        private async Task<ObjResultadoPspDto> ObterResultadoDre()
        {
            var resultadoBusca = (ResultadoDreDto)ObjResultado.Resultado;
            var result = await repositorioResultadoDre.ObterResultadoDre(resultadoBusca.Edicao, resultadoBusca.AreaConhecimentoID, resultadoBusca.UadSigla, resultadoBusca.AnoEscolar);
            if (result == null) return null;
            return new ObjResultadoPspDto(ObjResultado.TipoResultado, result);
        }

        private async Task<ObjResultadoPspDto> ObterResultadoEscola()
        {
            var resultadoBusca = (ResultadoEscolaDto)ObjResultado.Resultado;
            var result = await repositorioResultadoEscola.ObterResultadoEscola(resultadoBusca.Edicao, resultadoBusca.AreaConhecimentoID, resultadoBusca.EscCodigo, resultadoBusca.AnoEscolar);
            if (result == null) return null;
            return new ObjResultadoPspDto(ObjResultado.TipoResultado, result);
        }
    }
}
