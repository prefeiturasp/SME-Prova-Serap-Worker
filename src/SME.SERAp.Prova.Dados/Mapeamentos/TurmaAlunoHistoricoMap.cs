using Dapper.FluentMap.Dommel.Mapping;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public class TurmaAlunoHistoricoMap : DommelEntityMap<TurmaAlunoHistorico>
    {
        public TurmaAlunoHistoricoMap()
        {
            ToTable("turma_aluno_historico");

            Map(c => c.Id).ToColumn("id").IsKey();

            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.AlunoId).ToColumn("aluno_id");
            Map(c => c.DataMatricula).ToColumn("data_matricula");
            Map(c => c.DataSituacao).ToColumn("data_situacao");
            Map(c => c.CriadoEm).ToColumn("criado_em");
            Map(c => c.AtualizadoEm).ToColumn("atualizado_em");
        }
    }
}
