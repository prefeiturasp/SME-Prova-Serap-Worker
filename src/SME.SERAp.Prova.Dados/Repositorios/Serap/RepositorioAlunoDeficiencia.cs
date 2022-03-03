using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioAlunoDeficiencia : RepositorioBase<AlunoDeficiencia>, IRepositorioAlunoDeficiencia
    {
        public RepositorioAlunoDeficiencia(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<bool> RemoverPorAlunoRa(long alunoRa)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"delete from aluno_deficiencia where aluno_ra = @alunoRa;";

                await conn.QueryAsync(query, new { alunoRa });
                return true;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
