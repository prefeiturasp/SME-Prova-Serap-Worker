using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProvaTaiUseCase : ITratarProvaTaiUseCase
    {
        private readonly IMediator mediator;

        public TratarProvaTaiUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var provaTai = mensagemRabbit.ObterObjetoMensagem<ProvaTaiSyncDto>();

            var alunosProvaTaiSemCaderno = await mediator.Send(new ObterAlunosProvaTaiSemCadernoQuery(provaTai.ProvaId, provaTai.Ano));
            
            var alunosAtivosProvaTaiSemCaderno = alunosProvaTaiSemCaderno.Where(c => c.Ativo());
            if (alunosAtivosProvaTaiSemCaderno == null || !alunosAtivosProvaTaiSemCaderno.Any())
                throw new NegocioException("Todos os alunos já possuem cadernos para a prova.");

            foreach (var aluno in alunosAtivosProvaTaiSemCaderno)
            {
                var msg = new AlunoCadernoProvaTaiTratarDto(provaTai.ProvaId, aluno.AlunoId,
                    provaTai.ProvaLegadoId, aluno.AlunoRa, provaTai.Disciplina,
                    provaTai.Ano, ProvaTai.Caderno);
            
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarCadernoAlunoProvaTai, msg));
            }
            
            //-> Limpar o cache
            await RemoverQuestaoProvaResumoCache(provaTai.ProvaId);

            return true;
        }
        
        private async Task RemoverQuestaoProvaResumoCache(long provaId)
        {
            await mediator.Send(new RemoverCacheCommand(string.Format(CacheChave.QuestaoProvaResumo, provaId)));
        }
    }
}