using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Sentry;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class IncluirRespostaAlunoUseCase : IIncluirRespostaAlunoUseCase
    {

        private readonly IMediator mediator;

        public IncluirRespostaAlunoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dto = mensagemRabbit.ObterObjetoMensagem<QuestaoAlunoRespostaSincronizarDto>();
            DateTime horaDataResposta = new DateTime(dto.DataHoraRespostaTicks);

            var questaoRespondida = await mediator.Send(new ObterQuestaoAlunoRespostaPorIdRaQuery(dto.QuestaoId, dto.AlunoRa));

            if (questaoRespondida == null)
            {
                return await mediator.Send(new IncluirQuestaoAlunoRespostaCommand(dto.QuestaoId, dto.AlunoRa, dto.AlternativaId, dto.Resposta, horaDataResposta, dto.TempoRespostaAluno ?? 0));
            }
            else
            {
                if(questaoRespondida.AlternativaId != null)
                {
                    if (questaoRespondida.CriadoEm > horaDataResposta)
                    {
                        return false;
                    }
                }

                if (questaoRespondida.AlternativaId == null && !String.IsNullOrEmpty(questaoRespondida.Resposta))
                {
                    if (questaoRespondida.CriadoEm > horaDataResposta)
                    {
                        return false;
                    }
                }

                questaoRespondida.Resposta = dto.Resposta;
                questaoRespondida.AlternativaId = dto.AlternativaId;
                questaoRespondida.TempoRespostaAluno += dto.TempoRespostaAluno ?? 0;
                questaoRespondida.CriadoEm = horaDataResposta;
                questaoRespondida.Visualizacoes += 1;

                return await mediator.Send(new AtualizarQuestaoAlunoRespostaCommand(questaoRespondida));
            }
        }
    }
}