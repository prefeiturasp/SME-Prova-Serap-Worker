using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class IncluirResultadoPspCommandHandler : IRequestHandler<IncluirResultadoPspCommand, bool>
    {
        private readonly IRepositorioResultadoSme repositorioResultadoSme;
        private readonly IRepositorioResultadoDre repositorioResultadoDre;

        public IncluirResultadoPspCommandHandler(IRepositorioResultadoSme repositorioResultadoSme,
                                                 IRepositorioResultadoDre repositorioResultadoDre)
        {
            this.repositorioResultadoSme = repositorioResultadoSme ?? throw new System.ArgumentNullException(nameof(repositorioResultadoSme));
            this.repositorioResultadoDre = repositorioResultadoDre ?? throw new System.ArgumentNullException(nameof(repositorioResultadoDre));
        }

        public async Task<bool> Handle(IncluirResultadoPspCommand request, CancellationToken cancellationToken)
        {
            switch (request.Resultado.TipoResultado)
            {                
                case TipoResultadoPsp.ResultadoSme:
                    return await IncluirResultadoSME(request.Resultado);
                case TipoResultadoPsp.ResultadoDre:
                    return await IncluirResultadoDre(request.Resultado);
                default:
                    return false;
            }
        }

        private async Task<bool> IncluirResultadoSME(ObjResultadoPspDto resultado)
        {
            var resultadoInserir = (ResultadoSme)resultado.Resultado;
            var result = await repositorioResultadoSme.IncluirAsync(resultadoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirResultadoDre(ObjResultadoPspDto resultado)
        {
            var resultadoInserir = (ResultadoDre)resultado.Resultado;
            var result = await repositorioResultadoDre.IncluirAsync(resultadoInserir);
            return result > 0;
        }
    }
}
