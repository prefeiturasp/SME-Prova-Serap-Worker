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
using System.Collections.Generic;

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
            var provaAno = new Dictionary<long, string>
            {
                {548, "9"},
                {549, "5"},
                {550, "5"},
                {551, "9"},
            };

            foreach(var prova in provaAno)
            {
                var resultado = await repositorioAluno.ObterAlunoSemProvaTai(prova.Key, prova.Value);

                foreach (var alunoProva in resultado)
                {
                    alunoProva.Caderno = "1";
                    alunoProva.ProvaId = prova.Key;

                    var nomeChave = string.Format(CacheChave.SincronizandoProvaTaiAluno, alunoProva.ProvaId, alunoProva.AlunoId);
                    try
                    {
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

                            await RemoverCaches(alunoProva.ProvaId, alunoProva.AlunoRa);
                            await RemoverCaches2(alunoProva.ProvaId, alunoProva.AlunoId);

                            await mediator.Send(new RemoverCacheCommand(nomeChave));
                        }
                    }
                    catch
                    {
                        await mediator.Send(new RemoverCacheCommand(nomeChave));
                        throw;
                    }

                }

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
