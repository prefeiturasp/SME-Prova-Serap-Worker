using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlterarProvaAlunoUseCase : AbstractUseCase, IAlterarProvaAlunoUseCase
    {
        IRepositorioCache repositorioCache;
        IServicoLog servicoLog;
        public AlterarProvaAlunoUseCase(IMediator mediator, IRepositorioCache repositorioCache) : base(mediator)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var provaAluno = mensagemRabbit.ObterObjetoMensagem<ProvaAluno>();
                await mediator.Send(new AtualizarProvaAlunoCommand(provaAluno));
                await repositorioCache.SalvarRedisAsync(provaAluno.ProvaId + provaAluno.AlunoRA.ToString(), provaAluno);
                return true;
            }
            catch (Exception ex)
            {
                servicoLog.Registrar($"Erro ao inicializar prova! -- Mensagem: {mensagemRabbit}", ex);
                return false;
            }

        }
    }
}
