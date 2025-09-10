using Moq;
using Moq.Dapper;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Prova.Dados.Test.Repositories
{
    internal class RepositorioAlunoProvaProficienciaFake : RepositorioAlunoProvaProficiencia
    {
        private readonly IDbConnection _conexaoLeitura;
        private readonly IDbConnection _conexaoEscrita;

        public RepositorioAlunoProvaProficienciaFake(
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
    public class RepositorioAlunoProvaProficienciaTeste
    {
        private readonly Mock<IDbConnection> conexaoLeitura;
        private readonly Mock<IDbConnection> conexaoEscrita;
        private readonly RepositorioAlunoProvaProficienciaFake repositorio;

        public RepositorioAlunoProvaProficienciaTeste()
        {
            conexaoLeitura = new Mock<IDbConnection>();
            conexaoEscrita = new Mock<IDbConnection>();

            repositorio = new RepositorioAlunoProvaProficienciaFake(
                new ConnectionStringOptions(),
                conexaoLeitura.Object,
                conexaoEscrita.Object
            );
        }

        [Fact]
        public async Task Deve_Obter_Alunos_Prova_Proficiencia_Boletim_Por_ProvaId()
        {
            var servicoTelemetria = new Mock<IServicoTelemetria>();
            DapperExtensionMethods.Init(servicoTelemetria.Object);

            var provaId = 123;
            var alunos = new List<AlunoProvaProficienciaBoletimDto>
            {
                new AlunoProvaProficienciaBoletimDto { ProvaId = provaId, NomeAluno = "Aluno 1" },
                new AlunoProvaProficienciaBoletimDto { ProvaId = provaId, NomeAluno = "Aluno 2" }
            };

            servicoTelemetria
                .Setup(c => c.RegistrarComRetornoAsync<AlunoProvaProficienciaBoletimDto>(
                    It.IsAny<Func<Task<object>>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(alunos);

            var resultado = await repositorio.ObterAlunosProvaProficienciaBoletimPorProvaId(provaId);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.All(resultado, a => Assert.Equal(provaId, a.ProvaId));
        }
    }
}
