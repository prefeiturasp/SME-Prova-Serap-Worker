using Dapper;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioUsuarioCoreSso : IRepositorioUsuarioCoreSso
    {
        private readonly ConnectionStringOptions connectionStringOptions;

        public RepositorioUsuarioCoreSso(ConnectionStringOptions connectionStringOptions)
        {
            this.connectionStringOptions = connectionStringOptions ?? throw new ArgumentNullException(nameof(connectionStringOptions));
        }

        public async Task<IEnumerable<UsuarioCoreSsoDto>> ObterUsuariosPorGrupoCoreSso(Guid grupo)
        {
            var query = @"select distinct u.usu_id IdCoreSso, u.usu_login [Login], p.pes_nome Nome
                                from SYS_Grupo g
                            inner join SYS_UsuarioGrupo ug
                                on g.gru_id = ug.gru_id and ug.usg_situacao = 1 and g.gru_situacao = 1
                            inner join SYS_Usuario u
                                on u.usu_id = ug.usu_id and u.usu_situacao = 1
                            left join PES_Pessoa p
                                on u.pes_id = p.pes_id
                            where g.gru_id = @grupo
                            and g.sis_id = 204";

            await using var conn = new SqlConnection(connectionStringOptions.CoreSSO);
            return await conn.QueryAsync<UsuarioCoreSsoDto>(query, new { grupo });
        }
    }
}
