using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioDownloadProvaAluno : RepositorioBase<DownloadProvaAluno>, IRepositorioDownloadProvaAluno
    {
        public RepositorioDownloadProvaAluno(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<bool> ExcluirDownloadProvaAluno(Guid[] codigos, DateTime? dataAlteracao)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"update downloads_prova_aluno set situacao = 3, alterado_em = @dataAlteracao  where codigo = any(@codigos)";
                return await (conn.ExecuteAsync(query, new { codigos, dataAlteracao })) > 0;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
