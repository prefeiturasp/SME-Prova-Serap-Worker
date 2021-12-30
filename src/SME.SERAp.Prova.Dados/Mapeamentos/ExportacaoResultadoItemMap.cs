using Dapper.FluentMap.Dommel.Mapping;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public class ExportacaoResultadoItemMap : DommelEntityMap<ExportacaoResultadoItem>
    {
        public ExportacaoResultadoItemMap()
        {
            ToTable("exportacao_resultado_item");

            Map(c => c.Id).ToColumn("id").IsKey();
            Map(c => c.ExportacaoResultadoId).ToColumn("exportacao_resultado_id");
            Map(c => c.DreCodigoEol).ToColumn("dre_codigo_eol");
            Map(c => c.UeCodigoEol).ToColumn("ue_codigo_eol");
            Map(c => c.CriadoEm).ToColumn("criado_em");
        }
    }
}
