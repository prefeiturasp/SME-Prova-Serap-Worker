using Moq;
using SME.SERAp.Prova.Aplicacao.Queries.ObterAlunosProvaProficienciaBoletimPorProvaId;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Aplicacao.Test.Queries
{
    public class ObterAlunosProvaProficienciaBoletimPorProvaIdQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_Lista_De_AlunoProvaProficienciaBoletimDto()
        {
            var mockRepositorio = new Mock<IRepositorioAlunoProvaProficiencia>();

            var provaId = 123;

            var listaEsperada = new List<AlunoProvaProficienciaBoletimDto>
            {
                new AlunoProvaProficienciaBoletimDto { },
                new AlunoProvaProficienciaBoletimDto { }
            };

            mockRepositorio
                .Setup(r => r.ObterAlunosProvaProficienciaBoletimPorProvaId(provaId))
                .ReturnsAsync(listaEsperada);

            var handler = new ObterAlunosProvaProficienciaBoletimPorProvaIdQueryHandler(mockRepositorio.Object);

            var query = new ObterAlunosProvaProficienciaBoletimPorProvaIdQuery(provaId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(listaEsperada.Count, ((List<AlunoProvaProficienciaBoletimDto>)resultado).Count);
            Assert.Equal(listaEsperada, resultado);

            mockRepositorio.Verify(r => r.ObterAlunosProvaProficienciaBoletimPorProvaId(provaId), Times.Once);
        }
    }
}
