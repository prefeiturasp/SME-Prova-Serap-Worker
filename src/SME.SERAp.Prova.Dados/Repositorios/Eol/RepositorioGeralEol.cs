using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
	public class RepositorioGeralEol : IRepositorioGeralEol
	{

		private readonly ConnectionStringOptions connectionStringOptions;

		public RepositorioGeralEol(ConnectionStringOptions connectionStringOptions)
		{
			this.connectionStringOptions = connectionStringOptions ?? throw new ArgumentNullException(nameof(connectionStringOptions));
		}

		public async Task<IEnumerable<string>> ObterUeDreAtribuidasEolAsync(string codigoRf, int[] tiposEscola)
		{
			var query = @"
				select coalesce(CdUnidadeEducacaoSobre, CdUnidadeEducacaoBase) as Codigo
				from (
					select sev.cd_registro_funcional         AS Rf,
						cba.cd_cargo                      AS CargoBase,
						ue_base.cd_unidade_educacao       as CdUnidadeEducacaoBase,
						ue_base.tp_unidade_educacao       as TipoUnidadeBase, 
						css.cd_cargo                      AS CargoSobre,
						IIF(ue_sobre.tp_unidade_educacao = 3,
							iif(ue_sobre.cd_unidade_administrativa_portal = '110000',
								ue_sobre.cd_unidade_administrativa_portal, ue_sobre.cd_unidade_administrativa_referencia),
							ue_sobre.cd_unidade_educacao) as CdUnidadeEducacaoSobre,
						ue_sobre.tp_unidade_educacao      as TipoUnidadeSobre, 
						coalesce(esc_fat.tp_escola, ue_fat.tp_unidade_educacao) as TipoEscolaFuncaoAtividade,
						coalesce(esc_fat.cd_escola, ue_fat.cd_unidade_educacao) as CdUnidadeEducacaoFuncaoAtividade,
						fat.cd_tipo_funcao                as CdTipoFuncaoAtividade
					from v_servidor_cotic sev with (nolock) 
							-- Cargo Base
							inner join v_cargo_base_cotic cba with (nolock) on sev.cd_servidor = cba.cd_servidor
							left join lotacao_servidor ls with (nolock)
									on cba.cd_cargo_base_servidor = ls.cd_cargo_base_servidor and ls.dt_fim is null 
						-- Cargo Sobreposto
							left join cargo_sobreposto_servidor css with (nolock)
									on cba.cd_cargo_base_servidor = css.cd_cargo_base_servidor AND
										(css.dt_fim_cargo_sobreposto IS NULL OR
										css.dt_fim_cargo_sobreposto > Getdate()) 
						-- Funcao Atividade
							left join funcao_atividade_cargo_servidor fat with (nolock)
									on fat.cd_cargo_base_servidor = cba.cd_cargo_base_servidor
										and (fat.dt_cancelamento is null or dt_fim_funcao_atividade > getdate())
										and (fat.dt_fim_funcao_atividade is null or dt_fim_funcao_atividade > getdate()) 
						--Unidades
							left join v_cadastro_unidade_educacao ue_base with (nolock)
									on (ls.cd_unidade_educacao = ue_base.cd_unidade_educacao)
							left join escola esc_base with (nolock)
									on esc_base.cd_escola = ls.cd_unidade_educacao 
							left join v_cadastro_unidade_educacao ue_sobre with (nolock)
									on (css.cd_unidade_local_servico = ue_sobre.cd_unidade_educacao)
							left join escola esc_sobre with (nolock)
									on esc_sobre.cd_escola = ue_sobre.cd_unidade_educacao 
							left join v_cadastro_unidade_educacao ue_fat with (nolock)
							on (ue_fat.cd_unidade_educacao = 
								case ue_fat.tp_unidade_educacao 
									when 3 then CONCAT(SUBSTRING(fat.cd_unidade_local_servico, 1, 5), '0') 
									else fat.cd_unidade_local_servico 
								end)
							left join escola esc_fat with (nolock) on esc_fat.cd_escola = fat.cd_unidade_local_servico 
					where sev.cd_registro_funcional = @codigoRf
					and cba.dt_fim_nomeacao is null
					and cba.dt_cancelamento is null
					 and (css.cd_cargo is not null or (css.cd_cargo_base_servidor is not null and esc_sobre.tp_escola in @tiposEscola
						or (esc_base.tp_escola in @tiposEscola)
						or (ue_base.tp_unidade_educacao in @tiposEscola)))
					) serv";

			using var conn = new SqlConnection(connectionStringOptions.Eol);
			return await conn.QueryAsync<string>(query, new { codigoRf, tiposEscola });
		}

		public async Task<IEnumerable<TurmaAtribuicaoEolDto>> ObterTurmaAtribuicaoEol(int anoInicial, string codigoRf, int[] tiposEscola, string turmaCodigo, int? anoLetivo)
		{
			var query = @"
						select atb.an_atribuicao as AnoLetivo,
								dre.cd_unidade_educacao as DreCodigo,
								atb.cd_unidade_educacao as UeCodigo,
								coalesce(stg.cd_turma_escola, tegp.cd_turma_escola) TurmaCodigo,
								min(atb.dt_atribuicao_aula) as DataAtribuicao,
								max(atb.dt_disponibilizacao_aulas) as DataDisponibilizacaoAula
						from atribuicao_aula (nolock) atb

						-- escolas
						inner join escola (nolock) esc on atb.cd_unidade_educacao = esc.cd_escola
						inner join v_cadastro_unidade_educacao (nolock) vue on vue.cd_unidade_educacao = esc.cd_escola
						inner join (select v_ua.cd_unidade_educacao, v_ua.nm_unidade_educacao, v_ua.nm_exibicao_unidade 
									from unidade_administrativa (nolock) ua
									inner join v_cadastro_unidade_educacao (nolock) v_ua on v_ua.cd_unidade_educacao = ua.cd_unidade_administrativa
									where tp_unidade_administrativa = 24) dre on dre.cd_unidade_educacao = vue.cd_unidade_administrativa_referencia

						--Servidor
						inner join v_cargo_base_cotic (nolock) cbs on atb.cd_cargo_base_servidor = cbs.cd_cargo_base_servidor
						inner join v_servidor_cotic (nolock) vsc on cbs.cd_servidor = vsc.cd_servidor

						-- SerieGrade
						left join serie_turma_grade (nolock) stg on atb.cd_serie_grade = stg.cd_serie_grade
						left join turma_escola (nolock) tur_reg on stg.cd_turma_escola = tur_reg.cd_turma_escola and tur_reg.an_letivo = atb.an_atribuicao and tur_reg.cd_tipo_turma <> 4

						-- ProgramaGrade
						left join turma_escola_grade_programa (nolock) tegp on tegp.cd_turma_escola_grade_programa = atb.cd_turma_escola_grade_programa
						left join turma_escola (nolock) tur_pro on tegp.cd_turma_escola = tur_pro.cd_turma_escola and tur_pro.an_letivo = atb.an_atribuicao and tur_reg.cd_tipo_turma <> 4

						where atb.dt_cancelamento is null
						  and cbs.dt_cancelamento is null 
						  and cbs.dt_fim_nomeacao is null
						  and atb.an_atribuicao >= @anoInicial
						  and vsc.cd_registro_funcional = @codigoRf
						  and esc.tp_escola in @tiposEscola
						  and (tur_reg.cd_turma_escola is not null or tur_pro.cd_turma_escola is not null)";

			if (!string.IsNullOrEmpty(turmaCodigo) && anoLetivo.HasValue)
			{
				query += @" and atb.an_atribuicao = @anoLetivo 
							and (stg.cd_turma_escola = @turmaCodigo or tegp.cd_turma_escola = @turmaCodigo) ";

			}

			query += @"	group by atb.an_atribuicao,
								dre.cd_unidade_educacao,
								atb.cd_unidade_educacao,
								coalesce(stg.cd_turma_escola, tegp.cd_turma_escola)";

			using var conn = new SqlConnection(connectionStringOptions.Eol);
			return await conn.QueryAsync<TurmaAtribuicaoEolDto>(query, new { anoInicial, codigoRf, tiposEscola, turmaCodigo, anoLetivo = anoLetivo.GetValueOrDefault() });
		}
	}
}
