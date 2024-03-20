using Npgsql;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Data;

namespace SME.SERAp.Prova.Dados
{
    public abstract class RepositorioSerapBase
    {
        private readonly ConnectionStringOptions connectionStrings;

        public RepositorioSerapBase(ConnectionStringOptions connectionStrings)
        {
            this.connectionStrings = connectionStrings ?? throw new ArgumentNullException(nameof(connectionStrings));
        }

        protected IDbConnection ObterConexaoLeitura()
        {
            var conexao = new NpgsqlConnection(connectionStrings.ApiSerapLeitura);
            conexao.Open();
            return conexao;
        }
        
        protected IDbConnection ObterConexao()
        {
            var conexao = new NpgsqlConnection(connectionStrings.ApiSerap);
            conexao.Open();
            return conexao;
        }        
    }
}