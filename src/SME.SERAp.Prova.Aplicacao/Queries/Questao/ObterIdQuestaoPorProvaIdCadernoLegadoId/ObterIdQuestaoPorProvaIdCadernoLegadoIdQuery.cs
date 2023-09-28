using FluentValidation;
using MediatR;
using System;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterIdQuestaoPorProvaIdCadernoLegadoIdQuery : IRequest<long>
    {
        public ObterIdQuestaoPorProvaIdCadernoLegadoIdQuery(long provaId, string caderno, long questaoLegadoId)
        {
            ProvaId = provaId;
            Caderno = caderno ?? throw new ArgumentNullException(nameof(caderno));
            QuestaoLegadoId = questaoLegadoId;
        }

        public long ProvaId { get;  }
        public string Caderno { get; }
        public long QuestaoLegadoId { get;  }
    }

    public class ObterIdQuestaoPorProvaIdCadernoLegadoIdQueryValidator : AbstractValidator<ObterIdQuestaoPorProvaIdCadernoLegadoIdQuery>
    {
        public ObterIdQuestaoPorProvaIdCadernoLegadoIdQueryValidator()
        {
            RuleFor(x => x.ProvaId)
                .GreaterThan(0)
                .WithMessage(" Informe o id da prova para verificar se existe  a questão para o aluno");

            RuleFor(x => x.Caderno)
                .NotEmpty()
                .WithMessage(" Informe o caderno para verificar se existe a questão para o aluno");

            RuleFor(x => x.QuestaoLegadoId)
                .NotEmpty()
                .WithMessage(" Informe o id legado da questão para verificar se existe a questão para o aluno");
        }
    }
}
