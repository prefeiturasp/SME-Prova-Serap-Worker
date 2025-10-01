using Moq;
using SME.SERAp.Prova.Aplicacao.Commands;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio.Entidades;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Commands
{
    public class InserirAlunoProvaSpProficienciaCommandHandlerTeste
    {
        [Fact]
        public async Task Deve_Inserir_Aluno_Prova_Sp_Proficiencia()
        {
            var repositorio = new Mock<IRepositorioAlunoProvaSpProficiencia>();
            var handler = new InserirAlunoProvaSpProficienciaCommandHandler(repositorio.Object);
            var alunoProvaSpProficiencia = new AlunoProvaSpProficiencia
            {
                Id = 1,
                AnoEscolar = 5,
                AlunoRa = 12345,
                AnoLetivo = 2024,
                DataAtualizacao = DateTime.Now,
                DisciplinaId = 1,
                NivelProficiencia = 2,
                Proficiencia = 250.5M,
                UeCodigo = "000191",
            };

            var command = new InserirAlunoProvaSpProficienciaCommand(alunoProvaSpProficiencia);
            repositorio
                .Setup(r => r.IncluirAsync(It.IsAny<AlunoProvaSpProficiencia>()))
                .ReturnsAsync(1);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(1, result);
            repositorio.Verify(r => r.IncluirAsync(It.Is<AlunoProvaSpProficiencia>(a => a == alunoProvaSpProficiencia)), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Repositorio_Falhar()
        {
            var repositorio = new Mock<IRepositorioAlunoProvaSpProficiencia>();
            var handler = new InserirAlunoProvaSpProficienciaCommandHandler(repositorio.Object);
            var alunoProvaSpProficiencia = new AlunoProvaSpProficiencia
            {
                Id = 1,
                AnoEscolar = 5,
                AlunoRa = 12345,
                AnoLetivo = 2024,
                DataAtualizacao = DateTime.Now,
                DisciplinaId = 1,
                NivelProficiencia = 2,
                Proficiencia = 250.5M,
                UeCodigo = "000191",
            };

            var command = new InserirAlunoProvaSpProficienciaCommand(alunoProvaSpProficiencia);

            repositorio
                .Setup(r => r.IncluirAsync(It.IsAny<AlunoProvaSpProficiencia>()))
                .ThrowsAsync(new Exception("Erro ao inserir no banco"));

            var exception = await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            Assert.Equal("Erro ao inserir no banco", exception.Message);
            repositorio.Verify(r => r.IncluirAsync(It.IsAny<AlunoProvaSpProficiencia>()), Times.Once);
        }
    }
}
