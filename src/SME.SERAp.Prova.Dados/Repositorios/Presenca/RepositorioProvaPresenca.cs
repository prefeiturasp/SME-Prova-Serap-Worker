using Dapper;
using Npgsql;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio.Entidades.Presenca;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados.Repositorios.Presenca
{
    public class RepositorioProvaPresenca : RepositorioBase<ProvaPresenca>, IRepositorioProvaPresenca
    {
        private readonly ConnectionStringOptions connectionStrings;

        public RepositorioProvaPresenca(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
            this.connectionStrings = connectionStrings ?? throw new ArgumentNullException(nameof(connectionStrings));
        }

        public async Task<long> IncluirComTurmasAsync(ProvaPresenca provaPresenca, IEnumerable<long> turmasIds)
        {
            const string queryProva = @"
                                        INSERT INTO public.prova_presenca
                                            (nome_prova, ano_prova, descricao_prova, data_inicial_aplicacao,
                                             data_final_aplicacao, data_corte, vincula_aluno_caderno_extra, data_processamento)
                                        VALUES
                                            (@NomeProva, @AnoProva, @DescricaoProva, @DataInicialAplicacao,
                                             @DataFinalAplicacao, @DataCorte, @VinculaAlunoCadernoExtra, @DataProcessamento)
                                        RETURNING id;";

            const string queryTurmas = @"
                                        INSERT INTO public.prova_presenca_turmas (prova_presenca_id, turma_id)
                                        VALUES (@ProvaPresencaId, @TurmaId);";

            var turmas = turmasIds?.Distinct().ToList() ?? new List<long>();

            using var conexao = new NpgsqlConnection(connectionStrings.ApiSerap);
            await conexao.OpenAsync();
            using var transacao = await conexao.BeginTransactionAsync();

            try
            {
                var provaPresencaId = await conexao.ExecuteScalarAsync<long>(queryProva, provaPresenca, transacao);

                if (turmas.Any())
                {
                    var parametros = turmas.Select(turmaId => new { ProvaPresencaId = provaPresencaId, TurmaId = turmaId });
                    await conexao.ExecuteAsync(queryTurmas, parametros, transacao);
                }

                await transacao.CommitAsync();
                return provaPresencaId;
            }
            catch
            {
                await transacao.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> AtualizarComTurmasAsync(ProvaPresenca provaPresenca, IEnumerable<long> turmasIds)
        {
            const string queryProva = @"
                                        UPDATE public.prova_presenca
                                        SET
                                            nome_prova = @NomeProva,
                                            ano_prova = @AnoProva,
                                            descricao_prova = @DescricaoProva,
                                            data_inicial_aplicacao = @DataInicialAplicacao,
                                            data_final_aplicacao = @DataFinalAplicacao,
                                            data_corte = @DataCorte,
                                            vincula_aluno_caderno_extra = @VinculaAlunoCadernoExtra
                                        WHERE id = @Id;";

            const string queryDeletarTurmas = @"
                                                DELETE FROM public.prova_presenca_turmas
                                                WHERE prova_presenca_id = @ProvaPresencaId;";

            const string queryInserirTurmas = @"
                                                INSERT INTO public.prova_presenca_turmas (prova_presenca_id, turma_id)
                                                VALUES (@ProvaPresencaId, @TurmaId);";

            var turmas = turmasIds?.Distinct().ToList();

            using var conexao = new NpgsqlConnection(connectionStrings.ApiSerap);
            await conexao.OpenAsync();
            using var transacao = await conexao.BeginTransactionAsync();

            try
            {
                var linhasAfetadas = await conexao.ExecuteAsync(queryProva, provaPresenca, transacao);

                if (linhasAfetadas == 0)
                {
                    await transacao.RollbackAsync();
                    return false;
                }

                if (turmas != null)
                {
                    await conexao.ExecuteAsync(queryDeletarTurmas, new { ProvaPresencaId = provaPresenca.Id }, transacao);

                    if (turmas.Any())
                    {
                        var parametros = turmas.Select(turmaId => new { ProvaPresencaId = provaPresenca.Id, TurmaId = turmaId });
                        await conexao.ExecuteAsync(queryInserirTurmas, parametros, transacao);
                    }
                }

                await transacao.CommitAsync();
                return true;
            }
            catch
            {
                await transacao.RollbackAsync();
                throw;
            }
        }

        public async Task<ProvaPresenca> ObterPorIdAsync(long id)
        {
            var query = @"
                            SELECT
                                id,
                                nome_prova AS NomeProva,
                                ano_prova AS AnoProva,
                                descricao_prova AS DescricaoProva,
                                data_inicial_aplicacao AS DataInicialAplicacao,
                                data_final_aplicacao AS DataFinalAplicacao,
                                data_corte AS DataCorte,
                                vincula_aluno_caderno_extra AS VinculaAlunoCadernoExtra,
                                data_processamento AS DataProcessamento
                            FROM public.prova_presenca
                            WHERE id = @Id;";

            using var conexao = new NpgsqlConnection(connectionStrings.ApiSerap);
            try
            {
                return await conexao.QueryFirstOrDefaultAsync<ProvaPresenca>(query, new { Id = id });
            }
            finally
            {
                conexao.Close();
                conexao.Dispose();
            }
        }

        public async Task<bool> DeletarAsync(long id)
        {
            const string query = @"DELETE FROM public.prova_presenca WHERE id = @Id;";

            using var conexao = new NpgsqlConnection(connectionStrings.ApiSerap);
            try
            {
                await conexao.OpenAsync();
                var linhasAfetadas = await conexao.ExecuteAsync(query, new { Id = id });
                return linhasAfetadas > 0;
            }
            finally
            {
                conexao.Close();
                conexao.Dispose();
            }
        }
    }
}