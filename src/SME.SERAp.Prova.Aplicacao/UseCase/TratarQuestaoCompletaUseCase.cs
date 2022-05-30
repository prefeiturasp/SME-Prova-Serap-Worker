using MessagePack;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarQuestaoCompletaUseCase : ITratarQuestaoCompletaUseCase
    {
        private readonly IRepositorioQuestao repositorioQuestao;
        private readonly IRepositorioQuestaoCompleta repositorioQuestaoCompleta;
        private readonly IRepositorioCache repositorioCache;

        public TratarQuestaoCompletaUseCase(IRepositorioQuestao repositorioQuestao, IRepositorioQuestaoCompleta repositorioQuestaoCompleta, IRepositorioCache repositorioCache)
        {
            this.repositorioQuestao = repositorioQuestao;
            this.repositorioQuestaoCompleta = repositorioQuestaoCompleta;
            this.repositorioCache = repositorioCache;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var questaoAtualizada = mensagemRabbit.ObterObjetoMensagem<QuestaoAtualizada>();
            var questaoCompleta = await repositorioQuestao.MontarQuestaoCompletaPorIdAsync(questaoAtualizada.Id);

            if (questaoCompleta != null)
            {
                if (questaoCompleta.QuantidadeAlternativas != questaoCompleta.Alternativas.Count())
                throw new NegocioException($"Total de alternativas diferente do informado na questão {questaoAtualizada.Id}");

                var bytes = MessagePackSerializer.Serialize(questaoCompleta);
                var json = MessagePackSerializer.ConvertToJson(bytes);

                await repositorioQuestaoCompleta.IncluirOuUpdateAsync(new QuestaoCompleta(questaoAtualizada.Id, json, questaoAtualizada.UltimaAtualizacao));
                await repositorioCache.RemoverRedisAsync(string.Format(CacheChave.QuestaoCompleta, questaoAtualizada.Id));
            } 

            return true;
        }
    }
}
