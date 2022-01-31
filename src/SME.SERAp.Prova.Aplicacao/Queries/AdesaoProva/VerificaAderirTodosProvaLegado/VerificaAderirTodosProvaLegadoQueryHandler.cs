using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class VerificaAderirTodosProvaLegadoQueryHandler : IRequestHandler<VerificaAderirTodosProvaLegadoQuery, bool>
    {

        private readonly IRepositorioProvaAdesaoLegado repositorioProvaAdesaoLegado;

        public VerificaAderirTodosProvaLegadoQueryHandler(IRepositorioProvaAdesaoLegado repositorioProvaAdesaoLegado)
        {
            this.repositorioProvaAdesaoLegado = repositorioProvaAdesaoLegado ?? throw new ArgumentNullException(nameof(repositorioProvaAdesaoLegado));
        }

        public async Task<bool> Handle(VerificaAderirTodosProvaLegadoQuery request, CancellationToken cancellationToken)
        {
            var aderirTodos = await repositorioProvaAdesaoLegado.ObterAderirPorProvaId(request.ProvaLegadoId);
            return aderirTodos > 0;
        }    
    }
}
