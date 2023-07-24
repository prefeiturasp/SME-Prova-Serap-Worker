using CsvHelper;
using CsvHelper.Configuration;
using SME.SERAp.Prova.Dominio;
using System;
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
            var path = $"{pathOptions.PathArquivos}/ResultadoPsp/{nomeArquivo}";
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

                if (decimal.TryParse(valor, NumberStyles.Number, CultureInfo.InvariantCulture, out var decValor))
                    return decValor;
                
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
                if (valor == null) 
                    return true;
                
                var dec_valor = (decimal)valor;
                
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
                if (listaAreaConhecimentoId.All(x => x != (AreaConhecimentoProvaSp)areaConhecimentoId))
                    throw new Exception();
            }
            catch (Exception ex)
            {
                throw new Exception($"AreaConhecimentoId: {areaConhecimentoId}, inválido -- {ex.Message} -- {ex.StackTrace}");
            }
        }

        public static string ObterFilaTratarPorTipoResultadoPsp(TipoResultadoPsp tipoResultado)
        {
            return tipoResultado switch
            {
                TipoResultadoPsp.ResultadoAluno => RotasRabbit.TratarResultadoAlunoPsp,
                TipoResultadoPsp.ResultadoSme => RotasRabbit.TratarResultadoSmePsp,
                TipoResultadoPsp.ResultadoDre => RotasRabbit.TratarResultadoDrePsp,
                TipoResultadoPsp.ResultadoEscola => RotasRabbit.TratarResultadoEscolaPsp,
                TipoResultadoPsp.ResultadoTurma => RotasRabbit.TratarResultadoTurmaPsp,
                TipoResultadoPsp.ResultadoParticipacaoTurma => RotasRabbit.TratarResultadoParticipacaoTurma,
                TipoResultadoPsp.ParticipacaoTurmaAreaConhecimento => RotasRabbit.TratarParticipacaoTurmaAreaConhecimento,
                TipoResultadoPsp.ResultadoParticipacaoUe => RotasRabbit.TratarResultadoParticipacaoUe,
                TipoResultadoPsp.ParticipacaoUeAreaConhecimento => RotasRabbit.TratarParticipacaoUeAreaConhecimento,
                TipoResultadoPsp.ParticipacaoDre => RotasRabbit.TratarResultadoParticipacaoDre,
                TipoResultadoPsp.ParticipacaoDreAreaConhecimento => RotasRabbit.TratarResultadoParticipacaoDreAreaConhecimento,
                TipoResultadoPsp.ParticipacaoSme => RotasRabbit.TratarResultadoParticipacaoSme,
                TipoResultadoPsp.ParticipacaoSmeAreaConhecimento => RotasRabbit.TratarResultadoParticipacaoSmeAreaConhecimento,
                TipoResultadoPsp.ResultadoCicloSme => RotasRabbit.TratarResultadoCicloSme,
                _ => string.Empty
            };
        }

        public static string ObterJsonObjetoResultado(object resultado)
        {
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Serialize(resultado, jsonSerializerOptions);
        }
        
        public static int? ConvertStringPraIntNullPsp(this string valor)
        {
            try
            {
                if (string.IsNullOrEmpty(valor)) 
                    return null;

                if (valor.ToUpper() == "NA") 
                    return null;

                if (int.TryParse(valor, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValor))
                    return intValor;
                
                throw new Exception();
            }
            catch (Exception)
            {
                throw new Exception($"não foi possível converter o valor para inteiro: {valor}");
            }
        }        
    }
}
