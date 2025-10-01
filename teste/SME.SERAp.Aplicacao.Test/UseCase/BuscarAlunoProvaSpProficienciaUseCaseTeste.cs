using MediatR;
using Moq;
using RabbitMQ.Client;
using SME.SERAp.Prova.Aplicacao;
using SME.SERAp.Prova.Aplicacao.Queries.ObterAnoProva;
using SME.SERAp.Prova.Aplicacao.Queries.ObterResultadoAlunoProvaSp;
using SME.SERAp.Prova.Aplicacao.UseCase;
using SME.SERAp.Prova.Dominio.Entidades;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Aplicacao.Test.UseCase
{
    public class BuscarAlunoProvaSpProficienciaUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IModel> model;
        private readonly Mock<IServicoLog> servicoLog;
        private readonly BuscarAlunoProvaSpProficienciaUseCase useCase;

        public BuscarAlunoProvaSpProficienciaUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            model = new Mock<IModel>();
            servicoLog = new Mock<IServicoLog>();
            useCase = new BuscarAlunoProvaSpProficienciaUseCase(mediator.Object, servicoLog.Object);
        }

        [Fact]
        public async Task Deve_Retornar_False_Se_MensagemRabbit_For_Nula()
        {
            var mensagemRabbit = new MensagemRabbit(string.Empty, Guid.NewGuid());
            var result = await useCase.Executar(mensagemRabbit);

            Assert.False(result);
            servicoLog.Verify(s => s.Registrar(It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Publicar_Fila_Quando_Tudo_Correto()
        {
            var boletim = new BoletimProvaAluno
            {
                AlunoRa = 123,
                DisciplinaId = 4,
                ProvaId = 2025
            };

            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(boletim), Guid.NewGuid());
            mediator.Setup(m => m.Send(It.IsAny<ObterAnoProvaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(2025);
            mediator.Setup(m => m.Send(It.IsAny<ObterResultadoAlunoProvaSpQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ResultadoAlunoProvaSpDto { });

            mediator.Setup(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var result = await useCase.Executar(mensagemRabbit);

            Assert.True(result);
            mediator.Verify(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_True_Se_Resultado_Do_Aluno_For_Nulo()
        {
            var boletim = new BoletimProvaAluno
            {
                AlunoRa = 123,
                DisciplinaId = 4,
                ProvaId = 2025
            };

            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(boletim), Guid.NewGuid());
            mediator.Setup(m => m.Send(It.IsAny<ObterAnoProvaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(2025);
            mediator.Setup(m => m.Send(It.IsAny<ObterResultadoAlunoProvaSpQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ResultadoAlunoProvaSpDto)null!);

            var result = await useCase.Executar(mensagemRabbit);

            Assert.True(result);
            mediator.Verify(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Se_Ano_Prova_For_Nulo()
        {
            var provaId = 2025;
            mediator.Setup(m => m.Send(It.IsAny<ObterAnoProvaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((int?)null);

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await useCase.TestObterEdicaoProvaSp(provaId);
            });
        }

        [Theory]
        [InlineData(4, 3)]
        [InlineData(5, 2)]
        [InlineData(2, 1)]
        [InlineData(6, 1)]
        [InlineData(7, 1)]
        [InlineData(999, 0)]
        public void Deve_Retornar_Valor_Correto_Obter_Area_Do_Conhecimento(long disciplinaId, int expected)
        {
            var result = useCase.TestObterAreaDoConhecimento(disciplinaId);

            Assert.Equal(expected, result);
        }
    }

    public static class BuscarAlunoProvaSpProficienciaUseCaseExtensions
    {
        public static async Task<int> TestObterEdicaoProvaSp(this BuscarAlunoProvaSpProficienciaUseCase useCase, long provaId)
        {
            var method = typeof(BuscarAlunoProvaSpProficienciaUseCase)
                .GetMethod("ObterEdicaoProvaSp", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            return await (Task<int>)method!.Invoke(useCase, new object[] { provaId })!;
        }

        public static int TestObterAreaDoConhecimento(this BuscarAlunoProvaSpProficienciaUseCase useCase, long disciplinaId)
        {
            var method = typeof(BuscarAlunoProvaSpProficienciaUseCase)
                .GetMethod("ObterAreaDoConhecimento", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            return (int)method.Invoke(useCase, new object[] { disciplinaId })!;
        }
    }
}
