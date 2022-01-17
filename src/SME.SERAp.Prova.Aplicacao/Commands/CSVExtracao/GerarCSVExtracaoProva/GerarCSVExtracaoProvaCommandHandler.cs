using CsvHelper;
using CsvHelper.Configuration;
using MediatR;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class GerarCSVExtracaoProvaCommandHandler : IRequestHandler<GerarCSVExtracaoProvaCommand, bool>
    {
        public GerarCSVExtracaoProvaCommandHandler(){}

        public async Task<bool> Handle(GerarCSVExtracaoProvaCommand request, CancellationToken cancellationToken)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine,
            };

            var quantidadeQuestoes = request.QuantidadeQuestoes;

            try
            {
                using (var writer = new StreamWriter(request.NomeArquivo))
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
                    csv.WriteField("tempo_total_prova");
                    csv.WriteField("aluno_frequencia");
                    csv.WriteField("data_inicio");
                    csv.WriteField("data_fim");

                    for (var c = 1; c <= quantidadeQuestoes; c++)
                        csv.WriteField($"questao_{c}");                    
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Gerar arquivo extração prova -- Erro: {ex.Message}");
            }
            return await Task.FromResult(true);
        }
    }
}
