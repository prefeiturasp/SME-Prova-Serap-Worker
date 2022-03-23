using Dapper;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioGeralCoreSso : IRepositorioGeralCoreSso
    {

        private readonly ConnectionStringOptions connectionStringOptions;

        public RepositorioGeralCoreSso(ConnectionStringOptions connectionStringOptions)
        {
            this.connectionStringOptions = connectionStringOptions ?? throw new ArgumentNullException(nameof(connectionStringOptions));
        }

        public async Task<IEnumerable<string>> ObterUeDreAtribuidasCoreSso(Guid usuarioIdCoreSso, Guid grupoIdCoreSso)
        {
            var query = $@"SELECT distinct
                                    UA.uad_codigo ue_codigo
                            FROM SYS_UsuarioGrupoUA UGUA
                            INNER JOIN SYS_UnidadeAdministrativa UA 
                                ON UGUA.uad_id = UA.uad_id
                            where UGUA.usu_id = @usuarioIdCoreSso
                            and UGUA.gru_id = @grupoIdCoreSso";

            using var conn = new SqlConnection(connectionStringOptions.CoreSSO);
            return await conn.QueryAsync<string>(query, new { usuarioIdCoreSso, grupoIdCoreSso });
        }
    }
}
