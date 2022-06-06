using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioUe : RepositorioBase<Ue>, IRepositorioUe
    {
        public RepositorioUe(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }
        public async Task<IEnumerable<Ue>> ObterUesSgpPorDreCodigo(string dreCodigo)
        {
            using var conn = ObterConexaoSgp();
            try
            {
                var query = @"select ue.* 
                                from ue
                               inner join dre on dre.id = ue.dre_id 
                               where dre.dre_id = @dreCodigo ";

                return await conn.QueryAsync<Ue>(query, new { dreCodigo });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        public async Task<IEnumerable<Ue>> ObterUesSerapPorDreCodigoAsync(string dreCodigo)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select ue.* 
                                from ue
                               inner join dre on dre.id = ue.dre_id 
                               where dre.dre_id = @dreCodigo ";

                return await conn.QueryAsync<Ue>(query, new { dreCodigo });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<Ue> ObterUePorCodigo(string ueCodigo)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select * from ue where ue_id = @ueCodigo ";

                return await conn.QueryFirstOrDefaultAsync<Ue>(query, new { ueCodigo });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<Ue>> ObterUesSerapPorProvaSerapEDreCodigoAsync(long provaSerap, string dreCodigo)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select distinct
                                    ue.ue_id CodigoUe, 
                                    ue.data_atualizacao DataAtualizacao, 
                                    ue.dre_id DreId, 
                                    ue.nome Nome, 
                                    ue.tipo_escola TipoEscola
                                from prova p
                                inner join prova_ano pa 
                                    on p.id = pa.prova_id
                                inner join turma t 
                                    on t.ano_letivo = EXTRACT(YEAR FROM p.inicio)
                                    and (
     		   									(p.modalidade not in(3,4) and p.modalidade = t.modalidade_codigo and t.ano = pa.ano)
	     									 or (t.ano = pa.ano and t.modalidade_codigo = pa.modalidade and t.etapa_eja = pa.etapa_eja)
     	 								)
                                inner join ue 
                                    on ue.id = t.ue_id
                                inner join dre on dre.id = ue.dre_id 
                                where p.prova_legado_id = @provaSerap
                                    and dre.dre_id = @dreCodigo
                                order by ue.ue_id;";

                return await conn.QueryAsync<Ue>(query, new { provaSerap, dreCodigo });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
