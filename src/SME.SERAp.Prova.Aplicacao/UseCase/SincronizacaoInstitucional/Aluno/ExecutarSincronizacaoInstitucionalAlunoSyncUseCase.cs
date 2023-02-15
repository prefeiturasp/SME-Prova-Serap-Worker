using MediatR;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExecutarSincronizacaoInstitucionalAlunoSyncUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalAlunoSyncUseCase
    {
       private readonly IServicoLog servicoLog;
        public ExecutarSincronizacaoInstitucionalAlunoSyncUseCase(IMediator mediator, IServicoLog servicoLog) : base(mediator)
        {
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dre = mensagemRabbit.ObterObjetoMensagem<DreParaSincronizacaoInstitucionalDto>();

            if (dre == null)
            {
                var mensagem = $"Não foi possível fazer parse da mensagem para sync de turmas da dre {mensagemRabbit.Mensagem}.";
                throw new NegocioException(mensagem);
            }

            var turmasDaDre = await mediator.Send(new ObterTurmasSerapPorDreCodigoQuery(dre.DreCodigo));
            if (turmasDaDre != null && turmasDaDre.Any())
            {
                foreach (var turma in turmasDaDre)
                {
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.SincronizaEstruturaInstitucionalAlunoTratar, turma));
                }
            }
            else throw new NegocioException($"Não foi possível localizar as turmas da Dre {dre.DreCodigo} para fazer sync dos alunos.");

            return true;
        }
    }
}
