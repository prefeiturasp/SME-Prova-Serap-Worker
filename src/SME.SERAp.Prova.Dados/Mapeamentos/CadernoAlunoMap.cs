using Dapper.FluentMap.Dommel.Mapping;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public class CadernoAlunoMap : DommelEntityMap<CadernoAluno>
    {
        public CadernoAlunoMap()
        {
            ToTable("caderno_aluno");
            
            Map(c => c.Id).ToColumn("id").IsKey();
            
            Map(c => c.ProvaId).ToColumn("prova_id");
            Map(c => c.AlunoId).ToColumn("aluno_id");
            Map(c => c.Caderno).ToColumn("caderno");
            Map(c => c.CadernoTai).ToColumn("caderno_tai");

        }
    }
}
