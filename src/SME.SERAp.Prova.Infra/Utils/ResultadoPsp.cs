using CsvHelper;
using CsvHelper.Configuration;
using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;

namespace SME.SERAp.Prova.Infra
{
    public static class ResultadoPsp
    {
        private static CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ";",
            MissingFieldFound = null,
        };

        public static CsvReader ObterReaderArquivoResultadosPsp(PathOptions pathOptions, string nomeArquivo)
        {
            string path = $"{pathOptions.PathArquivos}/{"ResultadoPsp"}/{nomeArquivo}";
            AjustarArquivo(path);
            var reader = new StreamReader(path, encoding: Encoding.UTF8);
            return new CsvReader(reader, config);
        }

        private static void AjustarArquivo(string path)
        {
            List<string> linhas = new List<string>();
            using var fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            using var sr = new StreamReader(fs, Encoding.UTF8);
            string linhaAtual = String.Empty;
            int cont = 1;
            bool reescreverArquivo = false;

            while (!sr.EndOfStream)
            {
                linhaAtual = sr.ReadLine();
                if (!string.IsNullOrEmpty(linhaAtual.Trim()))
                    linhas.Add(linhaAtual);
                cont++;
                reescreverArquivo = linhas.Count == 2;
                if (linhas.Count > 2) break;
            }

            fs.Close();
            sr.Close();

            if (reescreverArquivo)
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string linha in linhas)
                        sb.AppendLine(linha);
                    sb.AppendLine(linhas[linhas.Count - 1]);
                    sw.Write(sb.ToString());
                }
            }
        }

        public static decimal? ConvertStringPraDecimalNullPsp(this string valor)
        {
            if (string.IsNullOrEmpty(valor)) return null;

            valor = valor.Replace(",", ".").Trim();

            if (valor == "NA") return null;

            if (decimal.TryParse(valor, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal dec_valor))
            {
                return dec_valor;
            }

            throw new ArgumentException($"não foi possível converter o valor para decimal: {valor}");
        }

        public static string ObterFilaTratarPorTipoResultadoPsp(TipoResultadoPsp tipoResultado)
        {
            switch (tipoResultado)
            {
                case TipoResultadoPsp.ResultadoAluno:
                    return RotasRabbit.TratarResultadoAlunoPsp;
                case TipoResultadoPsp.ResultadoSme:
                    return RotasRabbit.TratarResultadoSmePsp;
                case TipoResultadoPsp.ResultadoDre:
                    return RotasRabbit.TratarResultadoDrePsp;
                case TipoResultadoPsp.ResultadoEscola:
                    return RotasRabbit.TratarResultadoEscolaPsp;
                case TipoResultadoPsp.ResultadoTurma:
                    return RotasRabbit.TratarResultadoTurmaPsp;
                default:
                    return string.Empty;
            }
        }

        public static string ObterJsonObjetoResultado(object resultado)
        {
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Serialize(resultado, jsonSerializerOptions);
        }
    }
}
