using FluentValidation;
using MediatR;

namespace SME.SERAp.Prova.Aplicacao.Queries
{
    public class ObterAlternativaArquivoIdPorAlternativaIdArquivoIdQuery : IRequest<long>
    {
        public ObterAlternativaArquivoIdPorAlternativaIdArquivoIdQuery(long alternativaId, long arquivoId)
        {
            AlternativaId = alternativaId;
            ArquivoId = arquivoId;
        }

        public long AlternativaId { get; }
        public long ArquivoId { get; }
    }

    public class ObterAlternativaArquivoIdPorAlternativaIdArquivoIdQueryValidator : AbstractValidator<ObterAlternativaArquivoIdPorAlternativaIdArquivoIdQuery>
    {
        public ObterAlternativaArquivoIdPorAlternativaIdArquivoIdQueryValidator()
        {
            RuleFor(c => c.AlternativaId)
                .GreaterThan(0)
                .WithMessage("O id da alternativa deve ser informado para obter o id do arquivo da alternativa.");
            
            RuleFor(c => c.ArquivoId)
                .GreaterThan(0)
                .WithMessage("O id do arquivo deve ser informado para obter o id do arquivo da alternativa.");            
        }
    }
}