using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioExportacaoResultado : RepositorioBase<ExportacaoResultado>, IRepositorioExportacaoResultado
    {
        public RepositorioExportacaoResultado(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        //public async Task<ExportacaoResultado> ObterPorIdAsync(long id)
        //{
        //    using var conn = ObterConexaoLeitura();
        //    try
        //    {
        //        var query = @"select id,nome_arquivo,status,criado_em,atualizado_em,prova_serap_id 
        //                        from exportacao_resultado where id = @id;";
        //        return await conn.QueryFirstOrDefaultAsync<ExportacaoResultado>(query, new { id });
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        conn.Dispose();
        //    }
        //}
    }
}
