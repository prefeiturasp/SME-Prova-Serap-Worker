using SME.SERAp.Prova.Dominio.Entidades;
using System;
using Xunit;

namespace SME.SERAp.Dominio.Test.Entities
{
    public class AlunoProvaSpProficienciaTeste
    {
        [Fact]
        public void Deve_Atribuir_E_Recuperar_Propriedades_Corretamente()
        {
            var aluno = new AlunoProvaSpProficiencia();

            var alunoRa = 123456L;
            var anoEscolar = 9;
            var anoLetivo = 2024;
            var disciplinaId = 77L;
            var proficiencia = 85.5m;
            var nivelProficiencia = 3;
            var dataAtualizacao = new DateTime(2024, 6, 1);

            aluno.AlunoRa = alunoRa;
            aluno.AnoEscolar = anoEscolar;
            aluno.AnoLetivo = anoLetivo;
            aluno.DisciplinaId = disciplinaId;
            aluno.Proficiencia = proficiencia;
            aluno.NivelProficiencia = nivelProficiencia;
            aluno.DataAtualizacao = dataAtualizacao;

            Assert.Equal(alunoRa, aluno.AlunoRa);
            Assert.Equal(anoEscolar, aluno.AnoEscolar);
            Assert.Equal(anoLetivo, aluno.AnoLetivo);
            Assert.Equal(disciplinaId, aluno.DisciplinaId);
            Assert.Equal(proficiencia, aluno.Proficiencia);
            Assert.Equal(dataAtualizacao, aluno.DataAtualizacao);
            Assert.Equal(nivelProficiencia, aluno.NivelProficiencia);
        }

        [Fact]
        public void Deve_Possuir_Valores_Padrao_Apos_Instanciacao()
        {
            var aluno = new AlunoProvaSpProficiencia();

            Assert.Equal(0L, aluno.AlunoRa);
            Assert.Equal(0, aluno.AnoEscolar);
            Assert.Equal(0, aluno.AnoLetivo);
            Assert.Equal(0L, aluno.DisciplinaId);
            Assert.Equal(0m, aluno.Proficiencia);
            Assert.Equal(0, aluno.NivelProficiencia);
            Assert.Equal(default(DateTime), aluno.DataAtualizacao);
        }
    }
}
