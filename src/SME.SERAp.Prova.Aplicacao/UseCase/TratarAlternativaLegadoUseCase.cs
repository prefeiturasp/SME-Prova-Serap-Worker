using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarAlternativaLegadoLegadoUseCase : ITratarAlternativaLegadoUseCase
    {
        private readonly IMediator mediator;

        public TratarAlternativaLegadoLegadoUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var detalheAlternativaDto = mensagemRabbit.ObterObjetoMensagem<DetalheAlternativaDto>();

            var questao = await mediator.Send(new ObterQuestaoPorProvaQuestaoLegadoQuery(detalheAlternativaDto.ProvaId, detalheAlternativaDto.QuestaoId));

            if (questao == null)
                throw new Exception(
                    $"A questao {detalheAlternativaDto.QuestaoId} na prova {detalheAlternativaDto.ProvaId} não localizada!");

            foreach (var alternativaId in detalheAlternativaDto.AlternativasId)
            {
                var alternativa =
                await mediator.Send(
                    new ObterAlternativaDetalheLegadoPorIdQuery(
                        detalheAlternativaDto.QuestaoId, alternativaId));

                if (alternativa == null)
                    throw new Exception(
                        $"A Alternativa {alternativa.AlternativaLegadoId} não localizada!");

                var alternativas = new Alternativa(
                    alternativa.AlternativaLegadoId,
                    alternativa.Ordem,
                    alternativa.Numeracao,
                    alternativa.Descricao,
                    questao.Id,
                    alternativa.Correta);

                await mediator.Send(new AlternativaIncluirCommand(alternativas));
            }

            return true;
        }
    }
}