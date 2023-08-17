using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos.Tai;
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

            var alunosProvaTaiSemCaderno = (await mediator.Send(new ObterAlunosProvaTaiSemCadernoQuery(provaTai.ProvaId, provaTai.Ano))).ToList();
            
            if (alunosProvaTaiSemCaderno == null || !alunosProvaTaiSemCaderno.Any())
                throw new NegocioException("Todos os alunos já possuem cadernos para a prova.");
            
            var dadosDaAmostraTai = await mediator.Send(new ObterDadosAmostraProvaTaiQuery(provaTai.ProvaLegadoId));
            
            if (dadosDaAmostraTai == null)
                throw new NegocioException($"Os dados da amostra tai não foram cadastrados para a prova {provaTai.ProvaLegadoId}");
            
            var itensAmostra = (await mediator.Send(new ObterItensAmostraTaiQuery(dadosDaAmostraTai.MatrizId,
                dadosDaAmostraTai.ListaConfigItens.Select(x => x.TipoCurriculoGradeId).ToArray()))).ToList();
                
            if (itensAmostra == null || itensAmostra.Count < dadosDaAmostraTai.NumeroItensAmostra)
                throw new NegocioException($"A quantidade de itens configurados com TRI é menor do que o número de itens para a prova {provaTai.ProvaLegadoId}");

            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarCadernosProvaTai,
                new CadernoProvaTaiTratarDto(provaTai.ProvaId, provaTai.ProvaLegadoId, provaTai.Disciplina,
                    alunosProvaTaiSemCaderno, dadosDaAmostraTai, itensAmostra, provaTai.Ano)));

            return true;
        }
    }
}