using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class GerarCSVExtracaoProvaCommand : IRequest<bool>
    {
        public GerarCSVExtracaoProvaCommand(IEnumerable<ConsolidadoProvaRespostaDto> resultado, string nomeArquivo)
        {
            Resultado = resultado;
            NomeArquivo = nomeArquivo;
        }

        public IEnumerable<ConsolidadoProvaRespostaDto> Resultado { get; set; }
        public string NomeArquivo { get; set; }
    }
}
