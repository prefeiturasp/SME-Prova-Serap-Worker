using Dapper.FluentMap.Dommel.Mapping;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public class ResultadoProvaConsolidadoMap : DommelEntityMap<ResultadoProvaConsolidado>
    {
        public ResultadoProvaConsolidadoMap()
        {
            ToTable("resultado_prova_consolidado");

            Map(c => c.Id).ToColumn("id").IsKey();
            Map(c => c.ProvaSerapId).ToColumn("prova_serap_id").IsKey();
            Map(c => c.ProvaSerapEstudantesId).ToColumn("prova_serap_estudantes_id").IsKey();
            Map(c => c.DreCodigoEol).ToColumn("dre_codigo_eol").IsKey();
            Map(c => c.DreSigla).ToColumn("dre_sigla").IsKey();
            Map(c => c.DreNome).ToColumn("dre_nome").IsKey();
            Map(c => c.UeCodigoEol).ToColumn("ue_codigo_eol").IsKey();
            Map(c => c.UeNome).ToColumn("ue_nome").IsKey();
            Map(c => c.TurmaAnoEscolar).ToColumn("turma_ano_escolar").IsKey();
            Map(c => c.TurmaAnoEscolarDescricao).ToColumn("turma_ano_escolar_descricao").IsKey();
            Map(c => c.TurmaCodigo).ToColumn("turma_codigo").IsKey();
            Map(c => c.TurmaDescricao).ToColumn("turma_descricao").IsKey();
            Map(c => c.AlunoCodigoEol).ToColumn("aluno_codigo_eol").IsKey();
            Map(c => c.AlunoNome).ToColumn("aluno_nome").IsKey();
            Map(c => c.AlunoSexo).ToColumn("aluno_sexo").IsKey();
            Map(c => c.AlunoDataNascimento).ToColumn("aluno_data_nascimento").IsKey();
            Map(c => c.ProvaComponente).ToColumn("prova_componente").IsKey();
            Map(c => c.ProvaCaderno).ToColumn("prova_caderno").IsKey();
            Map(c => c.ProvaQuantidadeQuestoes).ToColumn("prova_quantidade_questoes").IsKey();
            Map(c => c.AlunoFrequencia).ToColumn("aluno_frequencia").IsKey();
            Map(c => c.DataInicio).ToColumn("prova_data_inicio").IsKey();
            Map(c => c.DataFim).ToColumn("prova_data_entregue").IsKey();
            Map(c => c.QuestaoId).ToColumn("questao_id").IsKey();
            Map(c => c.QuestaoOrdem).ToColumn("questao_ordem").IsKey();
            Map(c => c.Resposta).ToColumn("resposta").IsKey();
        }
    }
}
