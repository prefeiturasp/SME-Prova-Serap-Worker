using CsvHelper;
using CsvHelper.Configuration;
using SME.SERAp.Prova.Dominio;
using System;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace SME.SERAp.Prova.Infra
{
    public static class ResultadoPsp
    {
        private static CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ";",
        };

        public static CsvReader ObterReaderArquivoResultadosPsp(PathOptions pathOptions, string nomeArquivo)
        {
            var reader = new StreamReader($"{pathOptions.PathArquivos}/{"ResultadoPsp"}/{nomeArquivo}");
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
