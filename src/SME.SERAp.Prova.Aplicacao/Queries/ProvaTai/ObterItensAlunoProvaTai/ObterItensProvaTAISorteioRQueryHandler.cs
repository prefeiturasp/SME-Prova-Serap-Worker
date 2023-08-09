using MediatR;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterItensProvaTAISorteioRQueryHandler : IRequestHandler<ObterItensProvaTAISorteioRQuery, ItensProvaTAISorteioDto>
    {
        private readonly ApiROptions apiROptions;

        public ObterItensProvaTAISorteioRQueryHandler(ApiROptions apiROptions)
        {
            this.apiROptions = apiROptions ?? throw new ArgumentNullException(nameof(apiROptions));
        }

        public async Task<ItensProvaTAISorteioDto> Handle(ObterItensProvaTAISorteioRQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                
                var textInfo = new CultureInfo("pt-BR").TextInfo;
                var componente = textInfo.ToTitleCase(request.Componente.ToLower());

                var obterItensProvaTaiDto = new ObterItensProvaTaiDto
                {
                    Estudante = request.AlunoId.ToString(),
                    Proficiencia = request.ProeficienciaAluno.ToString(CultureInfo.InvariantCulture),
                    IdItem = string.Join(",", request.Itens.Select(t => t.ItemId)),
                    ParA = string.Join(",", request.Itens.Select(t => t.Discriminacao.ToString(CultureInfo.InvariantCulture))),
                    ParB = string.Join(",", request.Itens.Select(t => t.ProporcaoAcertos.ToString(CultureInfo.InvariantCulture))),
                    ParC = string.Join(",", request.Itens.Select(t => t.AcertoCasual.ToString(CultureInfo.InvariantCulture))),
                    NIj = request.QuantidadeItensDaAmostra.ToString(),
                    Componente = componente
                };

                var json = JsonSerializer.Serialize(obterItensProvaTaiDto);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiROptions.UrlAmostra, stringContent, cancellationToken);

                if (!response.IsSuccessStatusCode) 
                    throw new Exception("Não foi possível obter os dados");
                
                var result = await response.Content.ReadAsStringAsync();
                result = result.Replace("\"NA\"", "0").Replace(request.AlunoId.ToString(), "").Replace("_", "");

                return JsonSerializer.Deserialize<ItensProvaTAISorteioDto[]>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true}).FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
    }
}