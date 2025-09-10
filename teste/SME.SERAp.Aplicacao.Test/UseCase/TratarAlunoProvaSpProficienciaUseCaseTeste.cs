using MediatR;
using Moq;
using SME.SERAp.Prova.Aplicacao.Commands;
using SME.SERAp.Prova.Aplicacao.Queries.ObterAlunoProvaSpProficiencia;
using SME.SERAp.Prova.Aplicacao.UseCase;
using SME.SERAp.Prova.Dominio.Entidades;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Testes.UseCases
{
    public class TratarAlunoProvaSpProficienciaUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IServicoLog> servicoLog;
        private readonly TratarAlunoProvaSpProficienciaUseCase useCase;

        public TratarAlunoProvaSpProficienciaUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            servicoLog = new Mock<IServicoLog>();
            useCase = new TratarAlunoProvaSpProficienciaUseCase(mediator.Object, servicoLog.Object);
        }

        [Fact]
        public async Task Deve_Retornar_False_Se_Mensagem_For_Nula()
        {
            var mensagemRabbit = new MensagemRabbit(string.Empty, Guid.NewGuid());

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.False(resultado);
            servicoLog.Verify(s => s.Registrar(It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Inserir_Aluno_Prova_Sp_Proficiencia_Quando_Nao_Existir_Registro()
        {
            var aluno = new AlunoProvaSpProficiencia
            {
                AnoLetivo = 2025,
                DisciplinaId = 1,
                AnoEscolar = 9
            };

            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(aluno), Guid.NewGuid());
            mediator.Setup(m => m.Send(It.IsAny<ObterAlunoProvaSpProficienciaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((AlunoProvaSpProficiencia)null!);

            mediator.Setup(m => m.Send(It.IsAny<InserirAlunoProvaSpProficienciaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<InserirAlunoProvaSpProficienciaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ExcluirAlunoProvaSpProficienciaCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Excluir_E_Inserir_Aluno_Prova_Sp_Proficiencia_Quando_Registro_Existente()
        {
            var aluno = new AlunoProvaSpProficiencia
            {
                AnoLetivo = 2025,
                DisciplinaId = 1,
                AnoEscolar = 9
            };

            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(aluno), Guid.NewGuid());
            mediator.Setup(m => m.Send(It.IsAny<ObterAlunoProvaSpProficienciaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AlunoProvaSpProficiencia());
            mediator.Setup(m => m.Send(It.IsAny<ExcluirAlunoProvaSpProficienciaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);
            mediator.Setup(m => m.Send(It.IsAny<InserirAlunoProvaSpProficienciaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<ExcluirAlunoProvaSpProficienciaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<InserirAlunoProvaSpProficienciaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_False_Quando_Ocorre_Excecao()
        {
            var aluno = new AlunoProvaSpProficiencia
            {
                AnoLetivo = 2025,
                DisciplinaId = 1,
                AnoEscolar = 9
            };

            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(aluno), Guid.NewGuid());
            mediator.Setup(m => m.Send(It.IsAny<ObterAlunoProvaSpProficienciaQuery>(), It.IsAny<CancellationToken>()))
               .Throws(new Exception("Teste erro."));

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.False(resultado);
            servicoLog.Verify(s => s.Registrar(It.IsAny<Exception>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<InserirAlunoProvaSpProficienciaCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
