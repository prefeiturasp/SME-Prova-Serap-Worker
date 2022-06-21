using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarCadernosProvaTaiUseCase : ITratarCadernosProvaTaiUseCase
    {

        private readonly IMediator mediator;

        public TratarCadernosProvaTaiUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var provaId = long.Parse(mensagemRabbit.Mensagem.ToString());
                var provaAtual = await mediator.Send(new ObterProvaDetalhesPorIdQuery(provaId));
                if (provaAtual == null)
                    throw new Exception($"Prova {provaId} não localizada.");

                if (provaAtual.AderirTodos)
                {
                    var dres = await mediator.Send(new ObterDresSerapQuery());
                    foreach (Dre dre in dres)
                    {
                        var ues = await mediator.Send(new ObterUesSerapPorProvaSerapEDreCodigoQuery(provaAtual.Id, dre.CodigoDre));
                        if (ues != null && ues.Any())
                        {
                            foreach (Ue ue in ues)
                            {
                                var turmasUe = await mediator.Send(new ObterTurmasPorCodigoUeEProvaSerapQuery(ue.CodigoUe, provaAtual.Id));
                                foreach (Turma turma in turmasUe)
                                {
                                    var alunos = await mediator.Send(new ObterAlunosPorTurmaIdQuery(turma.Id));
                                    foreach (Aluno aluno in alunos.Where(a => a.Ativo()))
                                    {
                                        await PublicarFilaTratarCadernoAluno(provaAtual.Id, aluno.Id);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    var alunosAdesao = await mediator.Send(new ObterAlunosAdesaoPorProvaIdQuery(provaAtual.Id));
                    if (alunosAdesao != null && alunosAdesao.Any())
                    {
                        foreach (Aluno aluno in alunosAdesao.Where(a => a.Ativo()))
                        {
                            await PublicarFilaTratarCadernoAluno(provaAtual.Id, aluno.Id);
                        }
                    }
                    else
                        throw new Exception($"Adesão da prova {provaId} não localizada.");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task PublicarFilaTratarCadernoAluno(long provaId, long alunoId)
        {
            var msg = new AlunoCadernoProvaTaiTratarDto(provaId, alunoId);
            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarCadernoAlunoProvaTai, msg));
        }
    }
}
