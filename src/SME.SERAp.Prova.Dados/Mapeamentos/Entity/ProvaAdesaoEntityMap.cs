using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public class ProvaAdesaoEntityMap : IEntityTypeConfiguration<ProvaAdesao>
    {
        public void Configure(EntityTypeBuilder<ProvaAdesao> builder)
        {

            builder.ToTable("prova_adesao");

            builder.Property(c => c.Id).HasColumnName("id");

            builder.Property(c => c.ProvaId)
                .HasColumnName("prova_id")
                .IsRequired();

            builder.Property(c => c.UeId)
                .HasColumnName("ue_id")
                .IsRequired();

            builder.Property(c => c.AlunoRa)
                .HasColumnName("aluno_ra")
                .IsRequired();

            builder.Property(c => c.AnoTurma)
                .HasColumnName("ano_turma")
                .IsRequired();
            builder.Property(c => c.TipoTurma)
                .HasColumnName("tipo_turma")
                .IsRequired();
            builder.Property(c => c.Modalidade)
                .HasColumnName("modalidade_codigo")
                .IsRequired();
            builder.Property(c => c.Tipoturno)
                .HasColumnName("tipo_turno")
                .IsRequired();

            builder.Property(c => c.CriadoEm)
                .HasColumnName("criado_em")
                .IsRequired();

            builder.Property(c => c.AtualizadoEm)
                .HasColumnName("atualizado_em")
                .IsRequired();

        }
    }
}
