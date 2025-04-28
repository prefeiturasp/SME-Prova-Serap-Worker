using MediatR;
using SME.SERAp.Prova.Aplicacao.Commands;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Aplicacao.Queries;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using System;
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

                var questoesAlunoRespostaComOrdem = await ObterQuestoesAlunoRespostaComOrdem(alunoRa, provaId);
                if (questoesAlunoRespostaComOrdem?.Count() > 0)
                {
                    var todasQuestoesAlunoTai = new List<QuestaoAlunoTai>();
                    var questaoAlunoRespostaSemQuestaoAlunoTai = questoesAlunoRespostaComOrdem.ToList();

                    var questoesAlunoTai = await ObterQuestoesAlunoTaiPorAlunoRaProvaId(alunoRa, provaId);
                    if (questoesAlunoTai?.Count() > 0)
                    {
                        todasQuestoesAlunoTai.AddRange(questoesAlunoTai);

                        var questoesAlunoTaiIds = questoesAlunoTai.Select(x => x.QuestaoId).ToList();
                        questaoAlunoRespostaSemQuestaoAlunoTai = questoesAlunoRespostaComOrdem.Where(x => !questoesAlunoTaiIds.Contains(x.QuestaoId)).ToList();
                    }

                    todasQuestoesAlunoTai.AddRange(CriarNovasQuestaoAlunoTai(questaoAlunoRespostaSemQuestaoAlunoTai, alunoId));

                    AtualizarQuestoesAlunoTaiOrdem(todasQuestoesAlunoTai);

                    var questoesAlunoTaiAtualizar = todasQuestoesAlunoTai.Where(x => x.Id > 0);
                    var questoesAlunoTaiIncluir = todasQuestoesAlunoTai.Where(x => x.Id == 0);

                    if (questoesAlunoTaiAtualizar?.Count() > 0)
                    {
                        foreach (var questaoAlunoTai in questoesAlunoTaiAtualizar)
                        {
                            await AlterarQuestaoAlunoTai(questaoAlunoTai);
                        }
                    }

                    foreach (var questaoAlunoTai in questoesAlunoTaiIncluir)
                    {
                        await IncluirQuestaoAlunoTai(questaoAlunoTai);
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

        private IEnumerable<QuestaoAlunoTai> CriarNovasQuestaoAlunoTai(List<QuestaoAlunoRespostaComOrdemDto> questaoAlunoRespostaSemQuestaoAlunoTai, long alunoId)
        {
            return questaoAlunoRespostaSemQuestaoAlunoTai.Select(questaoAlunoResposta => new QuestaoAlunoTai(questaoAlunoResposta.QuestaoId, alunoId,
                0, questaoAlunoResposta.CriadoEm));
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

        private async Task IncluirQuestaoAlunoTai(QuestaoAlunoTai questaoAlunoTai)
        {
            await mediator.Send(new IncluirQuestaoAlunoTaiCommand(questaoAlunoTai));
        }

        private async Task LimparCachesProvaAluno(long alunoRa, long alunoId, long provaId)
        {
            await mediator.Send(new RemoverCacheCommand($"al-prova-{provaId}-{alunoRa}"));
            await mediator.Send(new RemoverCacheCommand($"al-q-administrado-tai-prova-{alunoId}-{provaId}"));
        }

        private void AtualizarQuestoesAlunoTaiOrdem(IEnumerable<QuestaoAlunoTai> lista)
        {
            var ordenados = lista.OrderBy(x => x.CriadoEm).ToList();

            var ordem = 0;
            foreach (var item in ordenados)
            {
                item.Ordem = ordem;
                ordem++;
            }
        }
    }
}
