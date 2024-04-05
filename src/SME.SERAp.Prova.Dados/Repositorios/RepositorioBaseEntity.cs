using Microsoft.EntityFrameworkCore;
using Npgsql.Bulk;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public abstract class RepositorioBaseEntity<T> where T : EntidadeBase
    {
        private readonly ConnectionStringOptions connectionStrings;

        public RepositorioBaseEntity(ConnectionStringOptions connectionStrings)
        {
            this.connectionStrings = connectionStrings ?? throw new ArgumentNullException(nameof(connectionStrings));
        }

        public async Task InserirVariosAsync(IEnumerable<T> entidades)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ContextoDbSerap>();
            optionsBuilder.UseNpgsql(connectionStrings.ApiSerap);

            using ContextoDbSerap dbContext = new ContextoDbSerap(optionsBuilder.Options);

            var uploader = new NpgsqlBulkUploader(dbContext);

            await uploader.InsertAsync(entidades);
        }

        public async Task AlterarVariosAsync(IEnumerable<T> entidades)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ContextoDbSerap>();
            optionsBuilder.UseNpgsql(connectionStrings.ApiSerap);

            using ContextoDbSerap dbContext = new ContextoDbSerap(optionsBuilder.Options);

            var uploader = new NpgsqlBulkUploader(dbContext);

            await uploader.UpdateAsync(entidades);
        }
    }
}
