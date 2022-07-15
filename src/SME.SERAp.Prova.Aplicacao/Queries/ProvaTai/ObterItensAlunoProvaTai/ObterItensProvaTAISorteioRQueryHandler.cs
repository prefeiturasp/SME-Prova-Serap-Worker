using MediatR;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterItensProvaTAISorteioRQueryHandler : IRequestHandler<ObterItensProvaTAISorteioRQuery, string>
    {
        public ObterItensProvaTAISorteioRQueryHandler() { }

        public async Task<string> Handle(ObterItensProvaTAISorteioRQuery request, CancellationToken cancellationToken)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"http://127.0.0.1:8080/BI.j3?ESTUDANTES=123&proficiencia=1&parA=1&parB=2&parC=3&n.Ij=1");
                client.DefaultRequestHeaders.Accept.Clear();

                HttpRequestMessage requestMessage = new HttpRequestMessage(new HttpMethod("POST"), client.BaseAddress.AbsoluteUri);
                requestMessage.Headers.Accept.Clear();
                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                HttpResponseMessage response = client.SendAsync(requestMessage).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                throw new Exception("Não foi possível obter os dados");
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
    }
}