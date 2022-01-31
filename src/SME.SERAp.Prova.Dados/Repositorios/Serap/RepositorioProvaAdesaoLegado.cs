using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioProvaAdesaoLegado : RepositorioSerapLegadoBase, IRepositorioProvaAdesaoLegado
    {
        public RepositorioProvaAdesaoLegado(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
            
        }

        public async Task<IEnumerable<ProvaAdesaoEntityDto>> ObterAdesaoPorProvaId(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"
                                select 
										t.id ProvaId,
										esc.esc_codigo UeCodigo,
										a.alu_matricula AlunoRa,
										tur.tur_id TurmaId,
										tt.ttn_id TipoTurno, 
										tt.ttn_nome tipo_turno_nome, 
										tcg.tcp_ordem AnoTurma, 
										tme.tme_id TipoTurma, 
										tme.tme_nome tipo_turma_nome, 
										ne.tne_id Modalidade, 
										ne.tne_nome modalidade_nome,
										getdate() CriadoEm,
										getdate() AtualizadoEm
									from Test t
									inner join Adherence alu on alu.[State] = 1 and t.id = alu.test_id 
									and alu.TypeEntity = 3 and alu.TypeSelection = 1
									inner join SGP_ACA_Aluno a on a.alu_id = alu.EntityId
									inner join SGP_TUR_Turma tur on tur.tur_id = alu.ParentId
									inner join SGP_ESC_Escola esc on esc.esc_id = tur.esc_id
									INNER JOIN SGP_TUR_TurmaTipoCurriculoPeriodo ttcp (nolock)
									ON tur.tur_id = ttcp.tur_id AND
										ttcp.ttcr_situacao = 1
									INNER JOIN SGP_TUR_TurmaCurriculo tc (nolock)
									ON ttcp.tur_id = tc.tur_id AND
										ttcp.crp_ordem = tc.crp_id AND
										tc.tcr_situacao = 1
									INNER JOIN sgp_aca_tipocurriculoperiodo tcg (nolock)
									ON tcg.tcp_id = tc.tcp_id AND tc.tcr_situacao = 1
									INNER JOIN sgp_aca_curso cur (nolock)
									ON cur.cur_id = tc.cur_id AND
										ttcp.tme_id = cur.tme_id
									INNER JOIN sgp_aca_curriculoperiodo crp (nolock)
									ON crp.cur_id = cur.cur_id AND
										crp.crp_ordem = tcg.tcp_ordem AND
										crp.tcp_id = tcg.tcp_id
									inner join SGP_ACA_TipoTurno tt on tt.ttn_id = tur.ttn_id
									inner join SGP_ACA_TipoModalidadeEnsino tme on tme.tme_id = ttcp.tme_id
									inner join SGP_ACA_TipoNivelEnsino ne on ne.tne_id = tcg.tne_id
									where t.AllAdhered = 0
										and t.id = @provaId";

                return await conn.QueryAsync<ProvaAdesaoEntityDto>(query, new { provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

		public async Task<int> ObterAderirPorProvaId(long provaId)
		{
			using var conn = ObterConexao();
			try
			{
				var query = @"select AllAdhered from Test where Id = @provaId";

				return await conn.QueryFirstOrDefaultAsync<int>(query, new { provaId });
			}
			finally
			{
				conn.Close();
				conn.Dispose();
			}
		}

	}
}
