using MediatR;
using SME.SERAp.Prova.Infra;
using System;
using System.Threading.Tasks;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra.Interfaces;

namespace SME.SERAp.Prova.Aplicacao
{
    public class IncluirRespostaAlunoUseCase : IIncluirRespostaAlunoUseCase
    {
        private readonly IMediator mediator;
        private readonly IServicoLog servicoLog;

        public IncluirRespostaAlunoUseCase(IMediator mediator, IServicoLog servicoLog)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dto = mensagemRabbit.ObterObjetoMensagem<QuestaoAlunoRespostaSincronizarDto>();
            var horaDataResposta = new DateTime(dto.DataHoraRespostaTicks);
            
            horaDataResposta = horaDataResposta.AddHours(-3);

            var questaoRespondida = await mediator.Send(new ObterQuestaoAlunoRespostaPorIdRaQuery(dto.QuestaoId, dto.AlunoRa));

            if (questaoRespondida == null)
            {
                var questao = await mediator.Send(new ObterQuestaoPorIdQuery(dto.QuestaoId));

                if (questao != null)
                {
                    return await mediator.Send(new IncluirQuestaoAlunoRespostaCommand(dto.QuestaoId,
                        dto.AlunoRa,
                        dto.AlternativaId,
                        dto.Resposta,
                        horaDataResposta,
                        dto.TempoRespostaAluno ?? 0,
                        dto.DispositivoId));
                }

                servicoLog.Registrar(LogNivel.Informacao,$"Questão não existe na base de dados: {mensagemRabbit.Mensagem.ToString()}");
                return false;
            }

            if (questaoRespondida.CriadoEm.Date > DateTime.Now.Date)
                questaoRespondida.CriadoEm = DateTime.Now.Date;

            if (questaoRespondida.AlternativaId != null)
            {
                if (questaoRespondida.CriadoEm > horaDataResposta)
                    return false;
            }

            if (questaoRespondida.AlternativaId == null && !string.IsNullOrEmpty(questaoRespondida.Resposta))
            {
                if (questaoRespondida.CriadoEm > horaDataResposta)
                    return false;
            }

            questaoRespondida.Resposta = dto.Resposta;
            questaoRespondida.AlternativaId = dto.AlternativaId;
            questaoRespondida.TempoRespostaAluno += dto.TempoRespostaAluno ?? 0;
            questaoRespondida.CriadoEm = horaDataResposta;
            questaoRespondida.Visualizacoes += 1;
            questaoRespondida.DispositivoId = dto.DispositivoId;

            return await mediator.Send(new AtualizarQuestaoAlunoRespostaCommand(questaoRespondida));
        }
    }
}