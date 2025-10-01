using Moq;
using Moq.Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Data;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Prova.Dados.Test.Repositories
{
    internal class RepositorioProvaFake : RepositorioProva
    {
        private readonly IDbConnection _conexaoLeitura;
        private readonly IDbConnection _conexaoEscrita;

        public RepositorioProvaFake(
            ConnectionStringOptions options,
            IDbConnection conexaoLeitura,
            IDbConnection conexaoEscrita) : base(options)
        {
            _conexaoLeitura = conexaoLeitura;
            _conexaoEscrita = conexaoEscrita;
        }

        protected override IDbConnection ObterConexaoLeitura() => _conexaoLeitura;
        protected override IDbConnection ObterConexao() => _conexaoEscrita;
    }

    [Collection("ColecaoMapeamentos")]
    public class RepositorioProvaTeste
    {
        private readonly Mock<IDbConnection> conexaoLeitura;
        private readonly Mock<IDbConnection> conexaoEscrita;
        private readonly RepositorioProvaFake repositorio;

        public RepositorioProvaTeste()
        {
            conexaoLeitura = new Mock<IDbConnection>();
            conexaoEscrita = new Mock<IDbConnection>();

            repositorio = new RepositorioProvaFake(
                new ConnectionStringOptions(),
                conexaoLeitura.Object,
                conexaoEscrita.Object
            );
        }

        [Fact]
        public async Task Deve_Obter_Ano_Prova()
        {

            var servicoTelemetria = new Mock<IServicoTelemetria>();
            DapperExtensionMethods.Init(servicoTelemetria.Object);

            var esperado = 2023;

            servicoTelemetria
                .Setup(c => c.RegistrarComRetornoAsync<int?>(
                    It.IsAny<Func<Task<object>>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(esperado);

            var resultado = await repositorio.ObterAnoProva(1);
            Assert.NotNull(resultado);
            Assert.Equal(2023, resultado);
        }

        [Fact]
        public async Task Deve_Obter_Ano_Prova_Nulo()
        {
            var servicoTelemetria = new Mock<IServicoTelemetria>();
            DapperExtensionMethods.Init(servicoTelemetria.Object);

            servicoTelemetria
                .Setup(c => c.RegistrarComRetornoAsync<int?>(
                    It.IsAny<Func<Task<object>>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync((int?)null);

            var resultado = await repositorio.ObterAnoProva(1);
            Assert.Null(resultado);
        }
    }
}
