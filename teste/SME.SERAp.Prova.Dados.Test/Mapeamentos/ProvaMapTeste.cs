using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using System.Linq;
using Xunit;

namespace SME.SERAp.Prova.Dados.Test.Mapeamentos
{
    public class ProvaMapTeste
    {
        static ProvaMapTeste()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new ProvaMap());
                config.ForDommel();
            });
        }

        [Fact]
        public void Deve_Mapear_Entidade_Para_Tabela_Correta()
        {
            var map = (ProvaMap)FluentMapper.EntityMaps[typeof(SME.SERAp.Prova.Dominio.Prova)];
            Assert.Equal("prova", map.TableName);
        }

        [Fact]
        public void Deve_Mapear_Propriedades_Para_Colunas_Corretas()
        {
            var map = (ProvaMap)FluentMapper.EntityMaps[typeof(SME.SERAp.Prova.Dominio.Prova)];

            Assert.Equal("id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.Id)).ColumnName);
            Assert.Equal("prova_legado_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.LegadoId)).ColumnName);
            Assert.Equal("descricao", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.Descricao)).ColumnName);
            Assert.Equal("inicio_download", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.InicioDownload)).ColumnName);
            Assert.Equal("inicio", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.Inicio)).ColumnName);
            Assert.Equal("disciplina_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.DisciplinaId)).ColumnName);
            Assert.Equal("disciplina", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.Disciplina)).ColumnName);
            Assert.Equal("fim", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.Fim)).ColumnName);
            Assert.Equal("inclusao", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.Inclusao)).ColumnName);
            Assert.Equal("total_itens", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.TotalItens)).ColumnName);
            Assert.Equal("tempo_execucao", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.TempoExecucao)).ColumnName);
            Assert.Equal("senha", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.Senha)).ColumnName);
            Assert.Equal("possui_bib", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.PossuiBIB)).ColumnName);
            Assert.Equal("total_cadernos", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.TotalCadernos)).ColumnName);
            Assert.Equal("modalidade", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.Modalidade)).ColumnName);
            Assert.Equal("ocultar_prova", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.OcultarProva)).ColumnName);
            Assert.Equal("aderir_todos", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.AderirTodos)).ColumnName);
            Assert.Equal("multidisciplinar", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.Multidisciplinar)).ColumnName);
            Assert.Equal("tipo_prova_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.TipoProvaId)).ColumnName);
            Assert.Equal("formato_tai", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.FormatoTai)).ColumnName);
            Assert.Equal("formato_tai_item", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.ProvaFormatoTaiItem)).ColumnName);
            Assert.Equal("qtd_itens_sincronizacao_respostas", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.QtdItensSincronizacaoRespostas)).ColumnName);
            Assert.Equal("formato_tai_avancar_sem_responder", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.PermiteAvancarSemResponderTai)).ColumnName);
            Assert.Equal("formato_tai_voltar_item_anterior", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.PermiteVoltarItemAnteriorTai)).ColumnName);
            Assert.Equal("ultima_atualizacao", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.UltimaAtualizacao)).ColumnName);
            Assert.Equal("prova_com_proficiencia", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.ProvaComProficiencia)).ColumnName);
            Assert.Equal("apresentar_resultados", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.ApresentarResultados)).ColumnName);
            Assert.Equal("apresentar_resultados_por_item", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.ApresentarResultadosPorItem)).ColumnName);
            Assert.Equal("exibir_video", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.ExibirVideo)).ColumnName);
            Assert.Equal("exibir_audio", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(SME.SERAp.Prova.Dominio.Prova.ExibirAudio)).ColumnName);
        }

        [Fact]
        public void Deve_Definir_Id_Como_Chave_Primaria()
        {
            var map = (ProvaMap)FluentMapper.EntityMaps[typeof(SME.SERAp.Prova.Dominio.Prova)];
            var idColumn = map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Dominio.Prova.Id)).ColumnName;
            Assert.Equal("id", idColumn);
        }
    }
}
