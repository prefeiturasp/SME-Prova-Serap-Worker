using SME.SERAp.Prova.Dominio.Entidades.Presenca;
using Xunit;

namespace SME.SERAp.Dominio.Test.Entidades.Presenca
{
    public class ProvaPresencaTurmaTeste
    {
        [Fact]
        public void Deve_Criar_ProvaPresencaTurma_Com_Valores_Padrao()
        {
            var provaPresencaTurma = new ProvaPresencaTurma();

            Assert.Equal(0, provaPresencaTurma.ProvaPresencaId);
            Assert.Equal(0, provaPresencaTurma.TurmaId);
        }

        [Fact]
        public void Deve_Atribuir_E_Recuperar_Propriedades_Corretamente()
        {
            var provaPresencaTurma = new ProvaPresencaTurma
            {
                ProvaPresencaId = 100,
                TurmaId = 200
            };

            Assert.Equal(100, provaPresencaTurma.ProvaPresencaId);
            Assert.Equal(200, provaPresencaTurma.TurmaId);
        }
    }
}