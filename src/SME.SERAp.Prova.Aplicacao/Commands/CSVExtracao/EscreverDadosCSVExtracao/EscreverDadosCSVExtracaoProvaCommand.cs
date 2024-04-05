using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class EscreverDadosCSVExtracaoProvaCommand : IRequest<bool>
    {
        public EscreverDadosCSVExtracaoProvaCommand(IEnumerable<ConsolidadoProvaRespostaDto> resultado, string nomeArquivo)
        {
            Resultado = resultado;
            NomeArquivo = nomeArquivo;
        }

        public IEnumerable<ConsolidadoProvaRespostaDto> Resultado { get; }
        public string NomeArquivo { get; }
    }
}
