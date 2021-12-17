using Npgsql;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SME.SERAp.Prova.Dados
{
    public abstract class RepositorioSgpBase
    {
        private readonly ConnectionStringOptions connectionStrings;

        public RepositorioSgpBase(ConnectionStringOptions connectionStrings)
        {
            this.connectionStrings = connectionStrings ?? throw new ArgumentNullException(nameof(connectionStrings));
        }

        protected IDbConnection ObterConexaoSgp()
        {
            var conexao = new NpgsqlConnection(connectionStrings.ApiSgp);
            conexao.Open();
            return conexao;
        }
    }
}