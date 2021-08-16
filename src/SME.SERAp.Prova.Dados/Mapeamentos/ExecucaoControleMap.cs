using Dapper.FluentMap.Dommel.Mapping;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public class ExecucaoControleMap : DommelEntityMap<ExecucaoControle>
    {
        public ExecucaoControleMap()
        {
            ToTable("execucao_controle");
            
            Map(c => c.Id).ToColumn("id").IsKey();
            
            Map(c => c.UltimaExecucao).ToColumn("ultima_execucao");
            Map(c => c.Tipo).ToColumn("execucao_tipo");            
        }
    }
}
