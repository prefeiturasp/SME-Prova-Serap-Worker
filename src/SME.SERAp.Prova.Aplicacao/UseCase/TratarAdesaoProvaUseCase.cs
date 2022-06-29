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

                var provaSerap = await mediator.Send(new ObterProvaDetalhesPorIdQuery(prova.ProvaLegadoId));
                await mediator.Send(new ExcluirAdesaoPorProvaIdCommand(prova.ProvaId));

                if (!prova.AderirTodos)
                {
                    var adesaoLegado = await mediator.Send(new ObterAdesaoProvaLegadoPorIdQuery(prova.ProvaLegadoId));

                    if (adesaoLegado is null || !adesaoLegado.Any())
                        return default;

                    var escolasAdesaoLegado = adesaoLegado.Select(e => e.UeCodigo).Distinct().ToList();
                    foreach (string escola in escolasAdesaoLegado)
                    {
                        var ue = await mediator.Send(new ObterUePorCodigoQuery(escola));
                        var turmasAdesaoEscolaLegado = adesaoLegado.Where(e => e.UeCodigo == escola).Select(t => t.TurmaId).Distinct().ToList();
                        foreach (long turma in turmasAdesaoEscolaLegado)
                        {
                            var turmaLegado = adesaoLegado.Where(a => a.TurmaId == turma).FirstOrDefault();
                            var raAlunosAdesaoLegado = adesaoLegado.Where(t => t.TurmaId == turma && t.UeCodigo == escola).Select(a => a.AlunoRa).Distinct().ToList();
                            if (raAlunosAdesaoLegado.Any())
                            {
                                var adesaoParaInserir = raAlunosAdesaoLegado.Select(a =>
                                        new ProvaAdesao(prova.ProvaId,
                                                        ue.Id,
                                                        a,
                                                        turmaLegado.AnoTurma.ToString(),
                                                        turmaLegado.TipoTurma,
                                                        (int)provaSerap.Modalidade,
                                                        ObterTipoTurno((TipoTurnoSerapLegado)turmaLegado.TipoTurno)))
                                                        .ToList();

                                await mediator.Send(new InserirListaProvaAdesaoCommand(adesaoParaInserir));
                            }
                            else
                            {
                                LogarAlunosNaoSincronizados(prova.ProvaLegadoId, ue.Id, turma, raAlunosAdesaoLegado.ToArray());
                            }
                        }
                    }

                    if (provaSerap.FormatoTai)
                        await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.AlunoProvaProficienciaAsync, provaSerap.Id));
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

        public int ObterTipoTurno(TipoTurnoSerapLegado tipoTurnoSerapLegado)
        {
            switch (tipoTurnoSerapLegado)
            {
                case TipoTurnoSerapLegado.Integral:
                    return (int)TipoTurno.Integral;
                case TipoTurnoSerapLegado.Vespertino:
                    return (int)TipoTurno.Vespertino;
                case TipoTurnoSerapLegado.Noite:
                    return (int)TipoTurno.Noite;
                case TipoTurnoSerapLegado.Intermediario:
                    return (int)TipoTurno.Intermediario;
                case TipoTurnoSerapLegado.Tarde:
                    return (int)TipoTurno.Tarde;
                case TipoTurnoSerapLegado.Manha:
                    return (int)TipoTurno.Manha;
                default:
                    throw new Exception($"Tipo turno não encontrado: {(int)tipoTurnoSerapLegado}");
            }
        }
    }
}
