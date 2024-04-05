using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlterarExportacaoResultadoItemCommand : IRequest<bool>
    {
        public AlterarExportacaoResultadoItemCommand(ExportacaoResultadoItem item)
        {
            Item = item;
        }

        public ExportacaoResultadoItem Item { get; }
    }

    public class AlterarExportacaoResultadoItemCommandValidator : AbstractValidator<AlterarExportacaoResultadoItemCommand>
    {
        public AlterarExportacaoResultadoItemCommandValidator()
        {
            RuleFor(c => c.Item)
                .NotNull()
                .WithMessage("O item de exportação devem ser informados para a alteração");
        }
    }
}