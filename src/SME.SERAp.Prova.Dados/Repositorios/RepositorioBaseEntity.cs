using Microsoft.EntityFrameworkCore;
using Npgsql.Bulk;
using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public abstract class RepositorioBaseEntity<T> where T : EntidadeBase
    {
        private readonly DbContextOptions<ContextoDbSerap> _dbContextOptions;

        public RepositorioBaseEntity(DbContextOptions<ContextoDbSerap> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions ?? throw new ArgumentNullException(nameof(dbContextOptions));
        }

        private ContextoDbSerap CriarContexto() => new(_dbContextOptions);

        public async Task InserirVariosAsync(IEnumerable<T> entidades)
        {
            await using var dbContext = CriarContexto();
            var uploader = new NpgsqlBulkUploader(dbContext);
            await uploader.InsertAsync(entidades);
        }

        public async Task AlterarVariosAsync(IEnumerable<T> entidades)
        {
            await using var dbContext = CriarContexto();
            var uploader = new NpgsqlBulkUploader(dbContext);
            await uploader.UpdateAsync(entidades);
        }
    }
}
