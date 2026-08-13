using SME.SERAp.Prova.Dominio.Entidades.Presenca;
using System;
using Xunit;

namespace SME.SERAp.Dominio.Test.Entidades.Presenca
{
    public class ProvaPresencaTeste
    {
        [Fact]
        public void Deve_Criar_ProvaPresenca_Com_Valores_Padrao()
        {
            var provaPresenca = new ProvaPresenca();

            Assert.Equal(0, provaPresenca.Id);
            Assert.Null(provaPresenca.NomeProva);
            Assert.Equal(0, provaPresenca.AnoProva);
            Assert.Null(provaPresenca.DescricaoProva);
            Assert.Equal(default(DateTime), provaPresenca.DataInicialAplicacao);
            Assert.Equal(default(DateTime), provaPresenca.DataFinalAplicacao);
            Assert.Null(provaPresenca.DataCorte);
            Assert.False(provaPresenca.VinculaAlunoCadernoExtra);
            Assert.NotEqual(default(DateTime), provaPresenca.DataProcessamento);
            Assert.True(provaPresenca.DataProcessamento <= DateTime.UtcNow);
        }

        [Fact]
        public void Deve_Atribuir_E_Recuperar_Propriedades_Corretamente()
        {
            var dataInicial = new DateTime(2024, 10, 1, 9, 0, 0);
            var dataFinal = new DateTime(2024, 10, 5, 17, 0, 0);
            var dataCorte = new DateTime(2024, 10, 3, 12, 0, 0);
            var dataProcessamento = new DateTime(2024, 9, 30, 8, 0, 0);

            var provaPresenca = new ProvaPresenca
            {
                Id = 1,
                NomeProva = "Prova de Presença Teste",
                AnoProva = 2024,
                DescricaoProva = "Descrição da prova de presença",
                DataInicialAplicacao = dataInicial,
                DataFinalAplicacao = dataFinal,
                DataCorte = dataCorte,
                VinculaAlunoCadernoExtra = true,
                DataProcessamento = dataProcessamento
            };

            Assert.Equal(1, provaPresenca.Id);
            Assert.Equal("Prova de Presença Teste", provaPresenca.NomeProva);
            Assert.Equal(2024, provaPresenca.AnoProva);
            Assert.Equal("Descrição da prova de presença", provaPresenca.DescricaoProva);
            Assert.Equal(dataInicial, provaPresenca.DataInicialAplicacao);
            Assert.Equal(dataFinal, provaPresenca.DataFinalAplicacao);
            Assert.Equal(dataCorte, provaPresenca.DataCorte);
            Assert.True(provaPresenca.VinculaAlunoCadernoExtra);
            Assert.Equal(dataProcessamento, provaPresenca.DataProcessamento);
        }

        [Fact]
        public void Construtor_Deve_Definir_DataProcessamento_Como_UtcNow()
        {
            var antes = DateTime.UtcNow;
            var provaPresenca = new ProvaPresenca();
            var depois = DateTime.UtcNow;

            Assert.True(provaPresenca.DataProcessamento >= antes);
            Assert.True(provaPresenca.DataProcessamento <= depois);
            Assert.NotEqual(default(DateTime), provaPresenca.DataProcessamento);
        }
    }
}