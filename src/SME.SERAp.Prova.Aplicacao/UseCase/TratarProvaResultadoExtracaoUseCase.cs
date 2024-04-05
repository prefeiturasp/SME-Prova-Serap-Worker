using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProvaResultadoExtracaoUseCase : ITratarProvaResultadoExtracaoUseCase
    {
        private readonly IMediator mediator;
        private readonly IServicoLog servicoLog;

        public TratarProvaResultadoExtracaoUseCase(IMediator mediator, IServicoLog servicoLog)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var extracao = mensagemRabbit.ObterObjetoMensagem<TratarProvaResultadoExtracaoDto>();
            if (extracao is null)
                throw new NegocioException("O id da prova serap precisa ser informado");
            
            var exportacaoResultado = await mediator.Send(new ObterExportacaoResultadoStatusQuery(extracao.ExtracaoResultadoId, extracao.ProvaSerapId));
            if (exportacaoResultado is null)
                throw new NegocioException("A exportação não foi encontrada");
            
            if (exportacaoResultado.Status != ExportacaoResultadoStatus.Processando) 
                return true;
            
            var checarProvaExiste = await mediator.Send(new VerificaProvaExistePorSerapIdQuery(extracao.ProvaSerapId));
            if (!checarProvaExiste)
                throw new NegocioException("A prova informada não foi encontrada no serap estudantes");
            
            var resultadoExtracaoProva = await mediator.Send(new VerificaResultadoExtracaoProvaExisteQuery(extracao.ProvaSerapId));
            if (!resultadoExtracaoProva)
                throw new NegocioException($"Os resultados da prova {extracao.ProvaSerapId} ainda não foram gerados");

            try
            {
                var prova = await mediator.Send(new ObterProvaDetalhesPorProvaLegadoIdQuery(extracao.ProvaSerapId));
                if (prova == null)
                    return false;
                
                await mediator.Send(new GerarCSVExtracaoProvaCommand(prova.TotalItens, extracao.CaminhoArquivo));
                
                var dres = await mediator.Send(new ObterDresSerapQuery());
                if (dres == null || !dres.Any())
                    return false;
                
                foreach (var dre in dres)
                {
                    var ues = await mediator.Send(new ObterUesSerapPorProvaSerapEDreCodigoQuery(extracao.ProvaSerapId, dre.CodigoDre));
                    if (ues == null || !ues.Any())
                        continue;

                    foreach (var ue in ues)
                    {
                        await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ExtrairResultadosProvaFiltro,
                            new TratarProvaResultadoExtracaoFiltroDto(extracao.ExtracaoResultadoId,
                                extracao.ProvaSerapId, extracao.CaminhoArquivo, dre.CodigoDre, ue.CodigoUe)));                        
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Erro));
                await mediator.Send(new ExcluirExportacaoResultadoItemCommand(0, exportacaoResultado.Id));

                servicoLog.Registrar($"Escrever dados no arquivo CSV. msg: {mensagemRabbit.Mensagem}", ex);

                return false;
            }
        }
    }
}