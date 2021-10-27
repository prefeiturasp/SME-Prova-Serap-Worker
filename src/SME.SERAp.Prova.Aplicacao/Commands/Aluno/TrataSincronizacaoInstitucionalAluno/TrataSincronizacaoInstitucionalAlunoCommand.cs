using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TrataSincronizacaoInstitucionalAlunoCommand : IRequest<bool>
    {
        public TrataSincronizacaoInstitucionalAlunoCommand(AlunoEolDto alunoEol, Aluno alunoSerap)
        {
            AlunoEol = alunoEol;
            AlunoSerap = alunoSerap;
        }

        public AlunoEolDto AlunoEol { get; set; }
        public Aluno AlunoSerap { get; set; }
    }
    public class TrataSincronizacaoInstitucionalAlunoCommandValidator : AbstractValidator<TrataSincronizacaoInstitucionalAlunoCommand>
    {
        public TrataSincronizacaoInstitucionalAlunoCommandValidator()
        {
            RuleFor(c => c.AlunoEol)
                .NotEmpty()
                .WithMessage("O AlunoEol deve ser informado.");
        }
    }
}
