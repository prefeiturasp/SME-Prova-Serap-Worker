using MediatR;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTamanhoArquivosQueryHandler : IRequestHandler<ObterTamanhoArquivosQuery, IEnumerable<Arquivo>>
    {
        public async Task<IEnumerable<Arquivo>> Handle(ObterTamanhoArquivosQuery request, CancellationToken cancellationToken)
        {

            var arquivos = request.Arquivos.ToList();

            foreach (var arquivo in arquivos)
            {
                using (var client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(arquivo.Caminho))
                {
                    byte[] fileContents = await response.Content.ReadAsByteArrayAsync();
                    arquivo.TamanhoBytes = fileContents.Length;
                }
            }

            return arquivos;

        }
    }
}
