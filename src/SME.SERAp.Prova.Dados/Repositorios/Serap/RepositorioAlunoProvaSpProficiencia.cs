using Dapper;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio.Entidades;
using SME.SERAp.Prova.Infra.Dtos;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados.Repositorios.Serap
{
    public class RepositorioAlunoProvaSpProficiencia : RepositorioBase<AlunoProvaSpProficiencia>, IRepositorioAlunoProvaSpProficiencia
    {
        public RepositorioAlunoProvaSpProficiencia(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<ResultadoAlunoProvaSpDto> ObterResultadoAlunoProvaSp(int edicao, int areaDoConhecimento, string alunoMatricula)
        {
            using var conn = ObterConexaoProvaSp();
            try
            {
                var query = @"select
	                            ra.Edicao,
	                            ra.AreaConhecimentoID,
	                            ra.AnoEscolar,
	                            ra.alu_matricula as AlunoMatricula,
                                ra.NivelProficienciaID as NivelProficiencia,
	                            ra.Valor
                            from
	                            ResultadoAluno ra
                            where
	                            ra.Edicao = @edicao and 
	                            ra.AreaConhecimentoID = @areaDoConhecimento and
	                            ra.alu_matricula = @alunoMatricula";

                return await conn.QueryFirstOrDefaultAsync<ResultadoAlunoProvaSpDto>(query, new { edicao, areaDoConhecimento, alunoMatricula });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<AlunoProvaSpProficiencia> ObterAlunoProvaSpProficiencia(int anoLetivo, long disciplinaId, long alunoRa)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select
	                            apsp.id,
	                            apsp.aluno_ra as alunoRa,
	                            apsp.ano_escolar as anoEscolar,
	                            apsp.ano_letivo  as anoLetivo,
	                            apsp.disciplina_id as disciplinaId,
	                            apsp.proficiencia,
                                apsp.nivel_proficiencia as nivelProficiencia,
	                            apsp.data_atualizacao as dataAtualizacao
                            from
	                            aluno_prova_sp_proficiencia apsp
                            where
	                            apsp.aluno_ra = @alunoRa
	                            and apsp.ano_letivo = @anoLetivo
	                            and apsp.disciplina_id = @disciplinaId";

                return await conn.QueryFirstOrDefaultAsync<AlunoProvaSpProficiencia>(query, new { anoLetivo, disciplinaId, alunoRa });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<int> ExcluirAlunoProvaSpProficiencia(int anoLetivo, long disciplinaId, long alunoRa)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"delete
                            from
	                            aluno_prova_sp_proficiencia apsp
                            where
	                            apsp.aluno_ra = @alunoRa
	                            and apsp.ano_letivo = @anoLetivo
	                            and apsp.disciplina_id = @disciplinaId";

                return await conn.ExecuteAsync(query, new { anoLetivo, disciplinaId, alunoRa });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
