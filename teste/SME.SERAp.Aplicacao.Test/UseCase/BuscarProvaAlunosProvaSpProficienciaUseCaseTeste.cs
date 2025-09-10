using MediatR;
using Moq;
using SME.SERAp.Prova.Aplicacao;
using SME.SERAp.Prova.Aplicacao.Queries.ObterAlunosProvaProficienciaBoletimPorProvaId;
using SME.SERAp.Prova.Aplicacao.UseCase;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Testes.UseCases
{
    public class BuscarProvaAlunosProvaSpProficienciaUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IServicoLog> servicoLog;
        private readonly BuscarProvaAlunosProvaSpProficienciaUseCase useCase;

        public BuscarProvaAlunosProvaSpProficienciaUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            servicoLog = new Mock<IServicoLog>();

            useCase = new BuscarProvaAlunosProvaSpProficienciaUseCase(
                mediator.Object,
                servicoLog.Object
            );
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_ProvaId_For_Zero()
        {
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(0), Guid.NewGuid());
            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.False(resultado);
            servicoLog.Verify(x => x.Registrar(It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_False_Quando_Nao_Existirem_Alunos()
        {
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(123), Guid.NewGuid());
            mediator
                .Setup(m => m.Send(It.IsAny<ObterAlunosProvaProficienciaBoletimPorProvaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoProvaProficienciaBoletimDto>());

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.False(resultado);
            mediator.Verify(m => m.Send(It.IsAny<ObterAlunosProvaProficienciaBoletimPorProvaIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Publicar_Mensagens_Quando_Existirem_Alunos()
        {
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(123), Guid.NewGuid());
            var alunos = new List<AlunoProvaProficienciaBoletimDto>
            {
                new AlunoProvaProficienciaBoletimDto
                {
                    CodigoDre = 123,
                    CodigoUe = "UE1",
                    NomeUe = "Escola Teste",
                    ProvaId = 123,
                    NomeProva = "Prova X",
                    AnoEscolar = 5,
                    Turma = "A",
                    CodigoAluno = 123456,
                    NomeAluno = "João",
                    NomeDisciplina = "Matemática",
                    DisciplinaId = 1,
                    ProvaStatus = ProvaStatus.Finalizado,
                    Proficiencia = 200,
                    ErroMedida = 2,
                    NivelCodigo = 3
                }
            };

            mediator
                .Setup(m => m.Send(It.IsAny<ObterAlunosProvaProficienciaBoletimPorProvaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunos);

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_RegistrarLog_E_Retornar_False_Quando_Ocorre_Excecao()
        {
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(123), Guid.NewGuid());
            mediator
                .Setup(m => m.Send(It.IsAny<ObterAlunosProvaProficienciaBoletimPorProvaIdQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro inesperado"));

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.False(resultado);
            servicoLog.Verify(x => x.Registrar(It.IsAny<Exception>()), Times.Once);
        }
    }
}
