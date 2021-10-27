using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TrataSincronizacaoInstitucionalAlunoCommandHandler : IRequestHandler<TrataSincronizacaoInstitucionalAlunoCommand, bool>
    {
        private readonly IRepositorioAluno repositorioAluno;

        public TrataSincronizacaoInstitucionalAlunoCommandHandler(IRepositorioAluno repositorioAluno)
        {
            this.repositorioAluno = repositorioAluno ?? throw new ArgumentNullException(nameof(repositorioAluno));
        }

        public async Task<bool> Handle(TrataSincronizacaoInstitucionalAlunoCommand request, CancellationToken cancellationToken)
        {
            if (request.AlunoSerap == null)
            {
                var idAluno = await repositorioAluno.IncluirAsync(
                    new Aluno(request.AlunoEol.Nome, request.AlunoEol.TurmaSerapId, request.AlunoEol.CodigoAluno, request.AlunoEol.SituacaoAluno)
                );

                if (idAluno <= 0)
                    throw new NegocioException($"Erro ao incluir novo Aluno {request.AlunoEol.CodigoAluno}");
            }
            else
            {
                if (VerificaSeTemAlteracao(request.AlunoEol, request.AlunoSerap))
                {
                    var alunoParaAtualizar = new Aluno()
                    {
                        TurmaId = request.AlunoEol.TurmaSerapId,
                        Nome = request.AlunoEol.Nome,
                        RA = request.AlunoSerap.RA,
                        Id = request.AlunoSerap.Id,                        
                    };
                    await repositorioAluno.UpdateAsync(alunoParaAtualizar);
                }
                else
                {
                    return true;
                }
            }
            return true;
        }
        private bool VerificaSeTemAlteracao(AlunoEolDto alunoEol, Aluno alunoSerap)
        {
            if (alunoEol.TurmaSerapId != alunoSerap.TurmaId)
            {
                alunoSerap.TurmaId = alunoEol.TurmaSerapId;
                return true;
            }
                
            if (alunoEol.SituacaoAluno != alunoSerap.Situacao)
            {
                alunoSerap.Situacao = alunoEol.SituacaoAluno;
                return true;
            }
            return false;
        }
    }
}
