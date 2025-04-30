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
    public class BuscarAlunosComRespostasSemQuestoesUseCase : IBuscarAlunosComRespostasSemQuestoesUseCase
    {
        private readonly IMediator mediator;

        public BuscarAlunosComRespostasSemQuestoesUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var inicio = new DateTime(2025, 4, 22, 0, 0, 0);
                var fim = new DateTime(2025, 4, 30, 23, 59, 59);
                long? alunoRaValidacao = null;

                var alunosProvas = await ObterAlunoComRespostasSemQuestoes(inicio, fim, alunoRaValidacao);
                if (alunosProvas?.Count() > 0)
                {
                    foreach (var alunoProva in alunosProvas)
                    {
                        await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarAlunosComRespostasSemQuestoes, alunoProva));
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        private async Task<IEnumerable<AlunoProvaRespostaSemPerguntaDto>> ObterAlunoComRespostasSemQuestoes(DateTime inicio, DateTime fim, long? alunoRa)
        {
            return await mediator.Send(new ObterAlunoComRespostasSemQuestoesQuery(inicio, fim, alunoRa));
        }
    }
}
