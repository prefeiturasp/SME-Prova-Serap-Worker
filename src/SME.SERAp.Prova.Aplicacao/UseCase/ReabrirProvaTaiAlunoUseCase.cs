using MediatR;
using SME.SERAp.Prova.Aplicacao.Commands;
using SME.SERAp.Prova.Aplicacao.Commands.AlunoProvaProficiencia.Excluir;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.UseCase
{
    public class ReabrirProvaTaiAlunoUseCase : AbstractUseCase, IReabrirProvaTaiAlunoUseCase
    {
        private readonly IServicoLog servicoLog;
        public ReabrirProvaTaiAlunoUseCase(IMediator mediator, IServicoLog servicoLog) : base(mediator)
        {
            this.servicoLog = servicoLog;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var provaAluno = mensagemRabbit.ObterObjetoMensagem<ProvaAluno>();
                if (provaAluno is null)
                    return false;

                var prova = await mediator.Send(new ObterProvaPorIdQuery(provaAluno.ProvaId));
                if(prova is null)
                    throw new NegocioException($"prova {provaAluno.ProvaId} não encontrada.");

                if(!prova.FormatoTai)
                    throw new NegocioException($"prova {provaAluno.ProvaId} deve ser formato TAI.");

                await mediator.Send(new ExcluirRespostaAlunoCommand(provaAluno.ProvaId, provaAluno.AlunoRA));
                await mediator.Send(new ExcluirQuestaoAlunoTaiCommand(provaAluno.ProvaId, provaAluno.AlunoRA));
                await mediator.Send(new ExcluirProvaAlunoCommand(provaAluno.ProvaId, provaAluno.AlunoRA));
                await mediator.Send(new ExcluirAlunoProvaProficienciaCommand(provaAluno.ProvaId, provaAluno.AlunoRA));
                await mediator.Send(new LimparCacheProvaTaiCommand(provaAluno.ProvaId, provaAluno.AlunoRA));

                return true;
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(ex);
                return false;
            }
        }
    }
}
