using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio.Entidades;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Threading.Tasks;
using static Slapper.AutoMapper;

namespace SME.SERAp.Prova.Dados.Repositorios.Serap
{
    public class RepositorioArquivoResultadoPsp : RepositorioSerapLegadoBase, IRepositorioArquivoResultadoPsp
    {
        public RepositorioArquivoResultadoPsp(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<ArquivoResultadoPspDto> ObterArquivoResultadoPspPorId(long id)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"
	                     SELECT  Id
                                ,CodigoTipoResultado
                                ,NomeArquivo
                                ,NomeOriginalArquivo
                                ,CreateDate
                                ,UpdateDate
                                ,State
                          FROM  ArquivoResultadoPsp
                         WHERE Id = @id";

                return await conn.QueryFirstOrDefaultAsync<ArquivoResultadoPspDto>(query, new { id });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }


        public async Task AtualizarStatusArquivoResultadoPspPorId(long id, StatusImportacao state)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"UPDATE ArquivoResultadoPsp 
                                    SET State = @state,
                                        UpdateDate = getdate()
                                WHERE Id = @Id";
                await conn.ExecuteAsync(query, new { id, state });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
