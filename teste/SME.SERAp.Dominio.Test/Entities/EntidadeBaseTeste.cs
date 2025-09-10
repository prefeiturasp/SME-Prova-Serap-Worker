using SME.SERAp.Prova.Dominio;
using Xunit;

namespace SME.SERAp.Dominio.Test.Entities
{
    public class EntidadeBaseTeste
    {
        private class EntidadeFake : EntidadeBase { }

        [Fact]
        public void DevePermitirAtribuirEObterId()
        {
            var entidade = new EntidadeFake { Id = 123 };

            Assert.Equal(123, entidade.Id);
        }
    }
}
