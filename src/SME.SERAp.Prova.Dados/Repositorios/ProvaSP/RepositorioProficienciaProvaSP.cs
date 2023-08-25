using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioProficienciaProvaSP : IRepositorioProficienciaProvaSP
    {
        private readonly ConnectionStringOptions connectionStringOptions;

        public RepositorioProficienciaProvaSP(ConnectionStringOptions connectionStringOptions)
        {
            this.connectionStringOptions = connectionStringOptions ?? throw new ArgumentNullException(nameof(connectionStringOptions));
        }        

        public async Task<decimal> ObterProficienciaAluno(string alunoRa, string codigoAnoTurma, string anoTurma, string codigoEscola, long areaConhecimentoId)
        {
            var query = @"select top 1 Valor 
							from ResultadoAluno 
							where alu_matricula = @alunoRa
								and AreaConhecimentoID = @areaConhecimentoId
								and (tur_codigo = @codigoAnoTurma or AnoEscolar = @anoTurma)
								and esc_codigo = @codigoEscola
								and Valor is not null
								and Valor > 0
						order by Edicao desc";

            await using var conn = new SqlConnection(connectionStringOptions.ProvaSP);
            return await conn.QueryFirstOrDefaultAsync<decimal>(query, new { alunoRa, codigoAnoTurma, anoTurma, codigoEscola, areaConhecimentoId });
        }

        public async Task<decimal> ObterMediaProficienciaEscolaAluno(string alunoRa, long areaConhecimentoId)
        {
	        const string query = @"with edicao_aluno as (
										select top 1 AnoEscolar, Edicao, esc_codigo, AreaConhecimentoID
										from ResultadoAluno
										where alu_matricula = @alunoRa
										and AreaConhecimentoID = @areaConhecimentoId
										and Valor is not null
										order by Edicao desc
									), total_aluno as (
										select re.Valor as media
										from ResultadoEscola re
										inner join edicao_aluno ea on ea.Edicao = re.Edicao
	  										and ea.AreaConhecimentoID = re.AreaConhecimentoID
	  										and ea.AnoEscolar = re.AnoEscolar
	  										and ea.esc_codigo = re.esc_codigo
										where re.Valor is not null
									)
									select COALESCE(media, 0) as media
									from total_aluno";

	        await using var conn = new SqlConnection(connectionStringOptions.ProvaSP);
            return await conn.QueryFirstOrDefaultAsync<decimal>(query, new { alunoRa, areaConhecimentoId });
        }

		public async Task<decimal> ObterMediaProficienciaDre(string dreSigla, string anoEscolar, long areaConhecimentoId)
		{
			var query = $@";with edicao_dre as (
							select top 1 rd.uad_sigla, rd.Edicao, rd.AnoEscolar, rd.AreaConhecimentoID
							from ResultadoDre rd
							where rd.uad_sigla = @dreSigla
							and rd.AnoEscolar = @anoEscolar
							and rd.AreaConhecimentoID = @areaConhecimentoId
							order by rd.Edicao desc)
							,total_dre as (
								select rd.Valor as media
									from ResultadoDre rd
									inner join edicao_dre ed on rd.uad_sigla = ed.uad_sigla
									and rd.Edicao = ed.Edicao
									and rd.AnoEscolar = ed.AnoEscolar
									and rd.AreaConhecimentoID = ed.AreaConhecimentoID)
							select COALESCE(media, 0) as media_dre
							from total_dre";

			await using var conn = new SqlConnection(connectionStringOptions.ProvaSP);
			return await conn.QueryFirstOrDefaultAsync<decimal>(query, new { dreSigla, anoEscolar, areaConhecimentoId });
		}
	}
}
