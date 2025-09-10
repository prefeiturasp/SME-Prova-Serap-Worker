using Moq;
using Moq.Dapper;
using SME.SERAp.Prova.Dados.Repositorios.Serap;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Dominio.Entidades;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Data;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Prova.Dados.Test.Repositories
{
    internal class RepositorioAlunoProvaSpProficienciaFake : RepositorioAlunoProvaSpProficiencia
    {
        private readonly IDbConnection _conexaoLeitura;
        private readonly IDbConnection _conexaoEscrita;
        private readonly IDbConnection _conexaoProvaSp;

        public RepositorioAlunoProvaSpProficienciaFake(
            ConnectionStringOptions options,
            IDbConnection conexaoLeitura,
            IDbConnection conexaoEscrita,
            IDbConnection conexaoProvaSp) : base(options)
        {
            _conexaoLeitura = conexaoLeitura;
            _conexaoEscrita = conexaoEscrita;
            _conexaoProvaSp = conexaoProvaSp;
        }

        protected override IDbConnection ObterConexaoLeitura() => _conexaoLeitura;
        protected override IDbConnection ObterConexao() => _conexaoEscrita;
        protected override IDbConnection ObterConexaoProvaSp() => _conexaoProvaSp;
    }

    [Collection("ColecaoMapeamentos")]
    public class RepositorioAlunoProvaSpProficienciaTeste
    {
        private readonly Mock<IDbConnection> conexaoLeitura;
        private readonly Mock<IDbConnection> conexaoEscrita;
        private readonly Mock<IDbConnection> conexaoProvaSp;
        private readonly RepositorioAlunoProvaSpProficienciaFake repositorio;

        public RepositorioAlunoProvaSpProficienciaTeste()
        {
            conexaoLeitura = new Mock<IDbConnection>();
            conexaoEscrita = new Mock<IDbConnection>();
            conexaoProvaSp = new Mock<IDbConnection>();

            repositorio = new RepositorioAlunoProvaSpProficienciaFake(
                new ConnectionStringOptions(),
                conexaoLeitura.Object,
                conexaoEscrita.Object,
                conexaoProvaSp.Object
            );
        }

        [Fact]
        public async Task Deve_Obter_Resultado_Aluno_ProvaSp()
        {
            var servicoTelemetria = new Mock<IServicoTelemetria>();
            DapperExtensionMethods.Init(servicoTelemetria.Object);

            var esperado = new ResultadoAlunoProvaSpDto
            {
                Edicao = "2024",
                AreaConhecimentoID = 2,
                AnoEscolar = "9º",
                AlunoMatricula = "123456",
                NivelProficiencia = 2,
                Valor = 85.5M
            };

            servicoTelemetria
                .Setup(c => c.RegistrarComRetornoAsync<ResultadoAlunoProvaSpDto>(
                    It.IsAny<Func<Task<object>>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(esperado);

            var resultado = await repositorio.ObterResultadoAlunoProvaSp(2024, 2, "123456");

            Assert.NotNull(resultado);
            Assert.Equal("2024", resultado.Edicao);
            Assert.Equal(2, resultado.AreaConhecimentoID);
            Assert.Equal("123456", resultado.AlunoMatricula);
            Assert.Equal(2, resultado.NivelProficiencia);
            Assert.Equal(85.5M, resultado.Valor);
        }

        [Fact]
        public async Task Deve_Obter_Aluno_ProvaSpProficiencia()
        {

            var servicoTelemetria = new Mock<IServicoTelemetria>();
            DapperExtensionMethods.Init(servicoTelemetria.Object);
            var esperado = new AlunoProvaSpProficiencia
            {
                Id = 1,
                AlunoRa = 987654,
                AnoEscolar = 9,
                AnoLetivo = 2024,
                DisciplinaId = 3,
                NivelProficiencia = 2,
                Proficiencia = 75.5m
            };

            servicoTelemetria
                .Setup(c => c.RegistrarComRetornoAsync<AlunoProvaSpProficiencia>(
                    It.IsAny<Func<Task<object>>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(esperado);

            var resultado = await repositorio.ObterAlunoProvaSpProficiencia(2024, 3, 987654);

            Assert.NotNull(resultado);
            Assert.Equal(987654, resultado.AlunoRa);
            Assert.Equal(3, resultado.DisciplinaId);
            Assert.Equal(2, resultado.NivelProficiencia);
            Assert.Equal(75.5M, resultado.Proficiencia);
        }

        [Fact]
        public async Task Deve_Excluir_Aluno_ProvaSpProficiencia()
        {

            var servicoTelemetria = new Mock<IServicoTelemetria>();
            DapperExtensionMethods.Init(servicoTelemetria.Object);
            servicoTelemetria
                .Setup(c => c.RegistrarComRetornoAsync<int>(
                    It.IsAny<Func<Task<object>>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(1);

            var resultado = await repositorio.ExcluirAlunoProvaSpProficiencia(2024, 3, 987654);

            Assert.Equal(1, resultado);
        }
    }
}
