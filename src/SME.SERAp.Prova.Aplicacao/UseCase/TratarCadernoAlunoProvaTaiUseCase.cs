using MediatR;
using SME.SERAp.Prova.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarCadernoAlunoProvaTaiUseCase : ITratarCadernoAlunoProvaTaiUseCase
    {

        private readonly IMediator mediator;

        public TratarCadernoAlunoProvaTaiUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {

                var alunoProva = mensagemRabbit.ObterObjetoMensagem<AlunoCadernoProvaTaiTratarDto>();
                var provaAtual = await mediator.Send(new ObterProvaDetalhesPorIdQuery(alunoProva.ProvaId));
                if (provaAtual == null)
                    throw new Exception($"Prova {alunoProva.ProvaId} não localizada.");

                var dadosDaAmostraTai = await mediator.Send(new ObterDadosAmostraProvaTaiQuery(provaAtual.LegadoId));
                var itens = await mediator.Send(new ObterItensAmostraTaiQuery(dadosDaAmostraTai.MatrizId, dadosDaAmostraTai.ListaConfigItens.Select(x => x.TipoCurriculoGradeId).ToArray()));
                var proficienciaAluno = await mediator.Send(new ObterProficienciaAlunoPorProvaIdQuery(provaAtual.Id, alunoProva.AlunoId));
                //var 

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
