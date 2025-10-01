using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System.Linq;
using Xunit;

namespace SME.SERAp.Prova.Dados.Test.Mapeamentos
{
    public class AlunoProvaProficienciaMapTeste
    {
        static AlunoProvaProficienciaMapTeste()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new AlunoProvaProficienciaMap());
                config.ForDommel();
            });
        }

        [Fact]
        public void Deve_Mapear_Entidade_Para_Tabela_Correta()
        {
            var map = (AlunoProvaProficienciaMap)FluentMapper.EntityMaps[typeof(AlunoProvaProficiencia)];

            Assert.Equal("aluno_prova_proficiencia", map.TableName);
        }

        [Fact]
        public void Deve_Mapear_Propriedades_Para_Colunas_Corretas()
        {
            var map = (AlunoProvaProficienciaMap)FluentMapper.EntityMaps[typeof(AlunoProvaProficiencia)];

            Assert.Equal("id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(AlunoProvaProficiencia.Id)).ColumnName);
            Assert.Equal("aluno_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(AlunoProvaProficiencia.AlunoId)).ColumnName);
            Assert.Equal("ra", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(AlunoProvaProficiencia.Ra)).ColumnName);
            Assert.Equal("prova_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(AlunoProvaProficiencia.ProvaId)).ColumnName);
            Assert.Equal("disciplina_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(AlunoProvaProficiencia.DisciplinaId)).ColumnName);
            Assert.Equal("proficiencia", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(AlunoProvaProficiencia.Proficiencia)).ColumnName);
            Assert.Equal("tipo", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(AlunoProvaProficiencia.Tipo)).ColumnName);
            Assert.Equal("origem", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(AlunoProvaProficiencia.Origem)).ColumnName);
            Assert.Equal("ultima_atualizacao", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(AlunoProvaProficiencia.UltimaAtualizacao)).ColumnName);
            Assert.Equal("erro_medida", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(AlunoProvaProficiencia.ErroMedida)).ColumnName);
        }

        [Fact]
        public void Deve_Definir_Id_Como_Chave_Primaria()
        {
            var map = (AlunoProvaProficienciaMap)FluentMapper.EntityMaps[typeof(AlunoProvaProficiencia)];
            var idColumn = map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(AlunoProvaProficiencia.Id)).ColumnName;
            Assert.Equal("id", idColumn);
        }
    }
}
