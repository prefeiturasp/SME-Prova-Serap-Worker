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

            builder.Property(c => c.DataAtualizacao)
               .HasColumnName("data_atualizacao")
               .IsRequired();

            builder.Property(c => c.NomeSocial)
                .HasColumnName("nome_social");

            builder.Property(c => c.Sexo)
                .HasColumnName("sexo");

            builder.Property(c => c.DataNascimento)
                .HasColumnName("data_nascimento");

            builder.Property(c => c.Situacao)
                .HasColumnName("situacao")
                .IsRequired();
        }
    }
}
