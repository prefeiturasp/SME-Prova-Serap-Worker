using MediatR;
using SME.SERAp.Prova.Infra;
using System;
using System.Threading.Tasks;
using SME.SERAp.Prova.Dominio;

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
            
            if (existeProficiencia)
                return true;
            
            var ultimaProficiencia = await mediator.Send(new ObterUltimaProficienciaAlunoPorDisciplinaIdQuery(alunoProvaDto.AlunoRa, alunoProvaDto.DisciplinaId));
            var disciplinaId = alunoProvaDto.DisciplinaId ?? 0;

            await mediator.Send(new IncluirAlunoProvaProficienciaCommand(new AlunoProvaProficiencia
            {
                AlunoId = alunoProvaDto.AlunoId,
                Ra = alunoProvaDto.AlunoRa,
                ProvaId = alunoProvaDto.ProvaId,
                DisciplinaId = disciplinaId,
                Origem = ultimaProficiencia.origem,
                Tipo = Dominio.AlunoProvaProficienciaTipo.Inicial,
                Proficiencia = ultimaProficiencia.proficiencia,
                UltimaAtualizacao = alunoProvaDto.UltimaAtualizacao
            }));

            return true;
        }
    }
}
