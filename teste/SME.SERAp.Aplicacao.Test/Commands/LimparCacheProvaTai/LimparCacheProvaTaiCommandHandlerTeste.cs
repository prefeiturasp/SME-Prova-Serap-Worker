using Moq;
using SME.SERAp.Prova.Aplicacao.Commands;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Aplicacao.Teste.Commands
{
    public class LimparCacheProvaTaiCommandHandlerTeste
    {
        private readonly Mock<IRepositorioCache> repositorioCache;
        private readonly Mock<IRepositorioAluno> repositorioAluno;
        private readonly LimparCacheProvaTaiCommandHandler handler;

        public LimparCacheProvaTaiCommandHandlerTeste()
        {
            repositorioCache = new Mock<IRepositorioCache>();
            repositorioAluno = new Mock<IRepositorioAluno>();
            handler = new LimparCacheProvaTaiCommandHandler(repositorioCache.Object, repositorioAluno.Object);
        }

        [Fact]
        public async Task Deve_Limpar_Cache_Com_Sucesso()
        {
            // Arrange
            var provaId = 1L;
            var alunoRa = 123456L;
            var command = new LimparCacheProvaTaiCommand(provaId, alunoRa);

            repositorioAluno.Setup(r => r.ObterAlunoPorCodigo(alunoRa))
                .ReturnsAsync(new Aluno { Id = 1 });

            repositorioCache.Setup(r => r.RemoverRedisAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var resultado = await handler.Handle(command, CancellationToken.None);

            Assert.True(resultado);
            repositorioCache.Verify(r => r.RemoverRedisAsync(It.IsAny<string>()), Times.Exactly(5));
        }

        [Fact]
        public async Task Nao_Deve_Limpar_Cache_Se_Aluno_Nao_Encontrado()
        {
            var provaId = 1L;
            var alunoRa = 123456L;
            var command = new LimparCacheProvaTaiCommand(provaId, alunoRa);

            repositorioAluno.Setup(r => r.ObterAlunoPorCodigo(alunoRa))
                .Returns(Task.FromResult<Aluno>(null));

            repositorioCache.Setup(r => r.RemoverRedisAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var resultado = await handler.Handle(command, CancellationToken.None);

            Assert.False(resultado);
            repositorioCache.Verify(r => r.RemoverRedisAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Retornar_False_Caso_Excecao()
        {
            // Arrange
            var provaId = 1L;
            var alunoRa = 123456L;
            var command = new LimparCacheProvaTaiCommand(provaId, alunoRa);

            repositorioAluno.Setup(r => r.ObterAlunoPorCodigo(alunoRa))
                .ReturnsAsync(new Aluno { Id = 1 });

            repositorioCache.Setup(r => r.RemoverRedisAsync(It.IsAny<string>()))
                .Throws(new Exception());

            var resultado = await handler.Handle(command, CancellationToken.None);

            Assert.False(resultado);
        }
    }
}
