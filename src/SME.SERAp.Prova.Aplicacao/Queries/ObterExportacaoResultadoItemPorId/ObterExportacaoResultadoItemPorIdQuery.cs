using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterExportacaoResultadoItemPorIdQuery : IRequest<ExportacaoResultadoItem>
    {
        public ObterExportacaoResultadoItemPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ObterExportacaoResultadoItemPorIdQueryValidator : AbstractValidator<ObterExportacaoResultadoItemPorIdQuery>
    {
        public ObterExportacaoResultadoItemPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0)
                .WithMessage("O Id do processo deve ser informado.");
        }
    }
}