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
                var prova = mensagemRabbit.ObterObjetoMensagem<ProvaAdesaoDto>();

                if (prova is null)
                    return default;

                await mediator.Send(new ExcluirAdesaoPorProvaIdCommand(prova.ProvaId));

                if (!prova.AderirTodos)
                {
                    var adesaoLegado = await mediator.Send(new ObterAdesaoProvaLegadoPorIdQuery(prova.ProvaLegadoId));

                    if (adesaoLegado is null || !adesaoLegado.Any())
                        return default;

                    var escolasAdesaoLegado = adesaoLegado.Select(e => e.UeId).Distinct().ToList();

                    foreach (long escola in escolasAdesaoLegado)
                    {
                        var turmasAdesaoEscolaLegado = adesaoLegado.Where(e => e.UeId == escola).Select(t => t.TurmaId).Distinct().ToList();
                        foreach (long turma in turmasAdesaoEscolaLegado)
                        {
                            var raAlunosAdesaoLegado = adesaoLegado.Where(t => t.TurmaId == turma && t.UeId == escola).Select(a => a.AlunoRa).Distinct().ToList();
                            var dadosAlunos = await mediator.Send(new ObterDadosAlunosParaAdesaoPorRaQuery(raAlunosAdesaoLegado.ToArray()));
                            if (dadosAlunos.Any())
                            {
                                var adesaoParaInserir = dadosAlunos.Select(a => new ProvaAdesao(prova.ProvaId, a.UeId, a.TurmaId, a.AlunoId)).ToList();
                                await mediator.Send(new InserirListaProvaAdesaoCommand(adesaoParaInserir));
                            }
                            else
                            {
                                LogarAlunosNaoSincronizados(prova.ProvaLegadoId, escola, turma, raAlunosAdesaoLegado.ToArray());
                            }                            
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

        private void LogarAlunosNaoSincronizados(long provaLegadoId, long codigoUe, long codigoTurma, long[] raAlunos)
        {
            string msg = $"Alunos não sincronizados no serap estudantes para adesão da prova {provaLegadoId}. ";
            msg += $"CodigoUE: {codigoUe}, CodigoTurma: {codigoTurma}, RaAlunos: {string.Join(",", raAlunos)}.";
            SentrySdk.CaptureMessage(msg, SentryLevel.Warning);
        }
    }
}
