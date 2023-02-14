using CsvHelper;
using CsvHelper.Configuration;
using SME.SERAp.Prova.Dominio;
using System;
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
            IgnoreBlankLines = true,
            ShouldSkipRecord = (records) =>
            {
                var linha = records.Row.Parser.RawRecord.Replace(Environment.NewLine, string.Empty);
                linha = linha.Trim().Replace("\r", string.Empty);
                linha = linha.Trim().Replace("\n", string.Empty);
                var arrayLinha = records.Row.Parser.Record;
                return string.IsNullOrEmpty(linha) || arrayLinha == null || arrayLinha?.Length == 0 || (arrayLinha?.Length > 0 && string.IsNullOrEmpty(arrayLinha?[0]));
            }
        };

        public static CsvReader ObterReaderArquivoResultadosPsp(PathOptions pathOptions, string nomeArquivo)
        {
            string path = $"{pathOptions.PathArquivos}/{"ResultadoPsp"}/{nomeArquivo}";
            var reader = new StreamReader(path, encoding: Encoding.UTF8);
            return new CsvReader(reader, config);
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
