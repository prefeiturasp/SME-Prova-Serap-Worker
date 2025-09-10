using Moq;
using SME.SERAp.Prova.Aplicacao.Commands;
using SME.SERAp.Prova.Dados.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Aplicacao.Test.Commands
{
    public class ExcluirAlunoProvaSpProficienciaCommandHandlerTeste
    {
        [Fact]
        public async Task Deve_Excluir_AlunoProvaSpProficiencia()
        {
            var mockRepositorio = new Mock<IRepositorioAlunoProvaSpProficiencia>();

            var anoLetivo = 2024;
            var disciplinaId = 4L;
            var alunoRa = 123456;

            mockRepositorio
                .Setup(r => r.ExcluirAlunoProvaSpProficiencia(anoLetivo, disciplinaId, alunoRa))
                .ReturnsAsync(1);

            var handler = new ExcluirAlunoProvaSpProficienciaCommandHandler(mockRepositorio.Object);
            var command = new ExcluirAlunoProvaSpProficienciaCommand(anoLetivo, disciplinaId, alunoRa);

            var resultado = await handler.Handle(command, CancellationToken.None);
            Assert.Equal(1, resultado);

            mockRepositorio.Verify(r => r.ExcluirAlunoProvaSpProficiencia(anoLetivo, disciplinaId, alunoRa), Times.Once);
        }
    }
}
