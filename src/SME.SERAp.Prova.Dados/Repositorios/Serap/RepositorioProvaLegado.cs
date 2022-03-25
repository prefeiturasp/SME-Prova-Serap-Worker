﻿using Dapper;
using SME.SERAp.Prova.Dominio;
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
                    
	                t.id,
					t.UpdateDate,
					tp.UpdateDate
                from
	                test t
					left join TestPermission tp
					on tp.Test_Id = t.Id
                     AND tp.gru_id = 'BD6D9CE6-9456-E711-9541-782BCB3D218E'
				
                where
	                t.ShowOnSerapEstudantes = 1
                    and (t.UpdateDate >  @ultimaAtualizacao or 
					      tp.UpdateDate >  @ultimaAtualizacao )
                    and t.State = 1
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
              SELECT DISTINCT  
	            t.Id,
	            t.Description as descricao,
                t.DownloadStartDate as InicioDownload,
	            t.ApplicationStartDate as Inicio,
	            t.ApplicationEndDate as Fim,
	            case 
	            	when t.NumberBlock > 0 then t.NumberItemsBlock else t.NumberItem
	            end TotalItens,
	            t.NumberBlock as TotalCadernos,
	            t.UpdateDate as UltimaAtualizacao,
                ttime.Segundos AS TempoExecucao,
                t.Password as Senha,
                d.Description as Disciplina,
                t.Bib as PossuiBIB,
	            tne.tne_id as Modalidade,
	            tne.tne_nome as ModalidadeNome,
                mt.Id ModeloProva,
                case when tp.TestHide  is null then 0 else tp.TestHide end OcultarProva,
	              case 
	            	when tt.tcp_id = 61 then 'S' else  CAST(tt.tcp_ordem as  VARCHAR)
	            end Ano,	           
                case when tp.TestHide  is null then 0 else tp.TestHide end OcultarProva
            FROM
	            Test t 
	            INNER JOIN TestCurriculumGrade tcg ON
	            t.Id = tcg.Test_Id	
            INNER JOIN SGP_ACA_TipoCurriculoPeriodo tt ON
	            tcg.TypeCurriculumGradeId = tt.tcp_id
            INNER JOIN TESTTIME ttime on
                t.TestTime_Id = ttime.id
            INNER JOIN TestTypeCourse ttc ON
	            ttc.TestType_Id = t.TestType_Id
            LEFT JOIN Discipline d ON
	            t.Discipline_Id = d.Id 
	        INNER JOIN TestType on
	        	t.TestType_Id = TestType.id
	       	INNER JOIN SGP_ACA_TipoNivelEnsino tne ON 
	       		TestType.TypeLevelEducationId = tne.tne_id
            INNER JOIN SGP_TUR_TurmaTipoCurriculoPeriodo ttcp ON
	            ttcp.crp_ordem = tt.tcp_ordem
	            AND tt.tme_id = ttcp.tme_id
            INNER JOIN modeltest mt on TestType.modeltest_id = mt.id
            LEFT JOIN TestPermission tp on tp.Test_Id = t.Id 
			  AND tp.gru_id = 'BD6D9CE6-9456-E711-9541-782BCB3D218E'
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

        public async Task<ProvaLegadoDetalhesIdDto> ObterProvaPorId(long id)
        {
            using var conn = ObterConexao();
            try
            {
              var query = @"
              SELECT DISTINCT  
	            t.Id,
	            t.Description as descricao,
                t.DownloadStartDate as InicioDownload,
	            t.ApplicationStartDate as Inicio,
	            t.ApplicationEndDate as Fim,
	            case 
	            	when t.NumberBlock > 0 then t.NumberItemsBlock else t.NumberItem
	            end TotalItens,
	            t.NumberBlock as TotalCadernos,
	            t.UpdateDate as UltimaAtualizacao,
                ttime.Segundos AS TempoExecucao,
                t.Password as Senha,
                d.Description as Disciplina,
                t.Bib as PossuiBIB,
	            tne.tne_id as Modalidade,
	            tne.tne_nome as ModalidadeNome,
                mt.Id ModeloProva,	             	           
                case when tp.TestHide  is null then 0 else tp.TestHide end OcultarProva,
				convert(bit, t.AllAdhered) as AderirTodos,
                convert(bit, t.Multidiscipline) as Multidisciplinar,
                t.TestType_Id as TipoProva,
                case when tp.TestTai  is null then 0 else tp.TestTai end FormatoTai
            FROM
	            Test t 
	            INNER JOIN TestCurriculumGrade tcg ON
	            t.Id = tcg.Test_Id	
            INNER JOIN SGP_ACA_TipoCurriculoPeriodo tt ON
	            tcg.TypeCurriculumGradeId = tt.tcp_id
            INNER JOIN TESTTIME ttime on
                t.TestTime_Id = ttime.id
            INNER JOIN TestTypeCourse ttc ON
	            ttc.TestType_Id = t.TestType_Id
            LEFT JOIN Discipline d ON
	            t.Discipline_Id = d.Id 
	        INNER JOIN TestType on
	        	t.TestType_Id = TestType.id
	       	INNER JOIN SGP_ACA_TipoNivelEnsino tne ON 
	       		TestType.TypeLevelEducationId = tne.tne_id
            INNER JOIN SGP_TUR_TurmaTipoCurriculoPeriodo ttcp ON
	            ttcp.crp_ordem = tt.tcp_ordem
	            AND tt.tme_id = ttcp.tme_id
            INNER JOIN modeltest mt on TestType.modeltest_id = mt.id
            LEFT JOIN TestPermission tp on tp.Test_Id = t.Id 
			  AND tp.gru_id = 'BD6D9CE6-9456-E711-9541-782BCB3D218E'
            where
	            t.id = @id";

                return await conn.QueryFirstOrDefaultAsync<ProvaLegadoDetalhesIdDto>(query, new { id });
                
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

        public async Task<IEnumerable<QuestoesPorProvaIdDto>> ObterQuestoesPorProvaId(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @" SELECT distinct I.id as QuestaoId, B.Description as Caderno,
                                    (DENSE_RANK() OVER(ORDER BY CASE WHEN (t.KnowledgeAreaBlock = 1) THEN ISNULL(Bka.[Order], 0) END, bi.[Order]) - 1) AS Ordem,
                                    I.[Statement] as Enunciado ,bt.Description  as TextoBase, T.Id as ProvaLegadoId,
                                    case 
	            	                    when IT.QuantityAlternative > 0 then 1 else 2
	                                end TipoItem,
                                    IT.QuantityAlternative as QuantidadeAlternativas
                                    FROM Item I WITH (NOLOCK)
                                    INNER JOIN BlockItem BI WITH (NOLOCK) ON BI.Item_Id = I.Id
                                    INNER JOIN ItemType IT  WITH (NOLOCK) ON I.ItemType_Id = IT.Id  
                                    INNER JOIN Block B WITH (NOLOCK) ON B.Id = BI.Block_Id            
                                    INNER JOIN Test T WITH (NOLOCK) ON T.Id = B.[Test_Id] 
                                    INNER JOIN BaseText bt  on bt.Id = I.BaseText_Id       
                                     LEFT JOIN BlockKnowledgeArea Bka WITH (NOLOCK) ON Bka.KnowledgeArea_Id = I.KnowledgeArea_Id AND B.Id = Bka.Block_Id
                                WHERE T.Id = @provaId  and T.ShowOnSerapEstudantes  = 1 and BI.State = 1;";

                return await conn.QueryAsync<QuestoesPorProvaIdDto>(query, new { provaId });
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
                                    I.[Statement] as Questao ,bt.Description  as Enunciado, T.Id as ProvaLegadoId,
                                    case 
	            	                    when IT.QuantityAlternative > 0 then 1 else 2
	                                end TipoItem,
                                    IT.QuantityAlternative as QuantidadeAlternativas
                                    FROM Item I WITH (NOLOCK)
                                    INNER JOIN BlockItem BI WITH (NOLOCK) ON BI.Item_Id = I.Id
                                    INNER JOIN ItemType IT  WITH (NOLOCK) ON I.ItemType_Id = IT.Id  
                                    INNER JOIN Block B WITH (NOLOCK) ON B.Id = BI.Block_Id            
                                    INNER JOIN Test T WITH (NOLOCK) ON T.Id = B.[Test_Id] 
                                    INNER JOIN BaseText bt  on bt.Id = I.BaseText_Id       
                                     LEFT JOIN BlockKnowledgeArea Bka WITH (NOLOCK) ON Bka.KnowledgeArea_Id = I.KnowledgeArea_Id AND B.Id = Bka.Block_Id
                                WHERE T.Id = @provaId  and T.ShowOnSerapEstudantes  = 1 and  I.id = @questaoId and BI.State = 1;";

                return await conn.QueryFirstOrDefaultAsync<QuestoesPorProvaIdDto>(query, new { provaId , questaoId});
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<Arquivo>> ObterAudiosPorQuestaoId(long questaoId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @" select f.[Path] Caminho, f.Id legadoId
                                from Item i WITH (NOLOCK)
                                    inner join ItemAudio ia WITH (NOLOCK) on i.Id = ia.Item_id and i.[State] = ia.[State]
                                    inner join [File] f WITH (NOLOCK) on f.Id = ia.[File_Id] and f.[State] = ia.[State]
                                where i.[State] = 1
                                    and i.Id = @questaoId;";

                return await conn.QueryAsync<Arquivo>(query, new { questaoId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<QuestaoVideoDto>> ObterVideosPorQuestaoId(long questaoId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @" SELECT  
                                    F.Id AS VideoId, 
                                    F.[Path] CaminhoVideo,  
                                    isnull(FT.Id,0) AS ThumbnailVideoId, 
                                    FT.[Path] AS CaminhoThumbnailVideo, 
                                    isnull(FC.Id, 0) AS VideoConvertidoId,
                                    FC.[Path] AS CaminhoVideoConvertido
                                FROM ItemFile IFI WITH (NOLOCK)
                                    INNER JOIN [File] F WITH (NOLOCK) ON F.Id = IFI.File_Id
                                    INNER JOIN [File] FT WITH (NOLOCK) ON FT.Id = IFI.Thumbnail_Id
                                    LEFT JOIN [File] FC (NOLOCK) ON IFI.ConvertedFile_Id = FC.Id
                                WHERE IFI.State = 1 
                                AND IFI.Item_Id = @questaoId;";

                return await conn.QueryAsync<QuestaoVideoDto>(query, new { questaoId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<ContextoProvaLegadoDto>> ObterContextosProvaPorProvaId(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select Id, 
                                Title as Titulo, Text as Texto, ImagePath as ImagemCaminho, ImagePosition as ImagemPosicao from TestContext
                            where Test_id = @provaId and State = 1;";

                return await conn.QueryAsync<ContextoProvaLegadoDto>(query, new { provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<ProvaFormatoTaiItem> ObterFormatoTaiItemPorId(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select top 1 niat.Value 
                              from NumberItemTestTai nit
                              left join NumberItemsAplicationTai niat on niat.Id = nit.ItemAplicationTaiId
                              where nit.TestId = @provaId;";

                return await conn.QueryFirstOrDefaultAsync<ProvaFormatoTaiItem>(query, new { provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}