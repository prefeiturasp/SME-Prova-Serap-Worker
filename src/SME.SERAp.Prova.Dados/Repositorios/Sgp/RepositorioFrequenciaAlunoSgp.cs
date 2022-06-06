using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioFrequenciaAlunoSgp : RepositorioSgpBase, IRepositorioFrequenciaAlunoSgp
    {
        public RepositorioFrequenciaAlunoSgp(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<FrequenciaAluno> ObterFrequenciaAlunoPorRAEData(long alunoRa, DateTime data)
        {
            using var conn = ObterConexaoSgp();
            try
            {
                var query = @"select valor from registro_frequencia_aluno rfa where rfa.codigo_aluno = @alunoRa and criado_em::date = @data";

                var parametros = new
                {
                    alunoRa = alunoRa.ToString(),data
                };

                return await conn.QueryFirstOrDefaultAsync<FrequenciaAluno>(query, parametros);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
