using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public class DreEntityMap : IEntityTypeConfiguration<Dre>
    {
        public void Configure(EntityTypeBuilder<Dre> builder)
        {

            builder.ToTable("dre");
            
            builder.Property(c => c.Id).HasColumnName("id");                       

            builder.Property(c => c.CodigoDre)
                .HasColumnType("varchar")
                .HasColumnName("dre_id")
                .IsRequired();

            builder.Property(c => c.Abreviacao)
                .HasColumnType("varchar")
                .HasColumnName("abreviacao")
                .IsRequired();

            builder.Property(c => c.Nome)
                .HasColumnType("varchar")
                .HasColumnName("nome")
                .IsRequired();

            builder.Property(c => c.DataAtualizacao)
                .HasColumnType("timestamp")
                .HasColumnName("data_atualizacao")
                .IsRequired();

        }
    }
}
