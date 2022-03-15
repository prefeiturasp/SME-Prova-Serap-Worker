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
        public EscreverDadosCSVExtracaoProvaCommandHandler() { }

        public async Task<bool> Handle(EscreverDadosCSVExtracaoProvaCommand request, CancellationToken cancellationToken)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine,
                HasHeaderRecord = false,
            };

            try
            {

                using (var stream = TentarAbrirArquivo(request.NomeArquivo))
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, config))
                {                    
                    var agrupamento = request.Resultado.OrderBy(r => r.QuestaoOrdem).GroupBy(a => a.AlunoCodigoEol);

                    foreach (var registro in agrupamento)
                    {
                        csv.NextRecord();
                        var valor = registro.FirstOrDefault();
                        valor.CalcularTempoTotalProva();
                        csv.WriteField(valor.ProvaSerapId);
                        csv.WriteField(valor.ProvaSerapEstudantesId);
                        csv.WriteField(valor.DreCodigoEol);
                        csv.WriteField(valor.DreSigla);
                        csv.WriteField(valor.DreNome);
                        csv.WriteField(valor.UeCodigoEol);
                        csv.WriteField(valor.UeNome);
                        csv.WriteField(valor.TurmaAnoEscolar);
                        csv.WriteField($"{valor.TurmaAnoEscolar} ano");
                        csv.WriteField(valor.TurmaCodigo);
                        csv.WriteField(valor.TurmaDescricao);
                        csv.WriteField(valor.AlunoCodigoEol);
                        csv.WriteField(valor.AlunoNome);
                        csv.WriteField(valor.AlunoSexo);
                        csv.WriteField(valor.AlunoDataNascimento);
                        csv.WriteField(valor.ProvaComponente);
                        csv.WriteField(valor.ProvaCaderno);
                        csv.WriteField(valor.TempoTotalProva > 0 ? valor.TempoTotalProva.ToString() : "");
                        csv.WriteField(valor.AlunoFrequencia);
                        csv.WriteField(valor.DataInicio != null ? valor.DataInicio.ToString() : "");
                        csv.WriteField(valor.DataFim != null ? valor.DataFim.ToString() : "");

                        foreach (var resultado in registro)
                        {
                            if (resultado?.Resposta != null)
                                csv.WriteField(resultado.Resposta.Replace(")", ""));
                            else
                                csv.WriteField("");
                        }                        
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Escrever dados CSV Extração prova -- Erro: {ex.Message}");
            }

            return await Task.FromResult(true);
        }

        private FileStream TentarAbrirArquivo(string nomeArquivo)
        {
            int tentativas = 1;
            var fileStream = AbrirArquivo(nomeArquivo);
            while (fileStream == null)
            {
                Task.Run(() => Thread.Sleep(5000)).GetAwaiter().GetResult();
                fileStream = AbrirArquivo(nomeArquivo);
                tentativas++;
                if (tentativas == 10 && fileStream == null)
                    throw new ArgumentException($"Não foi possível abrir o arquivo.");
            }
            return fileStream;
        }

        private FileStream AbrirArquivo(string nomeArquivo)
        {
            try
            {
                return File.Open(nomeArquivo, FileMode.Append);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
