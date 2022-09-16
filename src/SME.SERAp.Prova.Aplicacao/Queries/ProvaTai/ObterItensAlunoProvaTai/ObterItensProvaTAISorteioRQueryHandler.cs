using MediatR;
using SME.SERAp.Prova.Infra.Dtos;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterItensProvaTAISorteioRQueryHandler : IRequestHandler<ObterItensProvaTAISorteioRQuery, ItensProvaTAISorteioDto>
    {
        public async Task<ItensProvaTAISorteioDto> Handle(ObterItensProvaTAISorteioRQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("https://dev-serap-estudante.sme.prefeitura.sp.gov.br");
                client.DefaultRequestHeaders.Accept.Clear();

                var parametros = new StringBuilder();
                parametros.AppendFormat("ESTUDANTE={0}", request.AlunoId);
                parametros.AppendFormat("&proficiencia={0}", request.ProeficienciaAluno.ToString(CultureInfo.InvariantCulture));

                parametros.AppendFormat("&idItem={0}", string.Join("%2C", request.Itens.Select(t => t.ItemId).Take(10)));

                parametros.AppendFormat("&parA={0}", string.Join("%2C", request.Itens.Select(t => t.Discriminacao.ToString(CultureInfo.InvariantCulture)).Take(10)));
                parametros.AppendFormat("&parB={0}", string.Join("%2C", request.Itens.Select(t => t.ProporcaoAcertos.ToString(CultureInfo.InvariantCulture)).Take(10)));
                parametros.AppendFormat("&parC={0}", string.Join("%2C", request.Itens.Select(t => t.AcertoCasual.ToString(CultureInfo.InvariantCulture)).Take(10)));

                parametros.AppendFormat("&n.Ij=5", request.QuantidadeItensDaAmostra);

                var response = await client.GetAsync($"BI.j3?{parametros}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    result = result.Replace("\"NA\"", "0").Replace(request.AlunoId.ToString(), "").Replace("_", "");

                    return JsonSerializer.Deserialize<ItensProvaTAISorteioDto[]>(result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true}).FirstOrDefault();
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