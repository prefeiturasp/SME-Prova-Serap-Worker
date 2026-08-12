using MediatR;
using SME.SERAp.Prova.Aplicacao.Commands.IncluirProvaPresenca;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos.Presenca;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.UseCase.Presenca
{
    public class CriarProvaPresencaUseCase : AbstractUseCase, ICriarProvaPresencaUseCase
    {
        private readonly IServicoLog servicoLog;

        public CriarProvaPresencaUseCase(
            IMediator mediator,
            IServicoLog servicoLog) : base(mediator)
        {
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            if (mensagemRabbit == null)
            {
                servicoLog.Registrar(Dominio.Enums.LogNivel.Critico, "Mensagem Rabbit para CriarProvaPresencaUseCase (Worker) veio nula ou em formato inválido.");
                throw new NegocioException("Mensagem Rabbit para CriarProvaPresencaUseCase (Worker) veio nula ou em formato inválido.");
            }

            var dto = mensagemRabbit.ObterObjetoMensagem<CriarProvaPresencaDto>();

            if (dto == null)
            {
                servicoLog.Registrar(Dominio.Enums.LogNivel.Critico, "Mensagem Rabbit para CriarProvaPresencaUseCase (Worker) veio nula ou em formato inválido.");
                throw new NegocioException("Mensagem Rabbit para CriarProvaPresencaUseCase (Worker) veio nula ou em formato inválido.");
            }

            try
            {
                var provaPresencaId = await mediator.Send(new IncluirProvaPresencaCommand(dto));

                servicoLog.Registrar(Dominio.Enums.LogNivel.Informacao, $"Prova de Presença '{dto.NomeProva}' (ID: {provaPresencaId}) criada com sucesso e associada a {dto.TurmasIds.Length} turmas.");
                return true;
            }
            catch (NegocioException)
            {
                throw;
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(Dominio.Enums.LogNivel.Critico, $"Erro geral no Worker ao processar Prova de Presença '{dto.NomeProva}': {ex.Message}", string.Empty, ex.StackTrace);
                throw;
            }
        }
    }
}