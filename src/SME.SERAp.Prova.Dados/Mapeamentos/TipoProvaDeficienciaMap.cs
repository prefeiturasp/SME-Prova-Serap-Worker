using Dapper.FluentMap.Dommel.Mapping;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public class TipoProvaDeficienciaMap : DommelEntityMap<TipoProvaDeficiencia>
    {
        public TipoProvaDeficienciaMap()
        {
            ToTable("tipo_prova_deficiencia");

            Map(c => c.Id).ToColumn("id").IsKey();

            Map(c => c.DeficienciaId).ToColumn("deficiencia_id");
            Map(c => c.TipoProvaId).ToColumn("tipo_prova_id");
            Map(c => c.CriadoEm).ToColumn("criado_em");
            Map(c => c.AtualizadoEm).ToColumn("atualizado_em");
        }
    }
}
