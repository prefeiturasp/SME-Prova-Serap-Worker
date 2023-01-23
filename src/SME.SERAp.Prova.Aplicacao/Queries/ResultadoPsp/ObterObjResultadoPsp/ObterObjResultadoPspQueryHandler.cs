using MediatR;
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

        public ObterObjResultadoPspQueryHandler(IRepositorioResultadoSme repositorioResultadoSme)
        {
            this.repositorioResultadoSme = repositorioResultadoSme ?? throw new System.ArgumentNullException(nameof(repositorioResultadoSme));
        }

        public async Task<ObjResultadoPspDto> Handle(ObterObjResultadoPspQuery request, CancellationToken cancellationToken)
        {
            switch (request.Resultado.TipoResultado)
            {
                case TipoResultadoPsp.ResultadoSme:
                    return await ObterResultadoSME(request.Resultado);
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
    }
}
