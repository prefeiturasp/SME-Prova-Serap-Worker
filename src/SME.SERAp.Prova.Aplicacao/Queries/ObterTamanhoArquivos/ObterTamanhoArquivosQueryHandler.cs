using MediatR;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTamanhoArquivosQueryHandler : IRequestHandler<ObterTamanhoArquivosQuery, IEnumerable<Arquivo>>
    {
        public async Task<IEnumerable<Arquivo>> Handle(ObterTamanhoArquivosQuery request, CancellationToken cancellationToken)
        {
            var arquivos = request.Arquivos;

            foreach (var arquivo in arquivos)
            {
                using var client = new HttpClient();

                if (!arquivo.Caminho.Contains("serap"))
                    arquivo.Caminho = "https://serap.sme.prefeitura.sp.gov.br/Files/Arquivo/2021/11/1ad3b83f-cc9b-434d-84bb-999a45c7cf3d.png";

                using var response = await client.GetAsync(arquivo.Caminho, cancellationToken);
                var fileContents = await response.Content.ReadAsByteArrayAsync();
                arquivo.TamanhoBytes = fileContents.Length;
            }

            return arquivos;

        }
    }
}
