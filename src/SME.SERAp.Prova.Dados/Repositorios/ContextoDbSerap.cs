using Microsoft.EntityFrameworkCore;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public class ContextoDbSerap : DbContext
    {
        public ContextoDbSerap(DbContextOptions<ContextoDbSerap> options)
         : base(options)
        { }

        public DbSet<Dre> Dres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new DreEntityMap());
            modelBuilder.ApplyConfiguration(new UeEntityMap());
            modelBuilder.ApplyConfiguration(new TurmaEntityMap());
            modelBuilder.ApplyConfiguration(new AlunoEntityMap());
            modelBuilder.ApplyConfiguration(new ResultadoProvaConsolidadoEntityMap());
            modelBuilder.ApplyConfiguration(new ProvaAdesaoEntityMap());
            modelBuilder.ApplyConfiguration(new ProvaGrupoPermissaoEntityMap());
        }
    }
}
