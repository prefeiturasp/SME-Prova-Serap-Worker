using Dapper.FluentMap.Dommel.Mapping;

namespace SME.SERAp.Prova.Dados
{
    public class ProvaMap : DommelEntityMap<Dominio.Prova>
    {
        public ProvaMap()
        {
            ToTable("prova");
            
            Map(c => c.Id).ToColumn("id").IsKey();
            
            Map(c => c.LegadoId).ToColumn("prova_legado_id");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.InicioDownload).ToColumn("inicio_download");
            Map(c => c.Inicio).ToColumn("inicio");
            Map(c => c.DisciplinaId).ToColumn("disciplina_id");
            Map(c => c.Disciplina).ToColumn("disciplina");
            Map(c => c.Fim).ToColumn("fim");
            Map(c => c.Inclusao).ToColumn("inclusao");
            Map(c => c.TotalItens).ToColumn("total_itens");
            Map(c => c.TempoExecucao).ToColumn("tempo_execucao");
            Map(c => c.Senha).ToColumn("senha");
            Map(c => c.PossuiBIB).ToColumn("possui_bib");
            Map(c => c.TotalCadernos).ToColumn("total_cadernos");
            Map(c => c.Modalidade).ToColumn("modalidade");
            Map(c => c.OcultarProva).ToColumn("ocultar_prova");
            Map(c => c.AderirTodos).ToColumn("aderir_todos");
            Map(c => c.Multidisciplinar).ToColumn("multidisciplinar");
            Map(c => c.TipoProvaId).ToColumn("tipo_prova_id");
            Map(c => c.FormatoTai).ToColumn("formato_tai");
            Map(c => c.QtdItensSincronizacaoRespostas).ToColumn("qtd_itens_sincronizacao_respostas");
            Map(c => c.PermiteAvancarSemResponderTai).ToColumn("formato_tai_avancar_sem_responder");
            Map(c => c.PermiteVoltarItemAnteriorTai).ToColumn("formato_tai_voltar_item_anterior");
            Map(c => c.UltimaAtualizacao).ToColumn("ultima_atualizacao");
            Map(c => c.ProvaComProficiencia).ToColumn("prova_com_proficiencia");
            Map(c => c.ApresentarResultados).ToColumn("apresentar_resultados");
            Map(c => c.ApresentarResultadosPorItem).ToColumn("apresentar_resultados_por_item");
            Map(c => c.ExibirVideo).ToColumn("exibir_video");
            Map(c => c.ExibirAudio).ToColumn("exibir_audio");
        }
    }
}
