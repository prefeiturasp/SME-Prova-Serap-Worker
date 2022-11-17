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
        private readonly IRepositorioCache repositorioCache;
        private readonly IServicoLog servicoLog;
        public AlterarProvaAlunoUseCase(IMediator mediator, 
                                        IRepositorioCache repositorioCache,
                                        IServicoLog servicoLog) : base(mediator)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var provaAluno = mensagemRabbit.ObterObjetoMensagem<ProvaAluno>();
                await mediator.Send(new AtualizarProvaAlunoCommand(provaAluno));
                await repositorioCache.SalvarRedisAsync(string.Format(CacheChave.AlunoProva, provaAluno.ProvaId, provaAluno.AlunoRA), provaAluno);
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
