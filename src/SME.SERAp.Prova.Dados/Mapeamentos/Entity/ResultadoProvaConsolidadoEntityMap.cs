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

            builder.Property(c => c.Id).HasColumnName("id");

            builder.Property(c => c.ProvaSerapId)
                .HasColumnName("prova_serap_id")
                .IsRequired();

            builder.Property(c => c.ProvaSerapEstudantesId)
                .HasColumnName("prova_serap_estudantes_id")
                .IsRequired();

            builder.Property(c => c.DreCodigoEol)
                .HasColumnName("dre_codigo_eol")
                .IsRequired();

            builder.Property(c => c.DreSigla)
                .HasColumnName("dre_sigla")
                .IsRequired();

            builder.Property(c => c.DreNome)
                .HasColumnName("dre_nome")
                .IsRequired();

            builder.Property(c => c.UeCodigoEol)
                .HasColumnName("ue_codigo_eol")
                .IsRequired();

            builder.Property(c => c.UeNome)
                .HasColumnName("ue_nome")
                .IsRequired();

            builder.Property(c => c.TurmaAnoEscolar)
                .HasColumnName("turma_ano_escolar")
                .IsRequired();

            builder.Property(c => c.TurmaAnoEscolarDescricao)
                .HasColumnName("turma_ano_escolar_descricao")
                .IsRequired();

            builder.Property(c => c.TurmaCodigo)
                .HasColumnName("turma_codigo")
                .IsRequired();

            builder.Property(c => c.TurmaDescricao)
                .HasColumnName("turma_descricao")
                .IsRequired();

            builder.Property(c => c.AlunoCodigoEol)
                .HasColumnName("aluno_codigo_eol")
                .IsRequired();

            builder.Property(c => c.AlunoNome)
                .HasColumnName("aluno_nome")
                .IsRequired();

            builder.Property(c => c.AlunoSexo)
                .HasColumnName("aluno_sexo")
                .IsRequired();

            builder.Property(c => c.AlunoDataNascimento)
                .HasColumnName("aluno_data_nascimento")
                .IsRequired();

            builder.Property(c => c.ProvaComponente)
                .HasColumnName("prova_componente");

            builder.Property(c => c.ProvaCaderno)
                .HasColumnName("prova_caderno")
                .IsRequired();

            builder.Property(c => c.ProvaQuantidadeQuestoes)
                .HasColumnName("prova_quantidade_questoes")
                .IsRequired();

            builder.Property(c => c.AlunoFrequencia)
                .HasColumnName("aluno_frequencia")
                .IsRequired();

            builder.Property(c => c.QuestaoId)
                .HasColumnName("questao_id");

            builder.Property(c => c.QuestaoOrdem)
                .HasColumnName("questao_ordem");

            builder.Property(c => c.Resposta)
                .HasColumnName("resposta");

        }
    }
}
