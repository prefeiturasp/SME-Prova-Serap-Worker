using Dapper.FluentMap.Dommel.Mapping;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public class TipoProvaMap : DommelEntityMap<TipoProva>
    {
        public TipoProvaMap()
        {
            ToTable("tipo_prova");

            Map(c => c.Id).ToColumn("id").IsKey();

            Map(c => c.LegadoId).ToColumn("legado_id");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.ParaEstudanteComDeficiencia).ToColumn("para_estudante_com_deficiencia");
            Map(c => c.CriadoEm).ToColumn("criado_em");
            Map(c => c.AtualizadoEm).ToColumn("atualizado_em");
        }
    }
}
