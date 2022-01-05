using MediatR;
using Sentry;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarAdesaoProvaUseCase : ITratarAdesaoProvaUseCase
    {
        private readonly IMediator mediator;

        public TratarAdesaoProvaUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var prova = mensagemRabbit.ObterObjetoMensagem<Dominio.Prova>();

                if (prova is null)
                    return default;

                await mediator.Send(new ExcluirAdesaoPorProvaIdCommand(prova.Id));

                if (!prova.AderirTodos)
                {
                    var adesaoLegado = await mediator.Send(new ObterAdesaoProvaLegadoPorIdQuery(prova.LegadoId));

                    if (adesaoLegado is null || !adesaoLegado.Any())
                        return default;

                    var escolasAdesaoLegado = adesaoLegado.Select(e => e.UeId).Distinct().ToList();

                    foreach (long escola in escolasAdesaoLegado)
                    {
                        var turmasAdesaoEscolaLegado = adesaoLegado.Where(e => e.UeId == escola).Select(t => t.TurmaId).Distinct().ToList();
                        foreach (long turma in turmasAdesaoEscolaLegado)
                        {
                            var alunosAdesaoLegado = adesaoLegado.Where(t => t.TurmaId == turma && t.UeId == escola).Select(a => a.AlunoId).Distinct().ToList();
                            var dadosAlunos = await mediator.Send(new ObterDadosAlunosParaAdesaoPorRaQuery(alunosAdesaoLegado.ToArray()));
                            var adesaoParaInserir = dadosAlunos.Select(a => new ProvaAdesao(prova.Id, a.UeId, a.TurmaId, a.AlunoId)).ToList();
                            await mediator.Send(new InserirListaProvaAdesaoCommand(adesaoParaInserir));
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return false;
            }
            return true;
        }
    }
}
