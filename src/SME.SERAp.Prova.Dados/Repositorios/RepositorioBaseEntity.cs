using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.Bulk;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private static void MapearEnumsAutomaticamente(NpgsqlDataSourceBuilder builder)
        {
            var mapEnumMethod = typeof(NpgsqlDataSourceBuilder)
                .GetMethods()
                .First(m => m.Name == nameof(NpgsqlDataSourceBuilder.MapEnum)
                            && m.GetParameters().Length == 2
                            && m.GetParameters()[0].ParameterType == typeof(string)
                            && m.GetParameters()[1].ParameterType == typeof(INpgsqlNameTranslator));

            var assemblies = new[]
            {
                typeof(EntidadeBase).Assembly,   // Dominio
            };

            var enumTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsEnum)
                .Distinct();

            foreach (var enumType in enumTypes)
            {
                mapEnumMethod!
                    .MakeGenericMethod(enumType)
                    .Invoke(builder, [null, null]);
            }
        }

        private ContextoDbSerap CriarContexto()
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionStrings.ApiSerap);
            MapearEnumsAutomaticamente(dataSourceBuilder);
            var dataSource = dataSourceBuilder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<ContextoDbSerap>();
            optionsBuilder.UseNpgsql(dataSource);

            return new ContextoDbSerap(optionsBuilder.Options);
        }

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
