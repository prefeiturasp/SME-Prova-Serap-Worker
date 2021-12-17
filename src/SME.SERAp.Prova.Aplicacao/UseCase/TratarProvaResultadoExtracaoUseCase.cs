﻿using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProvaResultadoExtracaoUseCase : ITratarProvaResultadoExtracaoUseCase
    {
        private readonly IMediator mediator;

        public TratarProvaResultadoExtracaoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var provaSerapId = mensagemRabbit.ObterObjetoMensagem<ProvaExtracaoDto>();
            ExportacaoResultado exportacaoResultado = new ExportacaoResultado($"{Guid.NewGuid().ToString().ToUpper()}.csv", provaSerapId.Id);
            try
            {
                if (provaSerapId == null)
                    throw new NegocioException("O id da prova serap precisa ser informado");

                var checarProvaExiste = await mediator.Send(new VerificaProvaExistePorSerapIdQuery(provaSerapId.Id));

                if (!checarProvaExiste)
                    throw new NegocioException("A prova informada não foi encontrada no serap estudantes");


                exportacaoResultado.Id = await mediator.Send(new ExportacaoResultadoIncluirCommand(exportacaoResultado));

                var resultado = await mediator.Send(new ObterExtracaoProvaRespostaQuery(provaSerapId.Id));

                if (!resultado.Any())
                    throw new NegocioException($"Os resultados da prova {provaSerapId} ainda não foram gerados");

                await mediator.Send(new GerarCSVExtracaoProvaCommand(resultado, exportacaoResultado.NomeArquivo));
                exportacaoResultado.AtualizarStatus(ExportacaoResultadoStatus.Finalizado);
                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado));

            }
            catch (Exception ex)
            {
                exportacaoResultado.AtualizarStatus(ExportacaoResultadoStatus.Erro);
                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado));
                throw ex;
            }
            return true;
        }
    }
}