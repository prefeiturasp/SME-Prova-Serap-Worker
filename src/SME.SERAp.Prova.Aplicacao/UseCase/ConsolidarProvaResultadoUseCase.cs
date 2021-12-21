using MediatR;
using Sentry;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static SME.SERAp.Prova.Infra.Utils.Paginacao;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ConsolidarProvaResultadoUseCase : IConsolidarProvaResultadoUseCase
    {
        private readonly IMediator mediator;

        public ConsolidarProvaResultadoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var extracao = mensagemRabbit.ObterObjetoMensagem<ProvaExtracaoDto>();
                var exportacaoResultado = await mediator.Send(new ObterExportacaoResultadoStatusPorIdQuery(extracao.ExtracaoResultadoId));

                var checarProvaExiste = await mediator.Send(new VerificaProvaExistePorSerapIdQuery(extracao.ProvaSerapId));

                if (!checarProvaExiste)
                    throw new NegocioException("A prova informada não foi encontrada no serap estudantes");

                await AtualizarStatusExportacao(exportacaoResultado, ExportacaoResultadoStatus.Processando);

                var dres = await mediator.Send(new ObterDresSerapQuery());
                foreach(Dre dre in dres)
                {
                    var ues = await mediator.Send(new ObterUesSerapPorDreCodigoQuery(dre.CodigoDre));
                    var paginas = Paginar(ues.ToList());                    
                    foreach (List<Ue> pagina in paginas)
                    {
                        var ueIds = pagina.Select(ue => ue.CodigoUe).ToArray();
                        await mediator.Send(new ConsolidarProvaRespostaPorFiltroCommand(extracao.ProvaSerapId, dre.CodigoDre, ueIds));
                    }
                }

                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ExtrairResultadosProva, extracao));
            }                
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return false;
            }
            return true;
        }

        private async Task AtualizarStatusExportacao(ExportacaoResultado exportacaoResultado, ExportacaoResultadoStatus status)
        {
            try
            {
                exportacaoResultado.AtualizarStatus(status);
                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado));
            }
            catch (Exception)
            {
                exportacaoResultado.AtualizarStatus(ExportacaoResultadoStatus.Erro);
                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado));
            }
            
        }

        private List<List<Ue>> Paginar(List<Ue> ues)
        {
            var paginacao = new ListaPaginada<Ue>(ues.ToList(), 10);
            return paginacao.ObterTodasAsPaginas();
        }

    }
}