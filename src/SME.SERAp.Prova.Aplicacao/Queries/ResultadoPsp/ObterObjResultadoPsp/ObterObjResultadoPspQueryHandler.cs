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

        public ObterObjResultadoPspQueryHandler(IRepositorioResultadoSme repositorioResultadoSme,
                                                IRepositorioResultadoDre repositorioResultadoDre)
        {
            this.repositorioResultadoSme = repositorioResultadoSme ?? throw new System.ArgumentNullException(nameof(repositorioResultadoSme));
            this.repositorioResultadoDre = repositorioResultadoDre ?? throw new System.ArgumentNullException(nameof(repositorioResultadoDre));
        }

        public async Task<ObjResultadoPspDto> Handle(ObterObjResultadoPspQuery request, CancellationToken cancellationToken)
        {
            switch (request.Resultado.TipoResultado)
            {
                case TipoResultadoPsp.ResultadoSme:
                    return await ObterResultadoSME(request.Resultado);
                case TipoResultadoPsp.ResultadoDre:
                    return await ObterResultadoDre(request.Resultado);
                default:
                    return null;
            }
        }

        private async Task<ObjResultadoPspDto> ObterResultadoSME(ObjResultadoPspDto resultado)
        {
            var resultadoBusca = (ResultadoSmeDto)resultado.Resultado;
            var result = await repositorioResultadoSme.ObterResultadoSme(resultadoBusca.Edicao, resultadoBusca.AreaConhecimentoID, resultadoBusca.AnoEscolar);
            if (result == null) return null;
            return new ObjResultadoPspDto(TipoResultadoPsp.ResultadoSme, result);
        }

        private async Task<ObjResultadoPspDto> ObterResultadoDre(ObjResultadoPspDto resultado)
        {
            var resultadoBusca = (ResultadoDreDto)resultado.Resultado;
            var result = await repositorioResultadoDre.ObterResultadoDre(resultadoBusca.Edicao, resultadoBusca.AreaConhecimentoID, resultadoBusca.UadSigla, resultadoBusca.AnoEscolar);
            if (result == null) return null;
            return new ObjResultadoPspDto(TipoResultadoPsp.ResultadoDre, result);
        }
    }
}
