using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioElasticTurma : RepositorioElasticBase<DocumentoElasticTurmaDto>, IRepositorioElasticTurma
    {
        public RepositorioElasticTurma(IServicoTelemetria servicoTelemetria, ElasticsearchClient elasticClient) : base(
            servicoTelemetria, elasticClient)
        {
        }

        public async Task<IEnumerable<AlunoMatriculaTurmaDreDto>> ObterTurmasAlunoHistoricoPorAlunosRa(long[] alunosRa)
        {
            Func<QueryDescriptor<AlunoMatriculaTurmaDreDto>, Query> query = q =>
                q.Bool(b => b
                    .Must(
                        m => m.Terms(t => t
                            .Field(f => f.CodigoAluno)
                            .Terms(new TermsQueryField(alunosRa.Select(ra => FieldValue.Long(ra)).ToList()))
                        ),
                        m => m.Term(t => t
                            .Field(f => f.AnoLetivo)
                            .Value(DateTime.Now.Year)
                        )
                    )
                );

            var alunosMatriculaTurmaDre = await ObterListaAsync(
                IndicesElastic.INDICE_ALUNO_MATRICULA_TURMA_DRE,
                query,
                "Obter turmas alunos histórico por alunos RA",
                new { alunosRa }
            );

            return alunosMatriculaTurmaDre
                .OrderBy(c => c.AnoLetivo)
                .ThenBy(c => c.DataMatricula)
                .GroupBy(c => new { c.CodigoMatricula, c.CodigoAluno, c.CodigoTurma, c.AnoLetivo })
                .Select(c => c.FirstOrDefault());
        }
    }
}