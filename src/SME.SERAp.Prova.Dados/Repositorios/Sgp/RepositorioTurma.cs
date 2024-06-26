﻿using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioTurma : RepositorioBase<Turma>, IRepositorioTurma
    {
        public RepositorioTurma(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<IEnumerable<TurmaSgpDto>> ObterturmasSgpPorUeCodigo(string ueCodigo)
        {
            using var conn = ObterConexaoSgp();
            try
            {
                var query = @"select ano, 
                                     ano_letivo as anoLetivo,
                                     turma_id as codigo,
                                     tipo_turma as tipoturma,
                                     modalidade_codigo as modalidadeCodigo,
                                     turma.nome as nomeTurma,
                                     tipo_turno as tipoturno                                      
                                from turma
                               inner join ue on turma.ue_id = ue.id
                               where ue.ue_id = @ueCodigo
                                 and not historica
                                 and tipo_turma = 1
                                 and modalidade_codigo in (3,4,5,6)
                                 and ano_letivo = @anoLetivo ";

                var parametros = new
                {
                    ueCodigo,
                    anoLetivo = DateTime.Now.Year
                };

                return await conn.QueryAsync<TurmaSgpDto>(query, parametros);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        public async Task<IEnumerable<TurmaSgpDto>> ObterTurmasSgpPorDreCodigoAsync(string dreCodigo)
        {
            using var conn = ObterConexaoSgp();
            try
            {
                var query = @"select t.ano, 
                                     t.ano_letivo as anoLetivo,
                                     t.turma_id as Codigo,
                                     t.tipo_turma as tipoturma,
                                     t.modalidade_codigo as modalidadeCodigo,
                                     t.nome as nomeTurma,
                                     t.tipo_turno as tipoturno,
                                     t.semestre as semestre,
                                     t.etapa_eja as etapaEja,
                                     t.serie_ensino as serieEnsino
                                from turma t
                               inner join ue u on t.ue_id = u.id
                               inner join dre d on u.dre_id  = d.id
                               where not t.historica
                                 and t.tipo_turma = 1
                                 and t.modalidade_codigo in (3,4,5,6)
                                 and t.ano_letivo = @anoLetivo 
                                 and d.dre_id = @dreCodigo";

                var parametros = new
                {
                    dreCodigo,
                    anoLetivo = DateTime.Now.Year
                };

                return await conn.QueryAsync<TurmaSgpDto>(query, parametros);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<TurmaSgpDto> ObterTurmaSgpPorCodigoAsync(string codigoTurma)
        {
            using var conn = ObterConexaoSgp();
            try
            {
                var query = @"select t.ano, 
                                     t.ano_letivo as anoLetivo,
                                     t.turma_id as Codigo,
                                     t.tipo_turma as tipoturma,
                                     t.modalidade_codigo as modalidadeCodigo,
                                     t.nome as nomeTurma,
                                     t.tipo_turno as tipoturno,
                                     t.semestre as semestre,
                                     t.etapa_eja as etapaEja,
                                     t.serie_ensino as serieEnsino,
                                     u.ue_id as ueCodigo
                                from turma t
                               inner join ue u on t.ue_id = u.id
                               inner join dre d on u.dre_id  = d.id
                               where t.tipo_turma = 1
                                 and t.modalidade_codigo in (3,4,5,6)
                                 and t.turma_id = @codigoTurma;";

                var parametros = new
                {
                    codigoTurma
                };

                return await conn.QueryFirstOrDefaultAsync<TurmaSgpDto>(query, parametros);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<TurmaSgpDto>> ObterTurmasSgpPorUeCodigoEAnoLetivoAsync(string ueCodigo, int anoLetivo, bool historica)
        {
            using var conn = ObterConexaoSgp();
            try
            {
                var query = new StringBuilder();
                query.AppendLine(@"select t.ano, 
                                     t.ano_letivo as anoLetivo,
                                     t.turma_id as Codigo,
                                     t.tipo_turma as tipoturma,
                                     t.modalidade_codigo as modalidadeCodigo,
                                     t.nome as nomeTurma,
                                     t.tipo_turno as tipoturno,
                                     t.semestre as semestre,
                                     t.etapa_eja as etapaEja,
                                     t.serie_ensino as serieEnsino,
                                     t.ue_id UeId
                                from turma t
                               inner join ue u on t.ue_id = u.id
                               inner join dre d on u.dre_id  = d.id
                               where t.tipo_turma = 1
                                 and t.modalidade_codigo in (3,4,5,6)
                                 and t.ano_letivo = @anoLetivo 
                                 and u.ue_id = @ueCodigo");

                query.AppendLine(historica ? " and t.historica" : " and not t.historica");

                var parametros = new
                {
                    anoLetivo, ueCodigo
                };

                return await conn.QueryAsync<TurmaSgpDto>(query.ToString(), parametros);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        public async Task<Turma> ObterturmaPorCodigo(string uecodigo)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select * 
                                from turma
                               where codigo = @uecodigo ";

                return await conn.QueryFirstOrDefaultAsync<Turma>(query, new { uecodigo });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<Turma>> ObterTurmasPorAnoEAnoLetivo(string ano, int anoLetivo)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select * 
                                from turma
                               where ano = @ano and ano_letivo = @anoLetivo ";

                return await conn.QueryAsync<Turma>(query, new { ano = ano.ToString(), anoLetivo });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<long> InserirOuAtualizarTurmaAsync(TurmaSgpDto turmaSgp)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"WITH upsert AS (
                            UPDATE turma
                            SET
                                ano = @ano, 
		                          ano_letivo = @anoLetivo, 
		                          ue_id = @ueId, 
		                          tipo_turma = @tipoTurma, 
		                          modalidade_codigo = @modalidade,
		                          nome = @nome,
		                          tipo_turno = @tipoTurno,
		                          data_atualizacao = @dataAtualizacao
                            WHERE
                                codigo = @codigo
                            RETURNING *
                        )
                        INSERT INTO turma(ano, ano_letivo, codigo, ue_id, tipo_turma, modalidade_codigo, nome, tipo_turno, data_atualizacao)
                        SELECT
                            @ano,  @anoLetivo, @codigo, @ueId, @tipoTurma,   @modalidade,  @nome, @tipoTurno, @dataAtualizacao
                        WHERE
                            NOT EXISTS (SELECT 1 FROM upsert);
                        select id from turma where codigo = @codigo;";

                return await conn.QueryFirstOrDefaultAsync<long>(query, new { ano = turmaSgp.Ano, 
                    anoLetivo = turmaSgp.AnoLetivo, 
                    codigo = turmaSgp.Codigo,
                    ueId = turmaSgp.UeId,
                    tipoTurma = turmaSgp.TipoTurma,
                    modalidade = turmaSgp.ModalidadeCodigo,
                    nome = turmaSgp.NomeTurma,
                    tipoTurno = turmaSgp.TipoTurno,
                    dataAtualizacao = DateTime.Now
                });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<TurmaSgpDto>> ObterTurmasSerapPorDreCodigoAsync(string dreCodigo)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select t.id,
                                     t.ano, 
                                     t.ano_letivo as anoLetivo,
                                     t.Codigo as Codigo,
                                     t.tipo_turma as tipoturma,
                                     t.modalidade_codigo as modalidadeCodigo,
                                     t.nome as nomeTurma,
                                     t.tipo_turno as tipoturno,
                                     t.ue_id as UeId,
                                     t.semestre as Semestre,
                                     t.etapa_eja as EtapaEja
                                from turma t
                               inner join ue u on t.ue_id = u.id
                               inner join dre d on u.dre_id  = d.id
                               where t.tipo_turma = 1
                                 and t.modalidade_codigo in (3,4,5,6)
                                 and d.dre_id = @dreCodigo";

                var parametros = new
                {
                    dreCodigo
                };

                return await conn.QueryAsync<TurmaSgpDto>(query, parametros);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<TurmaSgpDto>> ObterTurmasSerapPorUeCodigoEAnoLetivoAsync(string ueCodigo, int anoLetivo)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select t.id,
                                     t.ano, 
                                     t.ano_letivo as anoLetivo,
                                     t.Codigo as Codigo,
                                     t.tipo_turma as tipoturma,
                                     t.modalidade_codigo as modalidadeCodigo,
                                     t.nome as nomeTurma,
                                     t.tipo_turno as tipoturno,
                                     t.ue_id as UeId,
                                     t.semestre  
                               from turma t
                               inner join ue u on t.ue_id = u.id
                               inner join dre d on u.dre_id  = d.id
                               where t.tipo_turma = 1
                                 and t.modalidade_codigo in (3,4,5,6)
                                 and t.ano_letivo = @anoLetivo 
                                 and u.ue_id = @ueCodigo";

                var parametros = new
                {
                    anoLetivo, ueCodigo
                };

                return await conn.QueryAsync<TurmaSgpDto>(query, parametros);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<Turma>> ObterTodasPorAnoAsync(int year)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select *                         
                                from turma t                               
                                 where t.ano_letivo = @anoLetivo;";

                var parametros = new
                {
                    anoLetivo = year
                };

                return await conn.QueryAsync<Turma>(query, parametros);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<Turma>> ObterTurmasPorCodigoUeEAnoLetivo(string codigoUe, int anoLetivo)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select t.id, t.ano, t.ano_letivo, t.codigo, t.ue_id, t.tipo_turma 
                                from turma t
                                inner join ue on t.ue_id = ue.id
                                where t.ano_letivo = @anoLetivo
                                and ue.ue_id = @codigoUe;";

                return await conn.QueryAsync<Turma>(query, new { codigoUe, anoLetivo });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<Turma>> ObterTurmasPorCodigoUeEProvaSerap(string codigoUe, long provaSerap)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select t.id, t.ano, t.ano_letivo, t.codigo, t.ue_id, t.tipo_turma 
                                    from prova p
                                    inner join prova_ano pa 
                                        on p.id = pa.prova_id
                                    inner join turma t 
                                        on t.ano_letivo = EXTRACT(YEAR FROM p.inicio)
                                        and (
     		   									(p.modalidade not in(3,4) and p.modalidade = t.modalidade_codigo and t.ano = pa.ano)
	     									 or (t.ano = pa.ano and t.modalidade_codigo = pa.modalidade and t.etapa_eja = pa.etapa_eja)
     	 									)                                        
                                    inner join ue 
                                        on ue.id = t.ue_id
                                    where 
                                        ue.ue_id = @codigoUe
                                        and p.prova_legado_id = @provaSerap;";

                return await conn.QueryAsync<Turma>(query, new { codigoUe, provaSerap });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<Turma>> ObterTurmasPorCodigos(string[] codigos)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select t.id, t.codigo, t.ano
                                from turma t
                                where t.codigo = ANY(@codigos)";

                return await conn.QueryAsync<Turma>(query, new { codigos });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<TurmaAtribuicaoDto> ObterTurmaAtribuicaoPorCodigo(int anoLetivo, string codigo)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select u.dre_id as dreId,
	                            u.id as ueId,
	                            t.id as turmaId
                            from turma t
                            inner join ue u on u.id = t.ue_id 
                            where t.codigo = @codigo and t.ano_letivo = @anoLetivo";

                return await conn.QueryFirstOrDefaultAsync<TurmaAtribuicaoDto>(query, new { anoLetivo, codigo });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<Turma>> ObterTurmasPorCodigoDreEProvaSerap(string codigoDre, long provaSerap)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = @"select t.id,
                                            t.ano,
                                            t.ano_letivo,
                                            t.codigo,
                                            t.ue_id,
                                            t.tipo_turma 
                                        from prova p
                                        inner join prova_ano pa on p.id = pa.prova_id
                                        inner join turma t on t.ano_letivo = EXTRACT(YEAR FROM p.inicio)
                                            and (
     		   									    (p.modalidade not in(3,4) and p.modalidade = t.modalidade_codigo and t.ano = pa.ano)
	     									        or (t.ano = pa.ano and t.modalidade_codigo = pa.modalidade and t.etapa_eja = pa.etapa_eja)
     	 									    )                                        
                                    inner join ue on ue.id = t.ue_id
                                    inner join dre on dre.id = ue.dre_id
                                    where dre.dre_id = @codigoDre
                                    and p.prova_legado_id = @provaSerap;";

                return await conn.QueryAsync<Turma>(query, new { codigoDre, provaSerap });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
