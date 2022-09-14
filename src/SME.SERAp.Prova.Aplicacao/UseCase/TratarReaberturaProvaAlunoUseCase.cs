using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using SME.SERAp.Prova.Infra.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.UseCase
    public class TratarReaberturaProvaAlunoUseCase : ITratarReaberturaProvaAlunoUseCase
    {
        private readonly IMediator mediator;
        private readonly IServicoLog servicoLog;

        public TratarReaberturaProvaAlunoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }


        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {

            var ProvaAlunoReabertura = mensagemRabbit.ObterObjetoMensagem<ProvaAlunoReabertura>();
            var provaAlunoBanco = await mediator.Send(new ObterProvaAlunoPorProvaIdRaQuery(ProvaAlunoReabertura.ProvaId, ProvaAlunoReabertura.AlunoRA));
            if (provaAlunoBanco == null)
                throw new NegocioException($"A prova {ProvaAlunoReabertura.ProvaId} não possui registro de inicio para o aluno {ProvaAlunoReabertura.AlunoRA}");
          
            await mediator.Send(new ExcluirProvaAlunoCommand(ProvaAlunoReabertura.ProvaId, ProvaAlunoReabertura.AlunoRA));
            await mediator.Send(new RemoverCacheCommand(string.Format(CacheChave.AlunoProva, ProvaAlunoReabertura.ProvaId, ProvaAlunoReabertura.AlunoRA)));
            await mediator.Send(new IncluirProvaAlunoReaberturaCommand(ProvaAlunoReabertura));
            
            servicoLog.Registrar(Dominio.Enums.LogNivel.Informacao, $"Solicitação de reabertura da prova {ProvaAlunoReabertura.ProvaId} para o aluno {ProvaAlunoReabertura.AlunoRA} em {DateTime.Now}");

            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ProvaAlunoReaberturaTratarAcompanhamento, ProvaAlunoReabertura));
            return true;
        }
    }
}
