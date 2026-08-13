using MediatR;
using SME.SERAp.Prova.Aplicacao.Commands.AtualizarProvaPresenca;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos.Presenca;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.UseCase.Presenca
{
    public class AtualizarProvaPresencaUseCase : AbstractUseCase, IAtualizarProvaPresencaUseCase
    {
        private readonly IServicoLog servicoLog;

        public AtualizarProvaPresencaUseCase(
            IMediator mediator,
            IServicoLog servicoLog) : base(mediator)
        {
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            if (mensagemRabbit == null)
            {
                servicoLog.Registrar(Dominio.Enums.LogNivel.Critico, "Mensagem Rabbit para AtualizarProvaPresencaUseCase (Worker) veio nula ou em formato inválido.", string.Empty, string.Empty);
                throw new NegocioException("Mensagem Rabbit para AtualizarProvaPresencaUseCase (Worker) veio nula ou em formato inválido.");
            }

            var dto = mensagemRabbit.ObterObjetoMensagem<AtualizarProvaPresencaDto>();

            if (dto == null)
            {
                servicoLog.Registrar(Dominio.Enums.LogNivel.Critico, "Mensagem Rabbit para AtualizarProvaPresencaUseCase (Worker) veio nula ou em formato inválido.", string.Empty, string.Empty);
                throw new NegocioException("Mensagem Rabbit para AtualizarProvaPresencaUseCase (Worker) veio nula ou em formato inválido.");
            }

            try
            {
                var atualizado = await mediator.Send(new AtualizarProvaPresencaCommand(dto));

                if (atualizado)
                {
                    servicoLog.Registrar(Dominio.Enums.LogNivel.Informacao, $"Prova de Presença '{dto.NomeProva}' (ID: {dto.Id}) atualizada com sucesso e associada a {dto.TurmasIds?.Length ?? 0} turmas.", string.Empty, string.Empty);
                }
                else
                {
                    servicoLog.Registrar(Dominio.Enums.LogNivel.Informacao, $"Prova de Presença '{dto.NomeProva}' (ID: {dto.Id}) não foi atualizada.", string.Empty, string.Empty);
                }
                return atualizado;
            }
            catch (NegocioException)
            {
                throw;
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(Dominio.Enums.LogNivel.Critico, $"Erro geral no Worker ao processar atualização da Prova de Presença '{dto.NomeProva}' (ID: {dto.Id}): {ex.Message}", string.Empty, ex.StackTrace);
                throw;
            }
        }
    }
}