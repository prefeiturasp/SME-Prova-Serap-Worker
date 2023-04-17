using CsvHelper;
using CsvHelper.Configuration;
using SME.SERAp.Prova.Dominio;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
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
            ShouldSkipRecord = records =>
            {
                var linha = records.Row.Parser.RawRecord.Replace(Environment.NewLine, string.Empty);
                linha = linha.Trim().Replace("\r", string.Empty);
                linha = linha.Trim().Replace("\n", string.Empty);
                linha = linha.Trim().Replace("\0", string.Empty);

                var arrayLinha = records.Row.Parser.Record;
                return string.IsNullOrEmpty(linha) || arrayLinha == null || arrayLinha.Length == 0 ||
                       (arrayLinha.Length > 0 && string.IsNullOrEmpty(arrayLinha[0]));
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
            try
            {
                if (string.IsNullOrEmpty(valor)) return null;

                valor = valor.Replace(",", ".").Trim();

                if (valor.ToUpper() == "NA") return null;

                if (decimal.TryParse(valor, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal dec_valor))
                {
                    return dec_valor;
                }
                throw new Exception();
            }
            catch (Exception)
            {
                throw new Exception($"não foi possível converter o valor para decimal: {valor}");
            }
        }

        public static bool AnoEdicaoValido(string edicao)
        {
            try
            {
                if (string.IsNullOrEmpty(edicao)) return false;
                if (int.TryParse(edicao, NumberStyles.Number, CultureInfo.InvariantCulture, out int ano_edicao))
                {
                    return ano_edicao > 2000 && ano_edicao < 3000;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DecimalNullValido(decimal? valor)
        {
            try
            {
                if (valor == null) return true;
                decimal dec_valor = (decimal)valor;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void ValidarAreaConhecimentoId(int areaConhecimentoId)
        {
            try
            {
                var listaAreaConhecimentoId = Enum.GetValues(typeof(AreaConhecimentoProvaSp)).Cast<AreaConhecimentoProvaSp>().ToList();
                if (!listaAreaConhecimentoId.Any(x => x == (AreaConhecimentoProvaSp)areaConhecimentoId))
                    throw new Exception();
            }
            catch (Exception ex)
            {
                throw new Exception($"AreaConhecimentoId: {areaConhecimentoId}, inválido -- {ex.Message} -- {ex.StackTrace}");
            }
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
                case TipoResultadoPsp.ResultadoParticipacaoTurma:
                    return RotasRabbit.TratarResultadoParticipacaoTurma;
                case TipoResultadoPsp.ParticipacaoTurmaAreaConhecimento:
                    return RotasRabbit.TratarParticipacaoTurmaAreaConhecimento;
                case TipoResultadoPsp.ResultadoParticipacaoUe:
                    return RotasRabbit.TratarResultadoParticipacaoUe;
                case TipoResultadoPsp.ParticipacaoUeAreaConhecimento:
                    return RotasRabbit.TratarParticipacaoUeAreaConhecimento;
                case TipoResultadoPsp.ParticipacaoDre:
                    return RotasRabbit.TratarResultadoParticipacaoDre;

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
