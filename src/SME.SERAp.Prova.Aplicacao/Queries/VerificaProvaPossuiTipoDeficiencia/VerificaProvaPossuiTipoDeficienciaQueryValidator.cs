using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao.Queries.VerificaProvaPossuiTipoDeficiencia
{
    public class VerificaProvaPossuiTipoDeficienciaQueryValidator : AbstractValidator<ObterAlunosResultadoProvaAdesaoManualQuery>
    {
        public VerificaProvaPossuiTipoDeficienciaQueryValidator()
        {
            RuleFor(x => x.ProvaId)
                .NotEmpty()
                .WithMessage("Código da prova é obrigatório");
        }
    }
}

