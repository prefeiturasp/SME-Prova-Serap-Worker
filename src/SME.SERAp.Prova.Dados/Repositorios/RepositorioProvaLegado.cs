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
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public  async Task<IEnumerable<long>> ObterAlternativasPorProvaIdEQuestaoId(long questaoId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @" SELECT 
                                    A.Id 
                                FROM  Alternative A (NOLOCK)                             
                                WHERE A.Item_Id = @questaoId;";

                return await conn.QueryAsync<long>(query, new {  questaoId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        
        public  async Task<AlternativasProvaIdDto> ObterDetalheAlternativasPorProvaIdEQuestaoId(long questaoId, long alternativaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"SELECT 
                                    A.Id as AlternativaLegadoId,                                    
                                    A.Numeration as Numeracao,
                                    A.Description as Descricao,
                                    A.[Order] as Ordem
                                FROM  Alternative A (NOLOCK)                             
                                WHERE A.Item_Id = @questaoId and A.id = @alternativaId;";
                

                return await conn.QueryFirstOrDefaultAsync<AlternativasProvaIdDto>(query, new { questaoId, alternativaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<QuestaoLegadoDto>> ObterQuestoesPorProvaId(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @" SELECT I.id, 
                                    (DENSE_RANK() OVER(ORDER BY CASE WHEN (t.KnowledgeAreaBlock = 1) THEN ISNULL(Bka.[Order], 0) END, bi.[Order]) - 1) AS Ordem
                                    FROM Item I WITH (NOLOCK)
                                    INNER JOIN BlockItem BI WITH (NOLOCK) ON BI.Item_Id = I.Id
                                    INNER JOIN Block B WITH (NOLOCK) ON B.Id = BI.Block_Id            
                                    INNER JOIN Test T WITH (NOLOCK) ON T.Id = B.[Test_Id] 
                                    INNER JOIN BaseText bt  on bt.Id = I.BaseText_Id
                                    LEFT JOIN BlockKnowledgeArea Bka WITH (NOLOCK) ON Bka.KnowledgeArea_Id = I.KnowledgeArea_Id AND B.Id = Bka.Block_Id
                                WHERE T.Id = @provaId  and T.ShowOnSerapEstudantes  = 1;";

                return await conn.QueryAsync<QuestaoLegadoDto>(query, new { provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        
        
        public async Task<QuestoesPorProvaIdDto> ObterDetalheQuestoesPorProvaId(long provaId, long questaoId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @" SELECT I.id as QuestaoId,
                                    (DENSE_RANK() OVER(ORDER BY CASE WHEN (t.KnowledgeAreaBlock = 1) THEN ISNULL(Bka.[Order], 0) END, bi.[Order]) - 1) AS Ordem,
                                    I.[Statement] as Questao ,bt.Description  as Enunciado, T.Id as ProvaLegadoId
                                    FROM Item I WITH (NOLOCK)
                                    INNER JOIN BlockItem BI WITH (NOLOCK) ON BI.Item_Id = I.Id
                                    INNER JOIN Block B WITH (NOLOCK) ON B.Id = BI.Block_Id            
                                    INNER JOIN Test T WITH (NOLOCK) ON T.Id = B.[Test_Id] 
                                    INNER JOIN BaseText bt  on bt.Id = I.BaseText_Id       
                                     LEFT JOIN BlockKnowledgeArea Bka WITH (NOLOCK) ON Bka.KnowledgeArea_Id = I.KnowledgeArea_Id AND B.Id = Bka.Block_Id
                                WHERE T.Id = @provaId  and T.ShowOnSerapEstudantes  = 1 and  I.id = @questaoId ;";

                return await conn.QueryFirstOrDefaultAsync<QuestoesPorProvaIdDto>(query, new { provaId , questaoId});
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}