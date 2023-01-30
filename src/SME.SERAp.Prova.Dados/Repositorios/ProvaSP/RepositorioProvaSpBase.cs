using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SME.SERAp.Prova.Dados
{
    public abstract class RepositorioProvaSpBase
    {
        private readonly ConnectionStringOptions connectionStrings;

        public RepositorioProvaSpBase(ConnectionStringOptions connectionStrings)
        {
            this.connectionStrings = connectionStrings ?? throw new ArgumentNullException(nameof(connectionStrings));
        }

        protected IDbConnection ObterConexaoProvaSp()
        {
            var conexao = new SqlConnection(connectionStrings.ProvaSP);
            conexao.Open();
            return conexao;
        }
    }
}
