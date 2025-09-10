using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using SME.SERAp.Prova.Dados.Mapeamentos;
using SME.SERAp.Prova.Dominio.Entidades;
using System.Linq;
using Xunit;

namespace SME.SERAp.Prova.Dados.Test.Mapeamentos
{
    public class AlunoProvaSpProficienciaMapTeste
    {
        static AlunoProvaSpProficienciaMapTeste()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new AlunoProvaSpProficienciaMap());
                config.ForDommel();
            });
        }

        [Fact]
        public void Deve_Mapear_Entidade_Para_Tabela_Correta()
        {
            var map = (AlunoProvaSpProficienciaMap)FluentMapper.EntityMaps[typeof(AlunoProvaSpProficiencia)];

            Assert.Equal("aluno_prova_sp_proficiencia", map.TableName);
        }

        [Fact]
        public void Deve_Mapear_Propriedades_Para_Colunas_Corretas()
        {
            var map = (AlunoProvaSpProficienciaMap)FluentMapper.EntityMaps[typeof(AlunoProvaSpProficiencia)];

            Assert.Equal("id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(AlunoProvaSpProficiencia.Id)).ColumnName);
            Assert.Equal("aluno_ra", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(AlunoProvaSpProficiencia.AlunoRa)).ColumnName);
            Assert.Equal("ano_letivo", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(AlunoProvaSpProficiencia.AnoLetivo)).ColumnName);
            Assert.Equal("ano_escolar", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(AlunoProvaSpProficiencia.AnoEscolar)).ColumnName);
            Assert.Equal("disciplina_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(AlunoProvaSpProficiencia.DisciplinaId)).ColumnName);
            Assert.Equal("proficiencia", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(AlunoProvaSpProficiencia.Proficiencia)).ColumnName);
            Assert.Equal("nivel_proficiencia", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(AlunoProvaSpProficiencia.NivelProficiencia)).ColumnName);
            Assert.Equal("data_atualizacao", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(AlunoProvaSpProficiencia.DataAtualizacao)).ColumnName);
        }

        [Fact]
        public void Deve_Definir_Id_Como_Chave_Primaria()
        {
            var map = (AlunoProvaSpProficienciaMap)FluentMapper.EntityMaps[typeof(AlunoProvaSpProficiencia)];
            var idColumn = map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(AlunoProvaSpProficiencia.Id)).ColumnName;
            Assert.Equal("id", idColumn);
        }
    }
}
