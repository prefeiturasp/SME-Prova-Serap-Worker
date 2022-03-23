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

        public async Task<IEnumerable<string>> ObterUeDreAtribuidasEolAsync(string codigoRf, string tiposEscola)
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
    }
}
