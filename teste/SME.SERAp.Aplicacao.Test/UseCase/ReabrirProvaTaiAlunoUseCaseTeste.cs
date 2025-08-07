using MediatR;
using Moq;
using SME.SERAp.Prova.Aplicacao;
using SME.SERAp.Prova.Aplicacao.Commands;
using SME.SERAp.Prova.Aplicacao.Commands.AlunoProvaProficiencia.Excluir;
using SME.SERAp.Prova.Aplicacao.UseCase;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Aplicacao.Teste.UseCase
{
    public class ReabrirProvaTaiAlunoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IServicoLog> servicoLog;
        private readonly ReabrirProvaTaiAlunoUseCase useCase;

        public ReabrirProvaTaiAlunoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            servicoLog = new Mock<IServicoLog>();
            useCase = new ReabrirProvaTaiAlunoUseCase(mediator.Object, servicoLog.Object);
        }

        [Fact]
        public async Task Deve_Reabrir_Prova_Tai_Aluno()
        {
            var provaAluno = new ProvaAluno { AlunoRA = 12345, ProvaId = 1 };
            var prova = ObterProva(provaAluno.ProvaId);

            mediator.Setup(m => m.Send(It.IsAny<ObterProvaPorIdQuery>(), CancellationToken.None)).ReturnsAsync(prova);
            mediator.Setup(m => m.Send(It.IsAny<ObterProvaAlunoPorProvaIdRaQuery>(), CancellationToken.None)).ReturnsAsync(provaAluno);

            var resultado = await useCase.Executar(new MensagemRabbit(JsonSerializer.Serialize(provaAluno), Guid.NewGuid()));

            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<ObterProvaPorIdQuery>(), CancellationToken.None), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterProvaAlunoPorProvaIdRaQuery>(), CancellationToken.None), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ExcluirRespostaAlunoCommand>(), CancellationToken.None), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ExcluirQuestaoAlunoTaiCommand>(), CancellationToken.None), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ExcluirProvaAlunoCommand>(), CancellationToken.None), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ExcluirAlunoProvaProficienciaCommand>(), CancellationToken.None), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<LimparCacheProvaTaiCommand>(), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_False_Quando_ProvaAluno_For_Null()
        {
            var resultado = await useCase.Executar(new MensagemRabbit(string.Empty, Guid.NewGuid()));

            Assert.False(resultado);
            mediator.Verify(m => m.Send(It.IsAny<ObterProvaPorIdQuery>(), CancellationToken.None), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<ObterProvaAlunoPorProvaIdRaQuery>(), CancellationToken.None), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<ExcluirRespostaAlunoCommand>(), CancellationToken.None), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<ExcluirQuestaoAlunoTaiCommand>(), CancellationToken.None), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<ExcluirProvaAlunoCommand>(), CancellationToken.None), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<ExcluirAlunoProvaProficienciaCommand>(), CancellationToken.None), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<LimparCacheProvaTaiCommand>(), CancellationToken.None), Times.Never);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Prova_Nao_Encontrada()
        {
            var provaAluno = new ProvaAluno { ProvaId = 1, AlunoRA = 123 };
            mediator.Setup(m => m.Send(It.IsAny<ObterProvaPorIdQuery>(), default)).ReturnsAsync((Prova.Dominio.Prova)null);

            var resultado = await useCase.Executar(new MensagemRabbit(JsonSerializer.Serialize(provaAluno), Guid.NewGuid()));

            Assert.False(resultado);
            mediator.Verify(m => m.Send(It.IsAny<ObterProvaPorIdQuery>(), CancellationToken.None), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterProvaAlunoPorProvaIdRaQuery>(), CancellationToken.None), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<ExcluirRespostaAlunoCommand>(), CancellationToken.None), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<ExcluirQuestaoAlunoTaiCommand>(), CancellationToken.None), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<ExcluirProvaAlunoCommand>(), CancellationToken.None), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<ExcluirAlunoProvaProficienciaCommand>(), CancellationToken.None), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<LimparCacheProvaTaiCommand>(), CancellationToken.None), Times.Never);
            servicoLog.Verify(l => l.Registrar(It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Prova_Nao_Em_Formato_Tai()
        {
            var provaAluno = new ProvaAluno { ProvaId = 1, AlunoRA = 123 };
            var prova = ObterProva(provaAluno.ProvaId);
            prova.FormatoTai = false;

            mediator.Setup(m => m.Send(It.IsAny<ObterProvaPorIdQuery>(), CancellationToken.None)).ReturnsAsync(prova);
            mediator.Setup(m => m.Send(It.IsAny<ObterProvaAlunoPorProvaIdRaQuery>(), CancellationToken.None)).ReturnsAsync(provaAluno);

            var resultado = await useCase.Executar(new MensagemRabbit(JsonSerializer.Serialize(provaAluno), Guid.NewGuid()));

            Assert.False(resultado);
            mediator.Verify(m => m.Send(It.IsAny<ObterProvaPorIdQuery>(), CancellationToken.None), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterProvaAlunoPorProvaIdRaQuery>(), CancellationToken.None), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<ExcluirRespostaAlunoCommand>(), CancellationToken.None), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<ExcluirQuestaoAlunoTaiCommand>(), CancellationToken.None), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<ExcluirProvaAlunoCommand>(), CancellationToken.None), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<ExcluirAlunoProvaProficienciaCommand>(), CancellationToken.None), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<LimparCacheProvaTaiCommand>(), CancellationToken.None), Times.Never);
            servicoLog.Verify(l => l.Registrar(It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_ProvaAlunoBanco_Null()
        {
            var provaAluno = new ProvaAluno { ProvaId = 1, AlunoRA = 123 };
            var prova = ObterProva(provaAluno.ProvaId);

            mediator.Setup(m => m.Send(It.IsAny<ObterProvaPorIdQuery>(), CancellationToken.None)).ReturnsAsync(prova);
            mediator.Setup(m => m.Send(It.IsAny<ObterProvaAlunoPorProvaIdRaQuery>(), default)).ReturnsAsync((ProvaAluno)null);

            var resultado = await useCase.Executar(new MensagemRabbit(JsonSerializer.Serialize(provaAluno), Guid.NewGuid()));

            Assert.False(resultado);
            mediator.Verify(m => m.Send(It.IsAny<ObterProvaPorIdQuery>(), CancellationToken.None), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterProvaAlunoPorProvaIdRaQuery>(), CancellationToken.None), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ExcluirRespostaAlunoCommand>(), CancellationToken.None), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<ExcluirQuestaoAlunoTaiCommand>(), CancellationToken.None), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<ExcluirProvaAlunoCommand>(), CancellationToken.None), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<ExcluirAlunoProvaProficienciaCommand>(), CancellationToken.None), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<LimparCacheProvaTaiCommand>(), CancellationToken.None), Times.Never);
            servicoLog.Verify(l => l.Registrar(It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Tratar_Erro_Geral()
        {
            var provaAluno = new ProvaAluno { ProvaId = 1, AlunoRA = 123 };

            mediator.Setup(m => m.Send(It.IsAny<ObterProvaPorIdQuery>(), CancellationToken.None)).Throws(new Exception("Erro inesperado"));

            var resultado = await useCase.Executar(new MensagemRabbit(JsonSerializer.Serialize(provaAluno), Guid.NewGuid()));

            Assert.False(resultado);
            servicoLog.Verify(l => l.Registrar(It.IsAny<Exception>()), Times.Once);
        }

        private static Prova.Dominio.Prova ObterProva(long provaId)
        {
            return new Prova.Dominio.Prova
            {
                Id = provaId,
                FormatoTai = true
            };
        }
    }
}
