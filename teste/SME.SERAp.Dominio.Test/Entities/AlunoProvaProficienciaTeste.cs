using SME.SERAp.Prova.Dominio;
using System;
using Xunit;

namespace SME.SERAp.Dominio.Test.Entities
{
    public class AlunoProvaProficienciaTeste
    {
        [Fact]
        public void Deve_Validar_Proficiencia()
        {
            var alunoProvaProficiencia = ObterEntidade();
            alunoProvaProficiencia.Proficiencia = 150;
            var excecao = Record.Exception(() => alunoProvaProficiencia.Validar());
            Assert.Null(excecao);
        }

        [Fact]
        public void Deve_Lancar_Excecao_Proficiencia_For_Negativa()
        {
            var alunoProvaProficiencia = ObterEntidade();
            alunoProvaProficiencia.Proficiencia = -150;
            Assert.Throws<Exception>(() => alunoProvaProficiencia.Validar());
        }

        private AlunoProvaProficiencia ObterEntidade()
        {
            return new AlunoProvaProficiencia
            {
                AlunoId = 0,
                DisciplinaId = 0,
                ErroMedida = 0,
                Origem = AlunoProvaProficienciaOrigem.PSP_Dre,
                Proficiencia = 0,
                ProvaId = 0,
                Ra = 0,
                Tipo = 0,
                UltimaAtualizacao = new DateTime(2025, 06, 09)
            };
        }
    }
}
