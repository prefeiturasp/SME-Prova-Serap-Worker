using MediatR;
using SME.SERAp.Prova.Aplicacao.Commands;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Aplicacao.Queries.ObterAlunoProvaSpProficiencia;
using SME.SERAp.Prova.Dominio.Entidades;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.UseCase
{
    public class TratarAlunoProvaSpProficienciaUseCase : AbstractUseCase, ITratarAlunoProvaSpProficienciaUseCase
    {
        private readonly IServicoLog servicoLog;
        public TratarAlunoProvaSpProficienciaUseCase(IMediator mediator, IServicoLog servicoLog) : base(mediator)
        {
            this.servicoLog = servicoLog;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var alunoProvaSpProficiencia = mensagemRabbit.ObterObjetoMensagem<AlunoProvaSpProficiencia>();
                if (alunoProvaSpProficiencia is null)
                    throw new Exception("Mensagem inválida.");

                var alunoProvaSpProficienciaExistente = await mediator
                    .Send(new ObterAlunoProvaSpProficienciaQuery(alunoProvaSpProficiencia.AnoLetivo, alunoProvaSpProficiencia.DisciplinaId, alunoProvaSpProficiencia.AlunoRa));

                if (alunoProvaSpProficienciaExistente != null)
                    await mediator.Send(new ExcluirAlunoProvaSpProficienciaCommand(alunoProvaSpProficiencia.AnoLetivo, alunoProvaSpProficiencia.DisciplinaId, alunoProvaSpProficiencia.AlunoRa));

                await mediator.Send(new InserirAlunoProvaSpProficienciaCommand(alunoProvaSpProficiencia));
                return true;
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(ex);
                return false;
            }
        }
    }
}
