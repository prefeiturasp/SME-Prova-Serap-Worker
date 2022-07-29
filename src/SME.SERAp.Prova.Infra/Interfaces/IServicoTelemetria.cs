using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Infra
{
    public interface IServicoTelemetria
    {
        bool Apm { get; }

        Task<dynamic> RegistrarComRetornoAsync<T>(Func<Task<object>> acao, string acaoNome, string telemetriaNome, string telemetriaValor);
        dynamic RegistrarComRetorno<T>(Func<object> acao, string acaoNome, string telemetriaNome, string telemetriaValor);
        void Registrar(Action acao, string acaoNome, string telemetriaNome, string telemetriaValor);
        Task RegistrarAsync(Func<Task> acao, string acaoNome, string telemetriaNome, string telemetriaValor);
    }
}
