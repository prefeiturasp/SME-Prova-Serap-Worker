using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public class UeEntityMap : IEntityTypeConfiguration<Ue>
    {
        public void Configure(EntityTypeBuilder<Ue> builder)
        {

            builder.ToTable("ue");
            
            builder.Property(c => c.Id).HasColumnName("id");                       

            builder.Property(c => c.CodigoUe)
                .HasColumnName("ue_id")
                .IsRequired();

            builder.Property(c => c.DreId)
                .HasColumnType("varchar")
                .HasColumnName("dre_id")
                .IsRequired();

            builder.Property(c => c.Nome)
                .HasColumnType("varchar")
                .HasColumnName("nome")
                .IsRequired();

            builder.Property(c => c.TipoEscola)
                .HasColumnName("tipo_escola")
                .IsRequired();

            builder.Property(c => c.DataAtualizacao)
                .HasColumnType("timestamp")
                .HasColumnName("data_atualizacao")
                .IsRequired();


            //ToTable("ue");
            //Map(c => c.Id).ToColumn("id").IsKey();

            //Map(c => c.CodigoUe).ToColumn("ue_id");
            //Map(c => c.DataAtualizacao).ToColumn("data_atualizacao");
            //Map(c => c.DreId).ToColumn("dre_id");
            //Map(c => c.TipoEscola).ToColumn("tipo_escola");
            //Map(c => c.Nome).ToColumn("nome");

        }
    }
}
