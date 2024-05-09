using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioElasticTurma : RepositorioElasticBase<DocumentoElasticTurmaDto>, IRepositorioElasticTurma
    {
        public RepositorioElasticTurma(IServicoTelemetria servicoTelemetria, IElasticClient elasticClient) : base(
            servicoTelemetria, elasticClient)
        {
        }

        public async Task<IEnumerable<AlunoMatriculaTurmaDreDto>> ObterTurmasAlunoHistoricoPorAlunosRa(long[] alunosRa)
        {
            QueryContainer query = new QueryContainerDescriptor<AlunoMatriculaTurmaDreDto>();
            
            query = query && new QueryContainerDescriptor<AlunoMatriculaTurmaDreDto>().Terms(t =>
                        t.Field(f => f.CodigoAluno).Terms(alunosRa));
            query = query && new QueryContainerDescriptor<AlunoMatriculaTurmaDreDto>().Term(t => t.AnoLetivo, DateTime.Now.Year);

            var alunosMatriculaTurmaDre = await ObterListaAsync<AlunoMatriculaTurmaDreDto>(IndicesElastic.INDICE_ALUNO_MATRICULA_TURMA_DRE,
                _ => query, "Obter turmas alunos histórico por alunos RA", new { alunosRa });

            return alunosMatriculaTurmaDre
                .OrderBy(c => c.AnoLetivo).ThenBy(c => c.DataMatricula)
                .GroupBy(c => new { c.CodigoMatricula, c.CodigoAluno, c.CodigoTurma, c.AnoLetivo })
                .Select(c => c.FirstOrDefault());
        }
    }
}