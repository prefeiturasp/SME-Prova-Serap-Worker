using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Prova.Aplicacao.Queries.ObterResultadoAlunoProvaSp;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Infra.Dtos;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Aplicacao.Test.Queries
{
    public class ObterResultadoAlunoProvaSpQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_ResultadoAlunoProvaSpDto()
        {
            var mockRepositorio = new Mock<IRepositorioAlunoProvaSpProficiencia>();
            var edicao = 2023;
            var areaDoConhecimento = 4;
            var alunoMatricula = "123456";
            var resultadoAlunoProvaSpDtoEsperado = new ResultadoAlunoProvaSpDto
            {
                AlunoMatricula = alunoMatricula,
                AreaConhecimentoID = areaDoConhecimento,
                Edicao = edicao.ToString(),
                NivelProficiencia = 2,
                Valor = 300.5M,
            };

            mockRepositorio
                .Setup(r => r.ObterResultadoAlunoProvaSp(edicao, areaDoConhecimento, alunoMatricula))
                .ReturnsAsync(resultadoAlunoProvaSpDtoEsperado);
            var handler = new ObterResultadoAlunoProvaSpQueryHandler(mockRepositorio.Object);
            var query = new ObterResultadoAlunoProvaSpQuery(edicao, areaDoConhecimento, alunoMatricula);
            var resultado = await handler.Handle(query, CancellationToken.None);
            Assert.NotNull(resultado);
            Assert.Equal(resultadoAlunoProvaSpDtoEsperado, resultado);
            mockRepositorio.Verify(r => r.ObterResultadoAlunoProvaSp(edicao, areaDoConhecimento, alunoMatricula), Times.Once);
        }
    }
}
