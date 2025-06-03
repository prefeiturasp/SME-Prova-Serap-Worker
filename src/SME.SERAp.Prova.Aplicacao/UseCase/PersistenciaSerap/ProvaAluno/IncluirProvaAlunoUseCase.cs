using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class IncluirProvaAlunoUseCase : AbstractUseCase, IIncluirProvaAlunoUseCase
    {
        private readonly IServicoLog servicoLog;
        
        public IncluirProvaAlunoUseCase(IMediator mediator, IServicoLog servicoLog) : base(mediator)
        {
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var provaAluno = mensagemRabbit.ObterObjetoMensagem<ProvaAluno>();

                var provaAlunoBanco = await mediator.Send(new ObterProvaAlunoPorProvaIdRaQuery(provaAluno.ProvaId, provaAluno.AlunoRA));

                if (provaAlunoBanco == null)
                {
                    provaAluno.Id = await mediator.Send(new IncluirProvaAlunoCommand(provaAluno));

                    await mediator.Send(new SalvarCacheCommand(string.Format(CacheChave.AlunoProva, provaAluno.ProvaId, provaAluno.AlunoRA), provaAluno));
                }

                return true;
            }
            catch (Exception ex)
            {
                servicoLog.Registrar($"Erro ao gravar  prova! -- Mensagem: {mensagemRabbit}", ex);
                return false;
            }
        }
    }
}