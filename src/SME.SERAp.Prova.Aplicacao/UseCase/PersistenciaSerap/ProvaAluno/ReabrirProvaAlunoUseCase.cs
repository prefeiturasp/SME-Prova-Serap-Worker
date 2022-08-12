using MediatR;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.UseCase
{
    public class ReabrirProvaAlunoUseCase : AbstractUseCase, IReabrirProvaAlunoUseCase
    {
        private readonly IServicoLog servicoLog;

        public ReabrirProvaAlunoUseCase(IMediator mediator, IServicoLog servicoLog) : base(mediator)
        {
           this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var provaAluno = mensagemRabbit.ObterObjetoMensagem<ProvaAluno>();

            var provaAlunoBanco = await mediator.Send(new ObterProvaAlunoPorProvaIdRaQuery(provaAluno.ProvaId, provaAluno.AlunoRA));
            if (provaAlunoBanco == null)
                throw new NegocioException($"A prova {provaAluno.ProvaId} não possui registro de inicio para o aluno {provaAluno.AlunoRA}");

            provaAlunoBanco.FinalizadoEm = null;
            provaAlunoBanco.Status = ProvaStatus.Iniciado;

            await mediator.Send(new AtualizarProvaAlunoCommand(provaAlunoBanco));
            await mediator.Send(new RemoverCacheCommand(string.Format(CacheChave.AlunoProva, provaAluno.ProvaId, provaAluno.AlunoRA)));

            servicoLog.Registrar(Dominio.Enums.LogNivel.Informacao, $"Solicitação de reabertura da prova {provaAluno.ProvaId} para o aluno {provaAluno.AlunoRA} em {DateTime.Now}");
            return true;
        }
    }
}
