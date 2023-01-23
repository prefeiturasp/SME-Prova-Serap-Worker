using CsvHelper;
using CsvHelper.Configuration;
using SME.SERAp.Prova.Dominio;
using System;
using System.Globalization;
using System.IO;

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

        public static decimal? ConverterStringPraDecimal(this string valor)
        {
            if (string.IsNullOrEmpty(valor)) return null;
            decimal dec_valor = 0;
            if (decimal.TryParse(valor.Trim(), out dec_valor))
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
                default:
                    return string.Empty;
            }
        }
    }
}
