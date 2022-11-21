using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public class ProvaGrupoPermissaoEntityMap : IEntityTypeConfiguration<ProvaGrupoPermissao>
    {
        public void Configure(EntityTypeBuilder<ProvaGrupoPermissao> builder)
        {
            builder.ToTable("prova_grupo_permissao");

            builder.Property(c => c.Id).HasColumnName("id");

            builder.Property(c => c.ProvaId)
                .HasColumnName("prova_id")
                .IsRequired();

            builder.Property(c => c.ProvaLegadoId)
                .HasColumnName("prova_legado_id")
                .IsRequired();

            builder.Property(c => c.GrupoId)
                .HasColumnName("grupo_id")
                .IsRequired();

            builder.Property(c => c.OcultarProva)
               .HasColumnName("ocultar_prova")
               .IsRequired();

            builder.Property(c => c.CriadoEm)
                .HasColumnName("criado_em");

            builder.Property(c => c.AlteradoEm)
                .HasColumnName("alterado_em");
        }
    }
}
