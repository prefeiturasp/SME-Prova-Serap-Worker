using MediatR;
using SME.SERAp.Prova.Aplicacao.Commands.DeletarProvaPresenca;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos.Presenca;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.UseCase.Presenca
{
    public class DeletarProvaPresencaUseCase : AbstractUseCase, IDeletarProvaPresencaUseCase
    {
        private readonly IServicoLog servicoLog;

        public DeletarProvaPresencaUseCase(
            IMediator mediator,
            IServicoLog servicoLog) : base(mediator)
        {
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            if (mensagemRabbit == null)
            {
                servicoLog.Registrar(Dominio.Enums.LogNivel.Critico, "Mensagem Rabbit para DeletarProvaPresencaUseCase (Worker) veio nula ou em formato inválido.", string.Empty, string.Empty);
                throw new NegocioException("Mensagem Rabbit para DeletarProvaPresencaUseCase (Worker) veio nula ou em formato inválido.");
            }

            var dto = mensagemRabbit.ObterObjetoMensagem<DeletarProvaPresencaDto>();

            if (dto == null || dto.Id <= 0)
            {
                servicoLog.Registrar(Dominio.Enums.LogNivel.Critico, "ID da Prova de Presença para exclusão é inválido (menor ou igual a zero) ou DTO nulo.", string.Empty, string.Empty);
                throw new NegocioException("ID da Prova de Presença para exclusão é inválido ou DTO nulo.");
            }

            try
            {
                var deletado = await mediator.Send(new DeletarProvaPresencaCommand(dto.Id));

                if (deletado)
                {
                    servicoLog.Registrar(Dominio.Enums.LogNivel.Informacao, $"Prova de Presença com ID {dto.Id} deletada com sucesso.", string.Empty, string.Empty);
                }
                else
                {
                    servicoLog.Registrar(Dominio.Enums.LogNivel.Informacao, $"Prova de Presença com ID {dto.Id} não foi deletada.", string.Empty, string.Empty);
                }
                return deletado;
            }
            catch (NegocioException)
            {
                throw;
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(Dominio.Enums.LogNivel.Critico, $"Erro geral no Worker ao processar exclusão da Prova de Presença com ID {dto.Id}: {ex.Message}", string.Empty, ex.StackTrace);
                throw;
            }
        }
    }
}