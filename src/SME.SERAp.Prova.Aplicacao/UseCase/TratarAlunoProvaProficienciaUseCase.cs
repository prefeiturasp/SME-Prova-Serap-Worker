using MediatR;
using SME.SERAp.Prova.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarAlunoProvaProficienciaUseCase : ITratarAlunoProvaProficienciaUseCase
    {
        private readonly IMediator mediator;

        public TratarAlunoProvaProficienciaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var alunoProvaDto = mensagemRabbit.ObterObjetoMensagem<AlunoProvaDto>();

            var existeProficiencia = await mediator.Send(new VerificaAlunoProvaProficienciaExisteQuery(alunoProvaDto.AlunoId, alunoProvaDto.ProvaId));
            if (!existeProficiencia)
            {

                var aluno = await mediator.Send(new ObterAlunoPorIdQuery(alunoProvaDto.AlunoId));
                var turma = await mediator.Send(new ObterTurmaSerapPorIdQuery(aluno.TurmaId));
                var ue = await mediator.Send(new ObterUePorIdQuery(turma.UeId));
                var dre = await mediator.Send(new ObterDrePorIdQuery(ue.DreId));

                var ultimaProficiencia = await mediator.Send(new ObterUltimaProficienciaAlunoPorDisciplinaIdQuery(alunoProvaDto.AlunoId, alunoProvaDto.DisciplinaId));

                await mediator.Send(new IncluirAlunoProvaProficienciaCommand(new Dominio.AlunoProvaProficiencia()
                {
                    AlunoId = alunoProvaDto.AlunoId,
                    Ra = alunoProvaDto.AlunoRa,
                    ProvaId = alunoProvaDto.ProvaId,
                    DisciplinaId = alunoProvaDto.DisciplinaId.GetValueOrDefault() > 0 ? (long)alunoProvaDto.DisciplinaId.Value : (long?)null,
                    Origem = Dominio.AlunoProvaProficienciaOrigem.TAI_estudante,
                    Tipo = Dominio.AlunoProvaProficienciaTipo.Inicial,
                    Proficiencia = ultimaProficiencia,
                    UltimaAtualizacao = alunoProvaDto.UltimaAtualizacao
                }));
            }

            return true;
        }
    }
}
