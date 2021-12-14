using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioProva : RepositorioBase<Dominio.Prova>, IRepositorioProva
    {
        public RepositorioProva(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }
        
        public async Task<Dominio.Prova> ObterPorIdLegadoAsync(long id)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select * from prova where prova_legado_id = @id";

                return await conn.QueryFirstOrDefaultAsync<Dominio.Prova>(query, new { id });
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<bool> VerificaSeExisteRespostasPorId(long id)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select 1 from prova_aluno pa where prova_id = @id and finalizado_em is not null limit 1";

                return await conn.QueryFirstOrDefaultAsync<bool>(query, new { id });
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<ProvaAlunoDto>> ObterProvasIniciadasPorModalidadeAsync(int modalidade)
        {
            using var conn = ObterConexao();
            try
            {
                int status = (int)ProvaStatus.Iniciado;
                var query = @"select
                                pa.id provaAlunoId,
                                pa.prova_id provaId,
                                pa.criado_em provaIniciadaEm,
                                pa.status,
                                p.modalidade,
                                p.inicio inicioProva,
                                p.fim fimProva,
                                p.tempo_execucao tempoExecucao
                            from 
                                prova p
                                inner join prova_aluno pa on p.id = pa.prova_id
                            where pa.status = @status
                            and p.modalidade = @modalidade
                            and (p.tempo_execucao > 0 or (p.tempo_execucao = 0 and NOW()::timestamp > p.fim))
                                order by p.id,pa.aluno_ra";

                return await conn.QueryAsync<ProvaAlunoDto>(query, new { modalidade, status });
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<bool> FinalizarProvaAsync(ProvaParaAtualizarDto provaParaAtualizar)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"update prova_aluno 
                                     set status = @status, 
                                     finalizado_em = @finalizadoEm
                                where prova_id = @provaId
                                and id = any(@ids)";

                await conn.ExecuteAsync(query, 
                    new { 
                            provaParaAtualizar.ProvaId, 
                            provaParaAtualizar.Status, 
                            provaParaAtualizar.FinalizadoEm,
                            ids = provaParaAtualizar.IdsProvasAlunos
                        });

                return true;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
