using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterExportacaoResultadoItemPorProcessoIdDreCodigoQuery : IRequest<ExportacaoResultadoItem>
    {
        public ObterExportacaoResultadoItemPorProcessoIdDreCodigoQuery(long processoId, string dreCodigo)
        {
            ProcessoId = processoId;
            DreCodigo = dreCodigo;
        }

        public long ProcessoId { get; }
        public string DreCodigo { get; }
    }

    public class ObterExportacaoResultadoItemPorProcessoIdDreCodigoQueryValidator : AbstractValidator<ObterExportacaoResultadoItemPorProcessoIdDreCodigoQuery>
    {
        public ObterExportacaoResultadoItemPorProcessoIdDreCodigoQueryValidator()
        {
            RuleFor(c => c.ProcessoId)
                .GreaterThan(0)
                .WithMessage("O Id do processo deve ser informado.");

            RuleFor(c => c.DreCodigo)
                .NotEmpty()
                .NotNull()
                .WithMessage("O código da DRE deve ser informado.");
        }
    }
}