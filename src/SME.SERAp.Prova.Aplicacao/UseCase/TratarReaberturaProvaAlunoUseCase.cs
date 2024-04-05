using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.UseCase
{ 
    public class TratarReaberturaProvaAlunoUseCase : ITratarReaberturaProvaAlunoUseCase
    {
        private readonly IMediator mediator;
        private readonly IServicoLog servicoLog;

        public TratarReaberturaProvaAlunoUseCase(IMediator mediator, IServicoLog servicoLog)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {

            var MensagemRabbitProvaAlunoReabertura = mensagemRabbit.ObterObjetoMensagem<ProvaAlunoReabertura>();
            var provaAlunoBanco = await mediator.Send(new ObterProvaAlunoPorProvaIdRaQuery(MensagemRabbitProvaAlunoReabertura.ProvaId, MensagemRabbitProvaAlunoReabertura.AlunoRA));
            if (provaAlunoBanco == null)
                throw new NegocioException($"A prova {MensagemRabbitProvaAlunoReabertura.ProvaId} não possui registro de inicio para o aluno {MensagemRabbitProvaAlunoReabertura.AlunoRA}");
          
            await mediator.Send(new ExcluirProvaAlunoCommand(MensagemRabbitProvaAlunoReabertura.ProvaId, MensagemRabbitProvaAlunoReabertura.AlunoRA));
            await mediator.Send(new RemoverCacheCommand(string.Format(CacheChave.AlunoProva, MensagemRabbitProvaAlunoReabertura.ProvaId, MensagemRabbitProvaAlunoReabertura.AlunoRA)));

            var entidade = new ProvaAlunoReabertura
            {
                AlunoRA = MensagemRabbitProvaAlunoReabertura.AlunoRA,
                LoginCoresso = MensagemRabbitProvaAlunoReabertura.LoginCoresso,
                ProvaId = MensagemRabbitProvaAlunoReabertura.ProvaId,
                UsuarioCoresso = MensagemRabbitProvaAlunoReabertura.UsuarioCoresso,
                GrupoCoresso = MensagemRabbitProvaAlunoReabertura.GrupoCoresso,
                CriadoEm = DateTime.Now

            };
            await mediator.Send(new IncluirProvaAlunoReaberturaCommand(entidade));
            
            servicoLog.Registrar(Dominio.Enums.LogNivel.Informacao, $"Solicitação de reabertura da prova {MensagemRabbitProvaAlunoReabertura.ProvaId} para o aluno {MensagemRabbitProvaAlunoReabertura.AlunoRA} em {DateTime.Now}. Solicitação feita pelo login: {MensagemRabbitProvaAlunoReabertura.LoginCoresso}.");

            await mediator.Send(new PublicarFilaRabbitSerapAcompanhamentoCommand(RotasRabbit.ProvaAlunoReaberturaTratarAcompanhamento, MensagemRabbitProvaAlunoReabertura));
            return true;
        }
    }
}
