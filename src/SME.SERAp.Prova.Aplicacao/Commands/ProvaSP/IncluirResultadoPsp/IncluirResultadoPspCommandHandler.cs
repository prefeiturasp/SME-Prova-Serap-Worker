using MediatR;
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

        public IncluirResultadoPspCommandHandler(IRepositorioResultadoSme repositorioResultadoSme)
        {
            this.repositorioResultadoSme = repositorioResultadoSme ?? throw new System.ArgumentNullException(nameof(repositorioResultadoSme));
        }

        public async Task<bool> Handle(IncluirResultadoPspCommand request, CancellationToken cancellationToken)
        {
            switch (request.Resultado.TipoResultado)
            {                
                case TipoResultadoPsp.ResultadoSme:
                    return await IncluirResultadoSME(request.Resultado);
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
    }
}
