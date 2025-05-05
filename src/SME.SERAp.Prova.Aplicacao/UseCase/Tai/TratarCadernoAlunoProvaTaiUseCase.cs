using System;
using System.Linq;
using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Threading.Tasks;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Aplicacao.Queries.Questao.ExisteQuestaoAlunoTaiPorId;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dados.Mapeamentos;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarCadernoAlunoProvaTaiUseCase : AbstractUseCase, ITratarCadernoAlunoProvaTaiUseCase
    {

        private IRepositorioAluno repositorioAluno;
        public TratarCadernoAlunoProvaTaiUseCase(IMediator mediator, IRepositorioAluno _repositorioAluno) : base(mediator)
        {
            repositorioAluno = _repositorioAluno;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var resultado = await repositorioAluno.Correcao();
            var contador = resultado.Count();
            foreach (var alunoProva in resultado)
            {

                //var alunoProva = mensagemRabbit.ObterObjetoMensagem<AlunoCadernoProvaTaiTratarDto>();

                //var alunoProva = new AlunoCadernoProvaTaiTratarDto(547, 1870156, 11739, 8366123, "Matemática", 9, 1  , "1")

                




                //var alunoProva = new AlunoCadernoProvaTaiTratarDto(551, 1274378, 11744, 5584405, "Matemática", "9", "1");


                var nomeChave = string.Format(CacheChave.SincronizandoProvaTaiAluno, alunoProva.ProvaId, alunoProva.AlunoId);
                try
                {

                    if (alunoProva.AlunoRa == 934352 || alunoProva.AlunoId == 934352 || alunoProva.AlunoRa == 700021 || alunoProva.AlunoId == 700021 || alunoProva.AlunoRa == 171162 || alunoProva.AlunoId == 171162)
                        continue;
                    /*var sincronizandoProvaAlunoTai = (bool)await mediator.Send(new ObterCacheQuery(nomeChave));
                    if (sincronizandoProvaAlunoTai)
                        return false;

                    await mediator.Send(new SalvarCacheCommand(nomeChave, true));*/

                    var cadernoAluno = new CadernoAluno(

                        alunoProva.AlunoId,
                        alunoProva.ProvaId,
                        alunoProva.Caderno);

                    var existeCadernoAluno = await mediator.Send(new ExisteCadernoAlunoPorProvaIdAlunoIdQuery(cadernoAluno.ProvaId, cadernoAluno.AlunoId));
                    var existeQuestaoAlunoTai = await mediator.Send(new ExisteQuestaoAlunoTaiPorAlunoIdQuery(cadernoAluno.ProvaId, cadernoAluno.AlunoId));

                    if (!existeCadernoAluno)
                    {
                        await mediator.Send(new CadernoAlunoIncluirCommand(cadernoAluno));
                    }

                    if (!existeQuestaoAlunoTai)
                    {
                        await IncluirPrimeiraQuestaoAlunoTai(alunoProva.ProvaId, alunoProva.AlunoId, alunoProva.Caderno);

                        //-> Limpar o cache
                        await RemoverQuestaoAmostraTaiAlunoCache(alunoProva.AlunoRa, alunoProva.ProvaId);
                        await RemoverRespostaAmostraTaiAlunoCache(alunoProva.AlunoRa, alunoProva.ProvaId);
                        await RemoverQuestaoProvaAlunoResumoCache(alunoProva.ProvaId, alunoProva.AlunoId);

                       

                        await mediator.Send(new RemoverCacheCommand(nomeChave));
                    }

                    await RemoverCaches(alunoProva.ProvaId, alunoProva.AlunoRa);
                    await RemoverCaches2(alunoProva.ProvaId, alunoProva.AlunoId);






                }
                catch
                {
                    await mediator.Send(new RemoverCacheCommand(nomeChave));
                    continue;
                }

                contador--;

            }

            return true;



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


        private async Task RemoverCaches(long provaId, long alunoRA)
        {
            await mediator.Send(new RemoverCacheCommand($"al-prova-{provaId}-{alunoRA}"));            
        }

        private async Task RemoverCaches2(long provaId, long alunoId)
        {
            await mediator.Send(new RemoverCacheCommand($"al-q-administrado-tai-prova-{alunoId}-{provaId}"));
        }
    }
}
