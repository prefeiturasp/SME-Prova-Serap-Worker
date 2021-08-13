using System.Threading.Tasks;
using Dapper;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dados.SerapLegado;
using SME.SERAp.Prova.Dominio.Dtos;
using SME.SERAp.Prova.Infra.EnvironmentVariables;

namespace SME.SERAp.Prova.Dados.Repositorios
{
    public class RepositorioProvaLegado : RepositorioSerapLegado, IRepositorioProvaLegado
    {
        
        public RepositorioProvaLegado(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<ObterIdsProvaLegadoDto> ObterIds()
        {
            var query = @"
            select t.id as Ids from Booklet b WITH (NOLOCK) 
            inner join Test t WITH (NOLOCK)
            on t.Id = b.Test_Id 
            where b.State = 3 
                and t.State = 3 
                and t.ShowOnSerapEstudantes = 0 
            order by t.ApplicationStartDate desc";

            using var conn = ObterConexao();
            return await conn.QueryFirstOrDefaultAsync<ObterIdsProvaLegadoDto>(query);
        }
    }
}