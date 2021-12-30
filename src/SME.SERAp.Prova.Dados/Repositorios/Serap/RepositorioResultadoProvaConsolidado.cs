using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioResultadoProvaConsolidado : RepositorioSerapBase, IRepositorioResultadoProvaConsolidado
    {
        public RepositorioResultadoProvaConsolidado(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {
            
        }
        public async Task<IEnumerable<ConsolidadoProvaRespostaDto>> ObterExtracaoProvaResposta(long provaSerapId, string dreCodigoEol)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select 
	                            ProvaSerapId,
                                ProvaSerapEstudantesId,
	                            DreCodigoEol, 
	                            DreSigla,
	                            DreNome,
	                            UeCodigoEol,
	                            UeNome,
	                            TurmaAnoEscolar,
	                            TurmaAnoEscolarDescricao,
	                            TurmaCodigo,
	                            TurmaDescricao,
	                            AlunoCodigoEol,
	                            AlunoNome,
	                            AlunoSexo,
	                            AlunoDataNascimento,
	                            ProvaComponente,
	                            ProvaCaderno,
                                ProvaQuantidadeQuestoes,
	                            AlunoFrequencia,  
	                            QuestaoId, 
	                            QuestaoOrdem,
	                            resposta
                           from f_extracao_prova_resposta(@provaSerapId, @dreCodigoEol);";

                return await conn.QueryAsync<ConsolidadoProvaRespostaDto>(query, new { provaSerapId, dreCodigoEol }, commandTimeout: 1000);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
