using Dapper;
using SME.SERAp.Prova.Infra;
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
            var query = $@"select top 1 Valor 
							from ResultadoAluno 
							where alu_matricula = @alunoRa
								and AreaConhecimentoID = @areaConhecimentoId
								and (tur_codigo = @codigoAnoTurma or AnoEscolar = @anoTurma)
								and esc_codigo = @codigoEscola
								and Valor is not null
						order by Edicao desc";

            using var conn = new SqlConnection(connectionStringOptions.ProvaSP);
            return await conn.QueryFirstOrDefaultAsync<decimal>(query, new { alunoRa, codigoAnoTurma, anoTurma, codigoEscola, areaConhecimentoId });
        }

        public async Task<decimal> ObterMediaProficienciaEscolaAluno(string alunoRa, long areaConhecimentoId)
        {
            var query = $@"declare @anoEscolar int, @edicao int, 
							@soma_esc decimal, @qtdeAlunosEsc bigint,
							@escCodigo varchar(10)

							select 
							   top 1 @anoEscolar = AnoEscolar, @edicao = Edicao,
								   @escCodigo = esc_codigo
							  from ResultadoAluno 
							 where alu_matricula = @alunoRa
							   and AreaConhecimentoID = @areaConhecimentoId
							   and Valor is not null
							order by Edicao desc

							;with soma_esc as (
							select esc_codigo,
								   SUM(ISNULL(Valor,0)) soma
							  from ResultadoAluno 
							 where Edicao = @edicao
							   and AreaConhecimentoID = @areaConhecimentoId
							   and AnoEscolar = @anoEscolar
							   and esc_codigo = @escCodigo
							group by esc_codigo)
							,qtde_esc as (
							select esc_codigo,
								   count(alu_matricula) qtde
							  from ResultadoAluno 
							 where Edicao = @edicao
							   and AreaConhecimentoID = @areaConhecimentoId
							   and AnoEscolar = @anoEscolar
							   and esc_codigo = @escCodigo
							   and Valor is not null
							group by esc_codigo)

								select (s.soma / q.qtde) media
								  from soma_esc s
							inner join qtde_esc q on s.esc_codigo = q.esc_codigo
								 where s.soma is not null
								   and q.qtde is not null";

            using var conn = new SqlConnection(connectionStringOptions.ProvaSP);
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
								select re.uad_sigla dre,
										SUM(re.TotalAlunos) qtde_alunos,
										SUM(re.Valor) total_profici
									from ResultadoEscola re
							inner join edicao_dre ed
									on re.uad_sigla = ed.uad_sigla
									and re.Edicao = ed.Edicao
									and re.AnoEscolar = ed.AnoEscolar
									and re.AreaConhecimentoID = ed.AreaConhecimentoID
								group by re.uad_sigla)
								select (total_profici / qtde_alunos) media_dre
									from total_dre";

			using var conn = new SqlConnection(connectionStringOptions.ProvaSP);
			return await conn.QueryFirstOrDefaultAsync<decimal>(query, new { dreSigla, anoEscolar, areaConhecimentoId });
		}
	}
}
