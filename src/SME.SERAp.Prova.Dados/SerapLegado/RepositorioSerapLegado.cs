using System;
using System.Data;
using System.Data.SqlClient;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using SME.SERAp.Prova.Infra.Utils;

namespace SME.SERAp.Prova.Dados.SerapLegado
{
    public class RepositorioSerapLegado
    {
        private readonly ConnectionStringOptions _connectionStrings;

        public RepositorioSerapLegado(ConnectionStringOptions connectionStrings)
        {
            _connectionStrings = connectionStrings ?? throw new ArgumentNullException(nameof(connectionStrings));
        }

        protected IDbConnection ObterConexao()
        {
            var conexao = new SqlConnection(_connectionStrings.SerapLegadoConnection);
            conexao.Open();
            return conexao;
        }
    }
}