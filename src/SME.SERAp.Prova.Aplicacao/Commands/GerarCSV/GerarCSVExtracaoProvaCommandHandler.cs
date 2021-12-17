using CsvHelper;
using CsvHelper.Configuration;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class GerarCSVExtracaoProvaCommandHandler : IRequestHandler<GerarCSVExtracaoProvaCommand, bool>
    {
        public GerarCSVExtracaoProvaCommandHandler()
        {
        }
        public async Task<bool> Handle(GerarCSVExtracaoProvaCommand request, CancellationToken cancellationToken)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine,
            };

            var caminhoBase = AppDomain.CurrentDomain.BaseDirectory;
            var nomeArquivo = Path.Combine(caminhoBase, "resultados", request.NomeArquivo);
            var quantidadeQuestoes = request.Resultado.FirstOrDefault();
            try
            {
                using (var writer = new StreamWriter(nomeArquivo))
                using (var csv = new CsvWriter(writer, config))
                {
                    csv.WriteField("prova_serap_id");
                    csv.WriteField("prova_serap_estudantes_id");
                    csv.WriteField("dre_codigo_eol");
                    csv.WriteField("dre_sigla");
                    csv.WriteField("dre_nome");
                    csv.WriteField("ue_codigo_eol");
                    csv.WriteField("ue_nome");
                    csv.WriteField("turma_ano_escolar");
                    csv.WriteField("turma_ano_escolar_descricao");
                    csv.WriteField("turma_codigo");
                    csv.WriteField("turma_descricao");
                    csv.WriteField("aluno_codigo_eol");
                    csv.WriteField("aluno_nome");
                    csv.WriteField("aluno_sexo");
                    csv.WriteField("aluno_data_nascimento");
                    csv.WriteField("prova_componente");
                    csv.WriteField("prova_caderno");
                    csv.WriteField("aluno_frequencia");

                    for(var c = 1; c <= quantidadeQuestoes.ProvaQuantidadeQuestoes; c++)
                        csv.WriteField($"questao_{c}");

                    csv.NextRecord();
                    var agrupamento = request.Resultado.OrderBy(r => r.QuestaoOrdem).GroupBy(a => a.AlunoCodigoEol);
                    foreach (var registro in agrupamento)
                    {
                        var valor = registro.FirstOrDefault();
                        csv.WriteField(valor.ProvaSerapId);
                        csv.WriteField(valor.ProvaSerapEstudantesId);
                        csv.WriteField(valor.DreCodigoEol);
                        csv.WriteField(valor.DreSigla);
                        csv.WriteField(valor.DreNome);
                        csv.WriteField(valor.UeCodigoEol);
                        csv.WriteField(valor.UeNome);
                        csv.WriteField(valor.TurmaAnoEscolar);
                        csv.WriteField(valor.TurmaAnoEscolarDescricao);
                        csv.WriteField(valor.TurmaCodigo);
                        csv.WriteField(valor.TurmaDescricao);
                        csv.WriteField(valor.AlunoCodigoEol);
                        csv.WriteField(valor.AlunoNome);
                        csv.WriteField(valor.AlunoSexo);
                        csv.WriteField(valor.AlunoDataNascimento);
                        csv.WriteField(valor.ProvaComponente);
                        csv.WriteField(valor.ProvaCaderno);
                        csv.WriteField(valor.AlunoFrequencia);

                        foreach(var resultado in registro)
                        {
                            csv.WriteField(resultado.Resposta);
                        }
                        csv.NextRecord();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return await Task.FromResult(true);
        }
    }
}
