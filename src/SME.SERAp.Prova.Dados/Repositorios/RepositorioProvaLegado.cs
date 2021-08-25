using Dapper;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioProvaLegado : RepositorioSerapLegadoBase, IRepositorioProvaLegado
    {
        public RepositorioProvaLegado(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<IEnumerable<long>> ObterProvasIdsParaSeremSincronizadasIds(DateTime ultimaAtualizacao)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"
                select
	                t.id
                from
	                Booklet b WITH (NOLOCK)
                inner join Test t WITH (NOLOCK) on
	                t.Id = b.Test_Id
                where
	                t.ShowOnSerapEstudantes = 1
                    and t.UpdateDate > @ultimaAtualizacao
                order by
	                t.ApplicationStartDate desc";

                return await conn.QueryAsync<long>(query, new { ultimaAtualizacao });
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<ProvaLegadoDetalhesIdDto> ObterDetalhesPorId(long id)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"
              select DISTINCT  
	            t.Id,
	            t.Description as descricao,
	            t.ApplicationStartDate as Inicio,
	            t.ApplicationEndDate as Fim,
	            t.NumberItem as TotalItens,
	            t.UpdateDate as UltimaAtualizacao,
	            tt.tcp_ordem AS Ano
            FROM
	            Test t 
	            INNER JOIN TestCurriculumGrade tcg ON
	            t.Id = tcg.Test_Id	
            INNER JOIN SGP_ACA_TipoCurriculoPeriodo tt ON
	            tcg.TypeCurriculumGradeId = tt.tcp_id	
            INNER JOIN TestTypeCourse ttc ON
	            ttc.TestType_Id = t.TestType_Id
            INNER JOIN SGP_TUR_TurmaTipoCurriculoPeriodo ttcp ON
	            ttcp.crp_ordem = tt.tcp_ordem
	            AND tt.tme_id = ttcp.tme_id
	            AND tt.tne_id = ttcp.tne_id
            where
	            t.id = @id";


                var lookup = new Dictionary<long, ProvaLegadoDetalhesIdDto>();

                await conn.QueryAsync<ProvaLegadoDetalhesIdDto, AnoDto, ProvaLegadoDetalhesIdDto>(query,
                    (provaLegadoDetalhesIdDtoQuery, anoDto) =>
                    {
                        ProvaLegadoDetalhesIdDto provaLegadoDetalhesIdDto;
                        if (!lookup.TryGetValue(provaLegadoDetalhesIdDtoQuery.Id, out provaLegadoDetalhesIdDto))
                        {
                            provaLegadoDetalhesIdDto = provaLegadoDetalhesIdDtoQuery;
                            lookup.Add(provaLegadoDetalhesIdDtoQuery.Id, provaLegadoDetalhesIdDto);
                        }

                        provaLegadoDetalhesIdDto.AddAno(anoDto.Ano);

                        return provaLegadoDetalhesIdDto;
                    }, param: new { id }, splitOn: "ano");

                return lookup.Values.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public Task<IEnumerable<AlternativasProvaIdDto>> ObterAlternativasPorIdDaProva(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @" SELECT
                            T.id as ProvaId,
                            I.Id AS ItemId,
                         (DENSE_RANK() OVER(ORDER BY CASE WHEN (t.KnowledgeAreaBlock = 1) THEN ISNULL(Bka.[Order], 0) END,
                        bi.[Order]) - 1) AS OrdemProva,
                        A.Id as Id, 
                        A.[Order] as OrdemAlternativa,
                        A.Numeration as Alternativa, 
                        A.Description as Descrição 
                    FROM Item I WITH (NOLOCK)
                    INNER JOIN BlockItem BI WITH (NOLOCK) ON BI.Item_Id = I.Id
                    INNER JOIN Block B WITH (NOLOCK) ON B.Id = BI.Block_Id
                    INNER JOIN Alternative A (NOLOCK) ON A.Item_Id = I.Id
                    INNER JOIN Test T WITH (NOLOCK) ON T.Id = B.[Test_Id]
                    LEFT JOIN BlockKnowledgeArea Bka WITH (NOLOCK) ON Bka.KnowledgeArea_Id = I.KnowledgeArea_Id AND B.Id = Bka.Block_Id
                    WHERE T.Id = @provaId ";

                return conn.QueryAsync<AlternativasProvaIdDto>(query, new { provaId });
            }
            catch (Exception)
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