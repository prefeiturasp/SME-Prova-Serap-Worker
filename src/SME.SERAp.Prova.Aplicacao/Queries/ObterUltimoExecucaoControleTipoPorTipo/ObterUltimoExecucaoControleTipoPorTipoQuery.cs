using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUltimoExecucaoControleTipoPorTipoQuery : IRequest<ExecucaoControle>
    {
        public ObterUltimoExecucaoControleTipoPorTipoQuery(ExecucaoControleTipo tipo)
        {
            Tipo = tipo;
        }

        public ExecucaoControleTipo Tipo { get; set; }
    }
}
