using System;
using MediatR;
using SME.SERAp.Prova.Infra;
using System.Threading.Tasks;
using SME.SERAp.Prova.Dominio;

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
            var alternativa = mensagemRabbit.ObterObjetoMensagem<AlternativasProvaIdDto>();

            var provaLegado = await mediator.Send(new ObterProvaLegadoDetalhesPorIdQuery(alternativa.ProvaId));

            if (provaLegado == null)
                throw new System.Exception($"Prova {alternativa.ProvaId} não localizada!");


            var alternativas = new Alternativas(
                alternativa.Descricao,
                alternativa.Alternativa,
                alternativa.ItemId,
                alternativa.ProvaId, alternativa.Id, 
                alternativa.OrdemProva, 
                alternativa.OrdemAlternativa, DateTime.Now,
                DateTime.Now
            );
            await mediator.Send(new AlternativasParaIncluirCommand(alternativas));

            return true;
        }
    }
}