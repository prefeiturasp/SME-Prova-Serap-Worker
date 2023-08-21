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

            var ultimaProficiencia = await mediator.Send(new ObterUltimaProficienciaAlunoPorDisciplinaIdQuery(
                alunoProvaDto.AlunoRa, alunoProvaDto.DisciplinaId, alunoProvaDto.TurmaId, alunoProvaDto.UeId,
                alunoProvaDto.Ano, alunoProvaDto.DreId, alunoProvaDto.UeCodigo));
            
            var disciplinaId = alunoProvaDto.DisciplinaId;

            await mediator.Send(new IncluirAlunoProvaProficienciaCommand(new AlunoProvaProficiencia
            {
                AlunoId = alunoProvaDto.AlunoId,
                Ra = alunoProvaDto.AlunoRa,
                ProvaId = alunoProvaDto.ProvaId,
                DisciplinaId = disciplinaId,
                Origem = ultimaProficiencia.origem,
                Tipo = AlunoProvaProficienciaTipo.Inicial,
                Proficiencia = ultimaProficiencia.proficiencia,
                UltimaAtualizacao = alunoProvaDto.UltimaAtualizacao,
                ErroMedida = decimal.Zero
            }));

            return true;
        }
    }
}
