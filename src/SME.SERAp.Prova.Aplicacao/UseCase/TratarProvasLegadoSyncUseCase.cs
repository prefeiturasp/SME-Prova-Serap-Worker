using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProvasLegadoSyncUseCase : ITratarProvasLegadoSyncUseCase
    {
        private readonly IMediator mediator;
        private readonly IServicoLog serviceLog;


        public TratarProvasLegadoSyncUseCase(IMediator mediator, IServicoLog serviceLog)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.serviceLog = serviceLog ?? throw new ArgumentNullException(nameof(serviceLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var ultimaAtualizacao = await mediator.Send(new ObterUltimoExecucaoControleTipoPorTipoQuery(ExecucaoControleTipo.ProvaLegadoSincronizacao));

            try
            {
                serviceLog.Registrar(LogNivel.Informacao, $"Última Atualização {ultimaAtualizacao.UltimaExecucao}");
                var provaIds = await mediator.Send(new ObterProvaLegadoParaSeremSincronizadasQuery(ultimaAtualizacao.UltimaExecucao));
               
                serviceLog.Registrar(LogNivel.Informacao, $"Última Atualização {ultimaAtualizacao.UltimaExecucao}");
                serviceLog.Registrar(LogNivel.Informacao, $"Total de provas para sincronizar {provaIds.ToList().Count}");
                
                foreach (var provaId in provaIds)
                {
                    serviceLog.Registrar(LogNivel.Informacao, $"Enviando prova {provaId} para tratar");
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ProvaTratar, provaId));
                }              
            }  
            finally
            {
                await mediator.Send(new ExecucaoControleAtualizarCommand(ultimaAtualizacao));
            }

            return true;
        }
    }
}