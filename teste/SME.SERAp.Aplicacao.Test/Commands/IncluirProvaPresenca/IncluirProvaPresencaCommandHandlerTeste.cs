using Moq;
using Npgsql;
using SME.SERAp.Prova.Aplicacao.Commands.IncluirProvaPresenca;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio.Entidades.Presenca;
using SME.SERAp.Prova.Infra.Dtos.Presenca;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using SME.SERAp.Prova.Dominio.Enums;
using System.Linq;

namespace SME.SERAp.Aplicacao.Test.Commands.IncluirProvaPresenca
{
    public class IncluirProvaPresencaCommandHandlerTeste
    {
        private readonly Mock<IRepositorioProvaPresenca> _repositorioProvaPresencaMock;
        private readonly Mock<IServicoLog> _servicoLogMock;
        private readonly IncluirProvaPresencaCommandHandler _handler;

        public IncluirProvaPresencaCommandHandlerTeste()
        {
            _repositorioProvaPresencaMock = new Mock<IRepositorioProvaPresenca>();
            _servicoLogMock = new Mock<IServicoLog>();
            _handler = new IncluirProvaPresencaCommandHandler(_repositorioProvaPresencaMock.Object, _servicoLogMock.Object);
        }

        [Fact]
        public void Construtor_Deve_Lancar_ArgumentNullException_Quando_RepositorioProvaPresenca_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() => new IncluirProvaPresencaCommandHandler(null, _servicoLogMock.Object));
        }

        [Fact]
        public void Construtor_Deve_Lancar_ArgumentNullException_Quando_ServicoLog_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() => new IncluirProvaPresencaCommandHandler(_repositorioProvaPresencaMock.Object, null));
        }

        [Fact]
        public async Task Handle_Deve_Incluir_ProvaPresenca_Com_Sucesso()
        {
            var dto = new CriarProvaPresencaDto
            {
                NomeProva = "Prova Teste",
                AnoProva = 2024,
                DataInicialAplicacao = DateTime.Now,
                DataFinalAplicacao = DateTime.Now.AddDays(1),
                TurmasIds = new long[] { 101, 102 }
            };
            var command = new IncluirProvaPresencaCommand(dto);
            var provaPresencaIdEsperado = 1L;

            _repositorioProvaPresencaMock
                .Setup(r => r.IncluirComTurmasAsync(It.IsAny<ProvaPresenca>(), It.IsAny<long[]>()))
                .ReturnsAsync(provaPresencaIdEsperado);

            var resultado = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(provaPresencaIdEsperado, resultado);
            _repositorioProvaPresencaMock.Verify(r => r.IncluirComTurmasAsync(
                It.Is<ProvaPresenca>(p => p.NomeProva == dto.NomeProva && p.AnoProva == dto.AnoProva),
                It.Is<long[]>(t => t.SequenceEqual(dto.TurmasIds))), Times.Once);
            _servicoLogMock.Verify(s => s.Registrar(It.IsAny<LogNivel>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Lancar_NegocioException_Quando_TurmasIds_Nulo()
        {
            var dto = new CriarProvaPresencaDto
            {
                NomeProva = "Prova Teste",
                AnoProva = 2024,
                DataInicialAplicacao = DateTime.Now,
                DataFinalAplicacao = DateTime.Now.AddDays(1),
                TurmasIds = null
            };
            var command = new IncluirProvaPresencaCommand(dto);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _handler.Handle(command, CancellationToken.None));

            Assert.Equal($"Não é possível criar a Prova de Presença '{dto.NomeProva}' sem turmas vinculadas.", exception.Message);

            _servicoLogMock.Verify(s => s.Registrar(LogNivel.Negocio, $"Prova de Presença '{dto.NomeProva}' recebida sem turmas vinculadas."), Times.Once);
            _repositorioProvaPresencaMock.Verify(r => r.IncluirComTurmasAsync(It.IsAny<ProvaPresenca>(), It.IsAny<long[]>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Lancar_NegocioException_Quando_TurmasIds_Vazio()
        {
            var dto = new CriarProvaPresencaDto
            {
                NomeProva = "Prova Teste",
                AnoProva = 2024,
                DataInicialAplicacao = DateTime.Now,
                DataFinalAplicacao = DateTime.Now.AddDays(1),
                TurmasIds = new long[] { }
            };
            var command = new IncluirProvaPresencaCommand(dto);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _handler.Handle(command, CancellationToken.None));

            Assert.Equal($"Não é possível criar a Prova de Presença '{dto.NomeProva}' sem turmas vinculadas.", exception.Message);

            _servicoLogMock.Verify(s => s.Registrar(LogNivel.Negocio, $"Prova de Presença '{dto.NomeProva}' recebida sem turmas vinculadas."), Times.Once);
            _repositorioProvaPresencaMock.Verify(r => r.IncluirComTurmasAsync(It.IsAny<ProvaPresenca>(), It.IsAny<long[]>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Lancar_NegocioException_Quando_ProvaPresencaId_Invalido()
        {
            var dto = new CriarProvaPresencaDto
            {
                NomeProva = "Prova Teste",
                AnoProva = 2024,
                DataInicialAplicacao = DateTime.Now,
                DataFinalAplicacao = DateTime.Now.AddDays(1),
                TurmasIds = new long[] { 101 }
            };
            var command = new IncluirProvaPresencaCommand(dto);

            _repositorioProvaPresencaMock
                .Setup(r => r.IncluirComTurmasAsync(It.IsAny<ProvaPresenca>(), It.IsAny<long[]>()))
                .ReturnsAsync(0L);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _handler.Handle(command, CancellationToken.None));

            Assert.Equal("Não foi possível salvar a Prova de Presença.", exception.Message);
            _repositorioProvaPresencaMock.Verify(r => r.IncluirComTurmasAsync(It.IsAny<ProvaPresenca>(), It.IsAny<long[]>()), Times.Once);
            _servicoLogMock.Verify(s => s.Registrar(
                LogNivel.Critico,
                It.Is<string>(msg => msg.Contains("Erro ao incluir Prova de Presença 'Prova Teste' no Worker: Não foi possível salvar a Prova de Presença.")),
                It.IsAny<string>(),
                It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_ReLancar_Exception_E_Logar_Critico_Quando_ErroDeRepositorioGenerico()
        {
            var dto = new CriarProvaPresencaDto
            {
                NomeProva = "Prova Com Erro",
                AnoProva = 2024,
                DataInicialAplicacao = DateTime.Now,
                DataFinalAplicacao = DateTime.Now.AddDays(1),
                TurmasIds = new long[] { 101 }
            };
            var command = new IncluirProvaPresencaCommand(dto);
            var excecaoInesperada = new Exception("Erro inesperado no banco de dados.");

            _repositorioProvaPresencaMock
                .Setup(r => r.IncluirComTurmasAsync(It.IsAny<ProvaPresenca>(), It.IsAny<long[]>()))
                .ThrowsAsync(excecaoInesperada);

            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));

            Assert.Equal(excecaoInesperada.Message, exception.Message);
            _repositorioProvaPresencaMock.Verify(r => r.IncluirComTurmasAsync(It.IsAny<ProvaPresenca>(), It.IsAny<long[]>()), Times.Once);
            _servicoLogMock.Verify(s => s.Registrar(
                LogNivel.Critico,
                It.Is<string>(msg => msg.Contains($"Erro ao incluir Prova de Presença '{dto.NomeProva}' no Worker:")),
                It.IsAny<string>(),
                It.IsAny<string>()), Times.Once);
        }
    }
}