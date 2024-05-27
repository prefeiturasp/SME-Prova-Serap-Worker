using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public class ResultadoProvaConsolidadoEntityMap : IEntityTypeConfiguration<ResultadoProvaConsolidado>
    {
        public void Configure(EntityTypeBuilder<ResultadoProvaConsolidado> builder)
        {
            builder.ToTable("resultado_prova_consolidado");

            builder.Property(c => c.ProvaSerapId)
                .HasColumnName("prova_serap_id");

            builder.Property(c => c.ProvaSerapEstudantesId)
                .HasColumnName("prova_serap_estudantes_id");

            builder.Property(c => c.DreCodigoEol)
                .HasColumnName("dre_codigo_eol");

            builder.Property(c => c.DreSigla)
                .HasColumnName("dre_sigla");

            builder.Property(c => c.DreNome)
                .HasColumnName("dre_nome");

            builder.Property(c => c.UeCodigoEol)
                .HasColumnName("ue_codigo_eol");

            builder.Property(c => c.UeNome)
                .HasColumnName("ue_nome");

            builder.Property(c => c.TurmaAnoEscolar)
                .HasColumnName("turma_ano_escolar");

            builder.Property(c => c.TurmaAnoEscolarDescricao)
                .HasColumnName("turma_ano_escolar_descricao");

            builder.Property(c => c.TurmaCodigo)
                .HasColumnName("turma_codigo");

            builder.Property(c => c.TurmaDescricao)
                .HasColumnName("turma_descricao");

            builder.Property(c => c.AlunoCodigoEol)
                .HasColumnName("aluno_codigo_eol");

            builder.Property(c => c.AlunoNome)
                .HasColumnName("aluno_nome");

            builder.Property(c => c.AlunoSexo)
                .HasColumnName("aluno_sexo");

            builder.Property(c => c.AlunoDataNascimento)
                .HasColumnName("aluno_data_nascimento");

            builder.Property(c => c.ProvaComponente)
                .HasColumnName("prova_componente");

            builder.Property(c => c.ProvaCaderno)
                .HasColumnName("prova_caderno");

            builder.Property(c => c.ProvaQuantidadeQuestoes)
                .HasColumnName("prova_quantidade_questoes");

            builder.Property(c => c.AlunoFrequencia)
                .HasColumnName("aluno_frequencia");

            builder.Property(c => c.QuestaoId)
                .HasColumnName("questao_id");

            builder.Property(c => c.QuestaoOrdem)
                .HasColumnName("questao_ordem");

            builder.Property(c => c.Resposta)
                .HasColumnName("resposta");

            builder.Property(c => c.DataInicio)
                .HasColumnName("prova_data_inicio");

            builder.Property(c => c.DataFim)
                .HasColumnName("prova_data_entregue");
        }
    }
}
