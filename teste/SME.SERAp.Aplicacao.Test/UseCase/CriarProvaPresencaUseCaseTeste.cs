using MediatR;
using Moq;
using SME.SERAp.Prova.Aplicacao.Commands.IncluirProvaPresenca;
using SME.SERAp.Prova.Aplicacao.UseCase.Presenca;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos.Presenca;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Aplicacao.Test.UseCase
{
    public class CriarProvaPresencaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IServicoLog> _servicoLogMock;
        private readonly CriarProvaPresencaUseCase _useCase;

        public CriarProvaPresencaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _servicoLogMock = new Mock<IServicoLog>();
            _useCase = new CriarProvaPresencaUseCase(_mediatorMock.Object, _servicoLogMock.Object);
        }

        [Fact]
        public void Construtor_Deve_Lancar_ArgumentNullException_Quando_ServicoLog_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() => new CriarProvaPresencaUseCase(Mock.Of<IMediator>(), null));
        }

        [Fact]
        public async Task Executar_Deve_Lancar_NegocioException_Quando_MensagemRabbit_Nula()
        {
            MensagemRabbit mensagemRabbit = null;

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(mensagemRabbit));

            Assert.Equal("Mensagem Rabbit para CriarProvaPresencaUseCase (Worker) veio nula ou em formato inválido.", exception.Message);
            _servicoLogMock.Verify(s => s.Registrar(LogNivel.Critico, "Mensagem Rabbit para CriarProvaPresencaUseCase (Worker) veio nula ou em formato inválido."), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<IncluirProvaPresencaCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_NegocioException_Quando_MensagemRabbit_Com_Dto_Nulo()
        {
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize<CriarProvaPresencaDto>(null), Guid.NewGuid());

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(mensagemRabbit));

            Assert.Equal("Mensagem Rabbit para CriarProvaPresencaUseCase (Worker) veio nula ou em formato inválido.", exception.Message);
            _servicoLogMock.Verify(s => s.Registrar(LogNivel.Critico, "Mensagem Rabbit para CriarProvaPresencaUseCase (Worker) veio nula ou em formato inválido."), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<IncluirProvaPresencaCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Criar_ProvaPresenca_Com_Sucesso()
        {
            var dto = new CriarProvaPresencaDto
            {
                NomeProva = "Prova Teste",
                AnoProva = 2024,
                DataInicialAplicacao = DateTime.Now,
                DataFinalAplicacao = DateTime.Now.AddDays(1),
                TurmasIds = new long[] { 101, 102 }
            };
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(dto), Guid.NewGuid());
            var provaPresencaId = 1L;

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<IncluirProvaPresencaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(provaPresencaId);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(
                It.Is<IncluirProvaPresencaCommand>(cmd => cmd.ProvaPresencaDto.NomeProva == dto.NomeProva),
                It.IsAny<CancellationToken>()), Times.Once);
            _servicoLogMock.Verify(s => s.Registrar(
                LogNivel.Informacao,
                $"Prova de Presença '{dto.NomeProva}' (ID: {provaPresencaId}) criada com sucesso e associada a {dto.TurmasIds.Length} turmas."), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_ReLancar_NegocioException_Do_Mediator()
        {
            var dto = new CriarProvaPresencaDto
            {
                NomeProva = "Prova Teste",
                AnoProva = 2024,
                DataInicialAplicacao = DateTime.Now,
                DataFinalAplicacao = DateTime.Now.AddDays(1),
                TurmasIds = new long[] { 101, 102 }
            };
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(dto), Guid.NewGuid());
            var negocioException = new NegocioException("Erro de negócio simulado.");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<IncluirProvaPresencaCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(negocioException);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(mensagemRabbit));

            Assert.Equal(negocioException.Message, exception.Message);
            _mediatorMock.Verify(m => m.Send(It.IsAny<IncluirProvaPresencaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _servicoLogMock.Verify(s => s.Registrar(It.IsAny<LogNivel>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Exception_Geral_E_Logar_Critico()
        {
            var dto = new CriarProvaPresencaDto
            {
                NomeProva = "Prova Teste",
                AnoProva = 2024,
                DataInicialAplicacao = DateTime.Now,
                DataFinalAplicacao = DateTime.Now.AddDays(1),
                TurmasIds = new long[] { 101, 102 }
            };
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(dto), Guid.NewGuid());
            var excecaoInesperada = new Exception("Erro inesperado no mediator.");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<IncluirProvaPresencaCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(excecaoInesperada);

            var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.Executar(mensagemRabbit));

            Assert.Equal(excecaoInesperada.Message, exception.Message);
            _mediatorMock.Verify(m => m.Send(It.IsAny<IncluirProvaPresencaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _servicoLogMock.Verify(s => s.Registrar(
                LogNivel.Critico,
                It.Is<string>(msg => msg.Contains($"Erro geral no Worker ao processar Prova de Presença '{dto.NomeProva}': {excecaoInesperada.Message}")),
                It.IsAny<string>(),
                It.IsAny<string>()), Times.Once);
        }
    }
}