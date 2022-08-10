using System;
using System.Threading.Tasks;
using static SME.SERAp.Prova.Infra.ServicoTelemetria;

namespace SME.SERAp.Prova.Infra
{
    public interface IServicoTelemetria
    {
        Task<dynamic> RegistrarComRetornoAsync<T>(Func<Task<object>> acao, string acaoNome, string telemetriaNome, string telemetriaValor);
        dynamic RegistrarComRetorno<T>(Func<object> acao, string acaoNome, string telemetriaNome, string telemetriaValor);
        void Registrar(Action acao, string acaoNome, string telemetriaNome, string telemetriaValor);
        Task RegistrarAsync(Func<Task> acao, string acaoNome, string telemetriaNome, string telemetriaValor);
        ServicoTelemetriaTransacao IniciarTransacao(string rota);
        void FinalizarTransacao(ServicoTelemetriaTransacao servicoTelemetriaTransacao);
        void RegistrarExcecao(ServicoTelemetriaTransacao servicoTelemetriaTransacao, Exception ex);
    }
}
