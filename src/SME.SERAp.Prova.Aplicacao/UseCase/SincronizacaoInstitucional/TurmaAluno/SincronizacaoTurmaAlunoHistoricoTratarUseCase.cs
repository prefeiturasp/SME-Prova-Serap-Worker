using MediatR;
using Sentry;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class SincronizacaoTurmaAlunoHistoricoTratarUseCase : AbstractUseCase, ISincronizacaoTurmaAlunoHistoricoTratarUseCase
    {
        public SincronizacaoTurmaAlunoHistoricoTratarUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var alunosRa = mensagemRabbit.ObterObjetoMensagem<long[]>();

            if(alunosRa == null)
            {
                var mensagem = $"Não foi possível fazer parse da mensagem para tratar turmas histórico dos alunos. {mensagemRabbit.Mensagem}.";
                SentrySdk.CaptureMessage(mensagem);
                throw new NegocioException(mensagem);
            }

            foreach(long ra in alunosRa)
            {
                //obter turmas histórico serap

                //obter turmas histórico eol
                var turmasHistoricoEol = await mediator.Send(new ObterTurmaAlunoHistoricoEolPorAlunoRaQuery(ra));
            }

            return true;
        }
    }
}
