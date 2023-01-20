using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;

namespace SME.SERAp.Prova.Infra
{
    public class ResultadoPsp
    {

        CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ";",
        };

        public CsvReader ObterReaderArquivoResultadosPsp(PathOptions pathOptions, string nomeArquivo)
        {
            var reader = new StreamReader($"{pathOptions.PathArquivos}/{"ResultadoPsp"}/{nomeArquivo}");
            return new CsvReader(reader, config);
        }
    }
}
