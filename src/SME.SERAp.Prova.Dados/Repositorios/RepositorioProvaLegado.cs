using Dapper;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioProvaLegado : RepositorioSerapLegadoBase, IRepositorioProvaLegado
    {

        public RepositorioProvaLegado(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<IEnumerable<long>> ObterProvasIdsParaSeremSincronizadasIds(DateTime ultimaAtualizacao)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"
                select
	                t.id
                from
	                Booklet b WITH (NOLOCK)
                inner join Test t WITH (NOLOCK) on
	                t.Id = b.Test_Id
                where
	                t.ShowOnSerapEstudantes = 1
                    and t.UpdateDate > @ultimaAtualizacao
                order by
	                t.ApplicationStartDate desc";

                return await conn.QueryAsync<long>(query, new { ultimaAtualizacao });
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        public async Task<ProvaLegadoDetalhesIdDto> ObterDetalhesPorId(long id)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"
                select
                    t.Id,
	                t.Description as descricao,
	                t.ApplicationStartDate as Inicio,
	                t.ApplicationEndDate as Fim,
	                t.NumberItem as TotalItens,
                    t.UpdateDate as UltimaAtualizacao
                from
	                test t
                where
	                t.id = @id";

                return await conn.QueryFirstOrDefaultAsync<ProvaLegadoDetalhesIdDto>(query, new { id });
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}