using System;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarAlternativaProvaLegadoLegadoUseCase : ITratarAlternativaProvaLegadoLegadoUseCase
    {
        private readonly IMediator mediator;

        public TratarAlternativaProvaLegadoLegadoUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var detalheAlternativaDto = mensagemRabbit.ObterObjetoMensagem<DetalheAlternativaDto>();

            var alternativa =
                await mediator.Send(
                    new ObterDetalheAlternativarLegadoProvaPorProvaIdQuery(
                        detalheAlternativaDto.QuestaoId, detalheAlternativaDto.AlternativaId));

            if (alternativa == null)
                throw new Exception(
                    $"A Alternativa {alternativa.AlternativaLegadoId} não localizada!");

            var questao = await mediator.Send(new ObterQuestaoPorProvaLegadoQuery(detalheAlternativaDto.QuestaoId));

            if (questao == null)
                throw new Exception(
                    $"A Alternativa {alternativa.AlternativaLegadoId} não localizada!");

            var alternativas = new Alternativas(
                alternativa.Ordem,
                alternativa.Alternativa,
                alternativa.Descricao,
                questao.Id
            );

            await mediator.Send(new AlternativasParaIncluirCommand(alternativas));

            return true;
        }
    }
}