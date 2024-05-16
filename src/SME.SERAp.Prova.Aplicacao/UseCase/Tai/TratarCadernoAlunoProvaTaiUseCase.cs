using System;
using System.Linq;
using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Threading.Tasks;
using SME.SERAp.Prova.Infra.Exceptions;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarCadernoAlunoProvaTaiUseCase : AbstractUseCase, ITratarCadernoAlunoProvaTaiUseCase
    {
        public TratarCadernoAlunoProvaTaiUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var alunoProva = mensagemRabbit.ObterObjetoMensagem<AlunoCadernoProvaTaiTratarDto>();

            var nomeChave = string.Format(CacheChave.SincronizandoProvaTaiAluno, alunoProva.ProvaId, alunoProva.AlunoId);
            try
            {
                var sincronizandoProvaAlunoTai = (bool)await mediator.Send(new ObterCacheQuery(nomeChave));
                if (sincronizandoProvaAlunoTai)
                    return false;

                await mediator.Send(new SalvarCacheCommand(nomeChave, true));

                var cadernoAluno = new CadernoAluno(
                    alunoProva.AlunoId,
                    alunoProva.ProvaId,
                    alunoProva.Caderno);

                var existe = await mediator.Send(new ExisteCadernoAlunoPorProvaIdAlunoIdQuery(cadernoAluno.ProvaId, cadernoAluno.AlunoId));
                if (!existe)
                {
                    await mediator.Send(new CadernoAlunoIncluirCommand(cadernoAluno));
                    await IncluirPrimeiraQuestaoAlunoTai(alunoProva.ProvaId, alunoProva.AlunoId, alunoProva.Caderno);
                }

                //-> Limpar o cache
                await RemoverQuestaoAmostraTaiAlunoCache(alunoProva.AlunoRa, alunoProva.ProvaId);
                await RemoverRespostaAmostraTaiAlunoCache(alunoProva.AlunoRa, alunoProva.ProvaId);
                await RemoverQuestaoProvaAlunoResumoCache(alunoProva.ProvaId, alunoProva.AlunoId);
                await mediator.Send(new RemoverCacheCommand(nomeChave));
                
                return true;
            }
            catch
            {
                await mediator.Send(new RemoverCacheCommand(nomeChave));
                throw;
            }
        }
        
        private async Task IncluirPrimeiraQuestaoAlunoTai(long provaId, long alunoId, string caderno)
        {
            var idsQuestoes = (await mediator.Send(new ObterIdsQuestoesPorProvaIdCadernoQuery(provaId, caderno))).Distinct().ToList();
            var sortear = new Random();
            var questaoIdSorteada = idsQuestoes[sortear.Next(idsQuestoes.Count)];
            
            var questaoAlunoTai = new QuestaoAlunoTai(questaoIdSorteada, alunoId, 0);
            var questaoAlunoTaiId = await mediator.Send(new QuestaoAlunoTaiIncluirCommand(questaoAlunoTai));
            
            if (questaoAlunoTaiId <= 0)
                throw new NegocioException($"As questões TAI do aluno {alunoId} não foram inseridas.");            
        }        

        private async Task RemoverQuestaoAmostraTaiAlunoCache(long alunoRa, long provaId)
        {
            await mediator.Send(new RemoverCacheCommand(string.Format(CacheChave.QuestaoAmostraTaiAluno,
                alunoRa, provaId)));
        }

        private async Task RemoverRespostaAmostraTaiAlunoCache(long alunoRa, long provaId)
        {
            await mediator.Send(new RemoverCacheCommand(string.Format(CacheChave.RespostaAmostraTaiAluno,
                alunoRa, provaId)));
        }

        private async Task RemoverQuestaoProvaAlunoResumoCache(long provaId, long alunoId)
        {
            await mediator.Send(new RemoverCacheCommand(string.Format(CacheChave.QuestaoProvaAlunoResumo,
                provaId, alunoId)));
        }
    }
}
