using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioAbrangencia : RepositorioBase<Abrangencia>, IRepositorioAbrangencia
    {
        public RepositorioAbrangencia(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions) { }

        public async Task<Abrangencia> ObterPorObjetoAbrangencia(Abrangencia abrangencia)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append(@"select a.id, a.usuario_id, a.grupo_id, a.dre_id, a.ue_id, a.turma_id
                                        from abrangencia a
                                        where a.usuario_id = @UsuarioId
                                            and a.grupo_id = @GrupoId ");

                query.AppendLine(abrangencia.DreId is null ? "and a.dre_id is null" : "and a.dre_id = @DreId ");
                query.AppendLine(abrangencia.UeId is null ? "and a.ue_id is null" : "and a.ue_id = @UeId ");
                query.AppendLine(abrangencia.TurmaId is null ? "and a.turma_id is null" : "and a.turma_id = @TurmaId ");
                query.AppendLine(abrangencia.Inicio is null ? "and a.inicio is null" : "and a.inicio = @Inicio ");
                query.AppendLine(abrangencia.Fim is null ? "and a.fim is null" : "and a.fim = @Fim ");

                return await conn.QueryFirstOrDefaultAsync<Abrangencia>(query.ToString(), new { abrangencia.UsuarioId, abrangencia.GrupoId, abrangencia.DreId, abrangencia.UeId, abrangencia.TurmaId, abrangencia.Inicio, abrangencia.Fim });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<Abrangencia>> ObterPorGrupoId(long grupoId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append(@"select a.id, a.usuario_id, a.grupo_id, a.dre_id, a.ue_id, a.turma_id
                                        from abrangencia a
                                        where a.grupo_id = @grupoId");

                return await conn.QueryAsync<Abrangencia>(query.ToString(), new { grupoId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<bool> ExcluirPorId(long id)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append(@"delete from abrangencia where id = @id");

                await conn.QueryAsync<long>(query.ToString(), new { id });
                return true;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<Abrangencia> ObterPorUsuarioGrupoDreUeTurmaAsync(long usuarioId, long grupoId, long dreId, long ueId, long turmaId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select a.id, a.usuario_id, a.grupo_id, a.dre_id, a.ue_id, a.turma_id, a.inicio, a.fim
                                        from abrangencia a
                                        where a.usuario_id = @usuarioId
                                          and a.grupo_id = @grupoId
                                          and a.dre_id = @dreId
                                          and a.ue_id = @ueId
                                          and a.turma_id = @turmaId";

                return await conn.QueryFirstOrDefaultAsync<Abrangencia>(query.ToString(), new { usuarioId, grupoId, dreId, ueId, turmaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
