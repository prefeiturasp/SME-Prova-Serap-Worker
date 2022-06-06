using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public class TurmaEntityMap : IEntityTypeConfiguration<Turma>
    {
        public void Configure(EntityTypeBuilder<Turma> builder)
        {
            builder.ToTable("turma");

            builder.Property(c => c.Id).HasColumnName("id");

            builder.Property(c => c.TipoTurma)
                .HasColumnName("tipo_turma")
                .IsRequired();

            builder.Property(c => c.Codigo)
                .HasColumnName("codigo")
                .IsRequired();

            builder.Property(c => c.AnoLetivo)
                .HasColumnName("ano_letivo")
                .IsRequired();

            builder.Property(c => c.Ano)
                .HasColumnName("ano")
                .IsRequired();

            builder.Property(c => c.ModalidadeCodigo)
                .HasColumnName("modalidade_codigo")
                .IsRequired();

            builder.Property(c => c.NomeTurma)
                .HasColumnName("nome")
                .IsRequired();

            builder.Property(c => c.TipoTurno)
                .HasColumnName("tipo_turno")
                .IsRequired();

            builder.Property(c => c.UeId)
                .HasColumnName("ue_id")
                .IsRequired();

            builder.Property(c => c.DataAtualizacao)
                .HasColumnType("timestamp")
                .HasColumnName("data_atualizacao")
                .IsRequired();

            builder.Property(c => c.Semestre)
                .HasColumnName("semestre")
                .IsRequired();

            builder.Property(c => c.EtapaEja)
                .HasColumnName("etapa_eja")
                .IsRequired();

            builder.Property(c => c.SerieEnsino)
                .HasColumnName("serie_ensino")
                .IsRequired();
        }
    }
}
