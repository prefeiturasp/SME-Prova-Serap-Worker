using MediatR;
using Newtonsoft.Json;
using Sentry;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExecutarSincronizacaoInstitucionalAlunoTratarUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalAlunoTratarUseCase
    {
        public ExecutarSincronizacaoInstitucionalAlunoTratarUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var alunoEol = mensagemRabbit.ObterObjetoMensagem<AlunoEolDto>();

            if (alunoEol == null)
                throw new NegocioException("Não foi possível localizar o Aluno.");

            var alunoSerap = await mediator.Send(new ObterAlunoPorCodigoQuery(alunoEol.CodigoAluno));

            try
            {
                await mediator.Send(new TrataSincronizacaoInstitucionalAlunoCommand(alunoEol, alunoSerap));
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }

            return true;
        }
    }
}
