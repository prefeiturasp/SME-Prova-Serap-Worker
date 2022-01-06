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
    public class EscreverDadosCSVExtracaoProvaCommandHandler : IRequestHandler<EscreverDadosCSVExtracaoProvaCommand, bool>
    {
        public EscreverDadosCSVExtracaoProvaCommandHandler(){}

        public async Task<bool> Handle(EscreverDadosCSVExtracaoProvaCommand request, CancellationToken cancellationToken)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine,
                HasHeaderRecord = false,
            };

            try
            {
                
                using (var stream = File.Open(request.NomeArquivo, FileMode.Append))
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, config))
                {
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

                        foreach (var resultado in registro)
                        {
                            csv.WriteField(resultado.Resposta);
                        }

                        csv.NextRecord();
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Escrever dados CSV Extração prova -- Erro: {ex.Message}");
            }

            return await Task.FromResult(true);
        }
    }
}
