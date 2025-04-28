using MediatR;
using SME.SERAp.Prova.Aplicacao.Commands;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Aplicacao.Queries;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.UseCase
{
    public class TratarAlunosComRespostasSemQuestoesUseCase : ITratarAlunosComRespostasSemQuestoesUseCase
    {
        private readonly IMediator mediator;
        public TratarAlunosComRespostasSemQuestoesUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var alunoProva = mensagemRabbit.ObterObjetoMensagem<AlunoProvaRespostaSemPerguntaDto>();
                if (alunoProva is null)
                    return false;

                var alunoRa = alunoProva.AlunoRa;
                var alunoId = alunoProva.AlunoId;
                var provaId = alunoProva.ProvaId;

                var provaAlunoDuplicada = await mediator.Send(new VerificaProvaAlunoDuplicadaQuery(alunoRa, provaId));
                if (provaAlunoDuplicada)
                    return true;

                var respostasComOrdem = await ObterQuestoesAlunoRespostaComOrdem(alunoRa, provaId);
                if (respostasComOrdem?.Count() > 0)
                {
                    var questoesAlunoTai = await ObterQuestoesAlunoTaiPorAlunoRaProvaId(alunoRa, provaId);
                    if (questoesAlunoTai?.Count() > 0)
                    {
                        if (questoesAlunoTai.Count() > 1)
                        {
                            return true;
                        }

                        var questaoAlunoTaiRestante = questoesAlunoTai.FirstOrDefault();
                        respostasComOrdem = respostasComOrdem.Where(x => x.QuestaoId != questaoAlunoTaiRestante.QuestaoId).ToList();
                        AtualizarQuestoesOrdem(respostasComOrdem);

                        questaoAlunoTaiRestante.Ordem = respostasComOrdem.Max(x => x.Ordem) + 1;
                        await AlterarQuestaoAlunoTai(questaoAlunoTaiRestante);
                    }

                    foreach (var questaoAlunoRespostaComOrdem in respostasComOrdem.OrderBy(x => x.Ordem))
                    {
                        await IncluirQuestaoAlunoTai(questaoAlunoRespostaComOrdem, alunoId);
                    }

                    await LimparCachesProvaAluno(alunoRa, alunoId, provaId);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<IEnumerable<QuestaoAlunoTai>> ObterQuestoesAlunoTaiPorAlunoRaProvaId(long aluno, long prova)
        {
            return await mediator.Send(new ObterQuestoesAlunoTaiPorAlunoRaProvaIdQuery(aluno, prova));
        }

        private async Task<IEnumerable<QuestaoAlunoRespostaComOrdemDto>> ObterQuestoesAlunoRespostaComOrdem(long aluno, long prova)
        {
            return await mediator.Send(new ObterQuestoesAlunoRespostaComOrdemQuery(aluno, prova));
        }

        private async Task AlterarQuestaoAlunoTai(QuestaoAlunoTai questaoAlunoTaiRestante)
        {
            await mediator.Send(new AlterarQuestaoAlunoTaiCommand(questaoAlunoTaiRestante));
        }

        private async Task IncluirQuestaoAlunoTai(QuestaoAlunoRespostaComOrdemDto questaoAlunoRespostaComOrdem, long alunoId)
        {
            var questaoAlunoTai = new QuestaoAlunoTai(questaoAlunoRespostaComOrdem.QuestaoId, alunoId,
                questaoAlunoRespostaComOrdem.Ordem, questaoAlunoRespostaComOrdem.CriadoEm);

            await mediator.Send(new IncluirQuestaoAlunoTaiCommand(questaoAlunoTai));
        }

        private async Task LimparCachesProvaAluno(long alunoRa, long alunoId, long provaId)
        {
            await mediator.Send(new RemoverCacheCommand($"al-prova-{provaId}-{alunoRa}"));
            await mediator.Send(new RemoverCacheCommand($"al-q-administrado-tai-prova-{alunoId}-{provaId}"));
        }

        private void AtualizarQuestoesOrdem(IEnumerable<QuestaoAlunoRespostaComOrdemDto> lista)
        {
            var ordenados = lista.OrderBy(x => x.Ordem).ToList();

            for (int i = 0; i < ordenados.Count; i++)
            {
                ordenados[i].Ordem = i;
            }
        }
    }
}
