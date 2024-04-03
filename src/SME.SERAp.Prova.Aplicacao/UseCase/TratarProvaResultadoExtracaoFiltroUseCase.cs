using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProvaResultadoExtracaoFiltroUseCase : ITratarProvaResultadoExtracaoFiltroUseCase
    {
        private readonly IMediator mediator;
        private readonly IServicoLog servicoLog;

        public TratarProvaResultadoExtracaoFiltroUseCase(IMediator mediator, IServicoLog servicoLog)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<TratarProvaResultadoExtracaoFiltroDto>();
            if (filtro is null)
                throw new NegocioException("O filtro precisa ser informado");            
            
            var exportacaoResultado = await mediator.Send(new ObterExportacaoResultadoStatusQuery(filtro.ProcessoId, filtro.ProvaSerapId));
            if (exportacaoResultado is null)
                throw new NegocioException("A exportação não foi encontrada");
            
            if (!File.Exists(filtro.CaminhoArquivo))
                throw new NegocioException($"Arquivo não foi encontrado: {filtro.CaminhoArquivo}");            
            
            if (exportacaoResultado.Status != ExportacaoResultadoStatus.Processando)
                return true;            

            try
            {
                var turmas = await mediator.Send(new ObterTurmasPorCodigoUeEProvaSerapQuery(filtro.UeCodigoEol, filtro.ProvaSerapId));
                if (turmas == null || !turmas.Any())
                    return false;                
                
                var listaFiltroTurma = new List<TratarProvaResultadoExtracaoFiltroTurmaDto>();
                foreach (var turma in turmas)
                {
                    var exportacaoResultadoItem = new ExportacaoResultadoItem(exportacaoResultado.Id,
                        filtro.DreCodigoEol, filtro.UeCodigoEol, turma.Codigo);
                    exportacaoResultadoItem.Id = await mediator.Send(new InserirExportacaoResultadoItemCommand(exportacaoResultadoItem));

                    listaFiltroTurma.Add(new TratarProvaResultadoExtracaoFiltroTurmaDto(filtro.ProcessoId, filtro.ProvaSerapId,
                        filtro.DreCodigoEol, filtro.UeCodigoEol, turma.Codigo, exportacaoResultadoItem.Id, filtro.CaminhoArquivo));
                }

                foreach (var filtroTurma in listaFiltroTurma)
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ExtrairResultadosProvaFiltroTurma, filtroTurma));
            }
            catch (Exception ex)
            {
                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Erro));
                await mediator.Send(new ExcluirExportacaoResultadoItemCommand(0, exportacaoResultado.Id));

                servicoLog.Registrar($"Escrever dados no arquivo CSV. msg: {mensagemRabbit.Mensagem}", ex);

                return false;
            }

            return true;
        }
        
        /*
        private static List<List<Turma>> Paginar(IEnumerable<Turma> turmas)
        {
            var paginacao = new ListaPaginada<Turma>(turmas.ToList(), 5);
            return paginacao.ObterTodasAsPaginas();
        }
        */        
    }
}
