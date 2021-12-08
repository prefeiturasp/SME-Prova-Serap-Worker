using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public class AlunoEntityMap : IEntityTypeConfiguration<Aluno>
    {
        public void Configure(EntityTypeBuilder<Aluno> builder)
        {
            builder.ToTable("aluno");

            builder.Property(c => c.Id).HasColumnName("id");

            builder.Property(c => c.Nome)
                .HasColumnName("nome")
                .IsRequired();

            builder.Property(c => c.RA)
                .HasColumnName("ra")
                .IsRequired();

            builder.Property(c => c.TurmaId)
                .HasColumnName("turma_id")
                .IsRequired();

            builder.Property(c => c.Situacao)
                .HasColumnName("situacao")
                .IsRequired();
        }
    }
}
