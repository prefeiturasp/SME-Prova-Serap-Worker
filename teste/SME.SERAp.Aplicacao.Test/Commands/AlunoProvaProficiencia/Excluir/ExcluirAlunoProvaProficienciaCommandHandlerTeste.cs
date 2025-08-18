using Moq;
using SME.SERAp.Prova.Aplicacao.Commands.AlunoProvaProficiencia.Excluir;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Aplicacao.Test.Commands.AlunoProvaProficiencia.Excluir
{
    public class ExcluirAlunoProvaProficienciaCommandHandlerTeste
    {
        private readonly Mock<IRepositorioAlunoProvaProficiencia> repositorioAlunoProvaProficienciaMock;
        private readonly ExcluirAlunoProvaProficienciaCommandHandler handler;

        public ExcluirAlunoProvaProficienciaCommandHandlerTeste()
        {
            repositorioAlunoProvaProficienciaMock = new Mock<IRepositorioAlunoProvaProficiencia>();
            handler = new ExcluirAlunoProvaProficienciaCommandHandler(repositorioAlunoProvaProficienciaMock.Object);
        }

        [Fact(DisplayName = "Deve excluir a proficiência do aluno da prova com sucesso")]
        public async Task Deve_Excluir_Aluno_Prova_Proficiencia_Com_Sucesso()
        {
            var provaId = 123L;
            var alunoRa = 456L;
            var linhasAfetadasEsperadas = 1;

            var command = new ExcluirAlunoProvaProficienciaCommand(provaId, alunoRa);

            repositorioAlunoProvaProficienciaMock
                .Setup(r => r.ExcluirAlunoProvaProficiencia(provaId, alunoRa))
                .ReturnsAsync(linhasAfetadasEsperadas);

            var resultado = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(linhasAfetadasEsperadas, resultado);
            repositorioAlunoProvaProficienciaMock.Verify(r => r.ExcluirAlunoProvaProficiencia(provaId, alunoRa), Times.Once);
        }

        [Fact(DisplayName = "Deve propagar a exceção quando o repositório lançar uma exceção")]
        public async Task Deve_Lancar_Excecao_Ao_Excluir_Aluno_Prova_Proficiencia()
        {
            var provaId = 123L;
            var alunoRa = 456L;
            var mensagemExcecao = "Erro simulado ao excluir proficiência do aluno da prova.";

            var command = new ExcluirAlunoProvaProficienciaCommand(provaId, alunoRa);

            repositorioAlunoProvaProficienciaMock
                .Setup(r => r.ExcluirAlunoProvaProficiencia(provaId, alunoRa))
                .ThrowsAsync(new Exception(mensagemExcecao));

            var excecao = await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            repositorioAlunoProvaProficienciaMock.Verify(r => r.ExcluirAlunoProvaProficiencia(provaId, alunoRa), Times.Once);
        }
    }
}