using MediatR;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Commands.DeletarProvaPresenca
{
    public class DeletarProvaPresencaCommandHandler : IRequestHandler<DeletarProvaPresencaCommand, bool>
    {
        private readonly IRepositorioProvaPresenca repositorioProvaPresenca;
        private readonly IServicoLog servicoLog;

        public DeletarProvaPresencaCommandHandler(IRepositorioProvaPresenca repositorioProvaPresenca, IServicoLog servicoLog)
        {
            this.repositorioProvaPresenca = repositorioProvaPresenca ?? throw new ArgumentNullException(nameof(repositorioProvaPresenca));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Handle(DeletarProvaPresencaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var provaExistente = await repositorioProvaPresenca.ObterPorIdAsync(request.Id);
                if (provaExistente == null)
                {
                    servicoLog.Registrar(Dominio.Enums.LogNivel.Negocio, $"Tentativa de deletar Prova de Presença com ID {request.Id} que não existe.");
                    throw new NegocioException($"Prova de Presença com ID {request.Id} não encontrada para exclusão.");
                }

                var deletado = await repositorioProvaPresenca.DeletarAsync(request.Id);

                if (!deletado)
                {
                    throw new NegocioException($"Não foi possível deletar a Prova de Presença com ID {request.Id}.");
                }

                return deletado;
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(Dominio.Enums.LogNivel.Critico, $"Erro ao deletar Prova de Presença com ID {request.Id} no Worker: {ex.Message}", string.Empty, ex.StackTrace);
                throw;
            }
        }
    }
}