using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos.Tai;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProvaTaiUseCase : ITratarProvaTaiUseCase
    {
        private readonly IMediator mediator;
        private readonly IServicoLog servicoLog;

        public TratarProvaTaiUseCase(IMediator mediator, IServicoLog servicoLog)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            const string caderno = "999";
            
            var provaTai = mensagemRabbit.ObterObjetoMensagem<ProvaTaiSyncDto>();

            var alunosProvaTaiSemCaderno = (await mediator.Send(new ObterAlunosProvaTaiSemCadernoQuery(provaTai.ProvaId, provaTai.Ano))).ToList();
            
            if (alunosProvaTaiSemCaderno == null || !alunosProvaTaiSemCaderno.Any())
                throw new NegocioException("Todos os alunos já possuem cadernos para a prova.");

            var dadosDaAmostraTai = (await mediator.Send(new ObterDadosAmostraProvaTaiQuery(provaTai.ProvaLegadoId))).ToList();
            
            if (dadosDaAmostraTai == null || !dadosDaAmostraTai.Any())
                throw new NegocioException($"Os dados da amostra tai não foram cadastrados para a prova {provaTai.ProvaLegadoId}");
            
            var listaLog = new List<string>();
            var amostrasUtilizar = new List<ItemAmostraTaiDto>();

            foreach (var dadosAmostra in dadosDaAmostraTai)
            {
                foreach (var configItem in dadosAmostra.ListaConfigItens)
                {
                    var itensAmostra = (await mediator
                            .Send(new ObterItensAmostraTaiQuery(configItem.MatrizId, new [] { configItem.TipoCurriculoGradeId })))
                        .ToList();
                    
                    var numeroItens = itensAmostra.Count * configItem.Porcentagem / 100;
                    var itensAmostraUtilizar = itensAmostra.Take(numeroItens).ToList();
                    amostrasUtilizar.AddRange(itensAmostraUtilizar);
                }
                
                if (!amostrasUtilizar.Any() || amostrasUtilizar.Count < dadosAmostra.NumeroItensAmostra)
                {
                    listaLog.Add($"A quantidade de itens configurados com TRI é menor do que o número de itens para a prova {provaTai.ProvaLegadoId}.");
                    continue;                    
                }
                
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarCadernosProvaTai,
                    new CadernoProvaTaiTratarDto(provaTai.ProvaId, provaTai.ProvaLegadoId, provaTai.Disciplina,
                        alunosProvaTaiSemCaderno, dadosAmostra.NumeroItensAmostra, amostrasUtilizar, provaTai.Ano, caderno)));                
            }

            if (!listaLog.Any()) 
                return true;
            
            foreach (var log in listaLog)
                servicoLog.Registrar(LogNivel.Negocio, log);

            return true;
        }
    }
}