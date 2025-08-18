using Moq;
using SME.SERAp.Prova.Aplicacao.Commands;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using Xunit;

namespace SME.SERAp.Aplicacao.Teste.Commands
{
    public class ExcluirQuestaoAlunoTaiCommandHandlerTeste
    {
        private readonly Mock<IRepositorioQuestaoAlunoTai> repositorio;
        private readonly ExcluirQuestaoAlunoTaiCommandHandler handler;
        public ExcluirQuestaoAlunoTaiCommandHandlerTeste()
        {
            repositorio = new Mock<IRepositorioQuestaoAlunoTai>();
            handler = new ExcluirQuestaoAlunoTaiCommandHandler(repositorio.Object);
        }

        [Fact]
        public async void Deve_Excluir_Resposta_Questao_Aluno_Tai()
        {
            var resultadoEsperado = 1;
            var comando = new ExcluirQuestaoAlunoTaiCommand(1, 1);
            repositorio.Setup(x => x.ExcluirQuestaoAlunoTai(comando.ProvaId, comando.AlunoRa)).ReturnsAsync(resultadoEsperado);

            var resultado = await handler.Handle(comando, CancellationToken.None);

            Assert.Equal(resultadoEsperado, resultado);
            repositorio.Verify(x => x.ExcluirQuestaoAlunoTai(comando.ProvaId, comando.AlunoRa), Times.Once);
        }

        [Fact]
        public async void Deve_Retornar_Excecao()
        {
            var mensagemExcecao = "teste exceção.";
            var comando = new ExcluirQuestaoAlunoTaiCommand(1, 1);
            repositorio.Setup(x => x.ExcluirQuestaoAlunoTai(comando.ProvaId, comando.AlunoRa)).Throws(new Exception(mensagemExcecao));

            var excecao = await Assert.ThrowsAsync<Exception>(() => handler.Handle(comando, CancellationToken.None));
            Assert.Equal(mensagemExcecao, excecao.Message);
            repositorio.Verify(x => x.ExcluirQuestaoAlunoTai(comando.ProvaId, comando.AlunoRa), Times.Once);
        }
    }
}
