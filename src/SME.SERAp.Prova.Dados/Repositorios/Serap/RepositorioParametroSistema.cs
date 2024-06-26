﻿using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioParametroSistema : RepositorioBase<ParametroSistema>, IRepositorioParametroSistema
    {
        public RepositorioParametroSistema(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<ParametroSistema> ObterPorTipoEAno(int tipo, int ano)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select * from parametro_sistema ps 
                                where ps.ano = @ano and ps.tipo = @tipo";

                return await conn.QueryFirstOrDefaultAsync<ParametroSistema>(query, new { tipo, ano });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<ParametroSistema>> ObterTodosParaCacheAsync()
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = "select ps.* from parametro_sistema ps";
                return await conn.QueryAsync<ParametroSistema>(query);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
