using Moq;
using SME.SERAp.Prova.Aplicacao;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Aplicacao.Test.Commands.ProvaAluno.Excluir
{
    public class ExcluirProvaAlunoCommandHandlerTeste
    {
        private readonly Mock<IRepositorioProvaAluno> repositorioProvaAlunoMock;
        private readonly ExcluirProvaAlunoCommandHandler handler;

        public ExcluirProvaAlunoCommandHandlerTeste()
        {
            repositorioProvaAlunoMock = new Mock<IRepositorioProvaAluno>();
            handler = new ExcluirProvaAlunoCommandHandler(repositorioProvaAlunoMock.Object);
        }

        [Fact(DisplayName = "Deve excluir a prova do aluno com sucesso")]
        public async Task Deve_Excluir_Prova_Aluno_Com_Sucesso()
        {
            var provaId = 123L;
            var alunoRa = 456L;
            var linhasAfetadasEsperadas = 1;
            var command = new ExcluirProvaAlunoCommand(provaId, alunoRa);

            repositorioProvaAlunoMock
                .Setup(r => r.ExcluirProvaAlunoAsync(provaId, alunoRa))
                .ReturnsAsync(linhasAfetadasEsperadas);

            var resultado = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(linhasAfetadasEsperadas, resultado);

            repositorioProvaAlunoMock.Verify(r => r.ExcluirProvaAlunoAsync(provaId, alunoRa), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar 0 se nenhuma prova do aluno for excluída")]
        public async Task Deve_Retornar_Zero_Se_Nenhuma_Prova_Aluno_For_Excluida()
        {
            var provaId = 123L;
            var alunoRa = 456L;
            var linhasAfetadasEsperadas = 0;

            var command = new ExcluirProvaAlunoCommand(provaId, alunoRa);

            repositorioProvaAlunoMock
                .Setup(r => r.ExcluirProvaAlunoAsync(provaId, alunoRa))
                .ReturnsAsync(linhasAfetadasEsperadas);

            var resultado = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(linhasAfetadasEsperadas, resultado);
            repositorioProvaAlunoMock.Verify(r => r.ExcluirProvaAlunoAsync(provaId, alunoRa), Times.Once);
        }

        [Fact(DisplayName = "Deve propagar a exceção quando o repositório lançar uma exceção")]
        public async Task Deve_Lancar_Excecao_Ao_Excluir_Prova_Aluno()
        {
            var provaId = 123L;
            var alunoRa = 456L;
            var mensagemExcecao = "Erro simulado ao excluir prova do aluno.";

            var command = new ExcluirProvaAlunoCommand(provaId, alunoRa);

            repositorioProvaAlunoMock
                .Setup(r => r.ExcluirProvaAlunoAsync(provaId, alunoRa))
                .ThrowsAsync(new Exception(mensagemExcecao));

            var excecao = await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
            Assert.Equal(mensagemExcecao, excecao.Message);
            repositorioProvaAlunoMock.Verify(r => r.ExcluirProvaAlunoAsync(provaId, alunoRa), Times.Once);
        }
    }
}