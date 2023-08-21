using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarAdesaoProvaUseCase : ITratarAdesaoProvaUseCase
    {
        private readonly IMediator mediator;
        private readonly IServicoLog servicoLog;

        public TratarAdesaoProvaUseCase(IMediator mediator, IServicoLog servicoLog)
        {
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
            this.mediator = mediator ?? throw new ArgumentException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var prova = mensagemRabbit.ObterObjetoMensagem<ProvaAdesaoDto>();

                if (prova is null)
                    return false;

                var provaSerap = await mediator.Send(new ObterProvaDetalhesPorProvaLegadoIdQuery(prova.ProvaLegadoId));
                
                await mediator.Send(new ExcluirAdesaoPorProvaIdCommand(prova.ProvaId));

                if (!prova.AderirTodos)
                {
                    var adesaoLegado = (await mediator.Send(new ObterAdesaoProvaLegadoPorIdQuery(prova.ProvaLegadoId))).ToList();

                    if (!adesaoLegado.Any())
                        return default;

                    var escolasAdesaoLegado = adesaoLegado.Select(e => e.UeCodigo).Distinct().ToList();

                    foreach (var escola in escolasAdesaoLegado)
                    {
                        var ue = await mediator.Send(new ObterUePorCodigoQuery(escola));
                        
                        var turmasAdesaoEscolaLegado = adesaoLegado.Where(e => e.UeCodigo == escola)
                            .Select(t => t.TurmaId).Distinct().ToList();
                        
                        foreach (var turmaIdLegado in turmasAdesaoEscolaLegado)
                        {
                            var turmaLegado = adesaoLegado.FirstOrDefault(a => a.TurmaId == turmaIdLegado);
                            
                            if (turmaLegado == null)
                                continue;
                           
                            var raAlunosAdesaoLegado = adesaoLegado
                                .Where(t => t.TurmaId == turmaIdLegado && t.UeCodigo == escola).Select(a => a.AlunoRa)
                                .Distinct().ToList();
                            
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

                                await EnviarParaFilaTratarAdesaoProvaAluno(adesaoParaInserir);
                            }
                            else
                            {
                                LogarAlunosNaoSincronizados(prova.ProvaLegadoId, ue.Id, turmaIdLegado, raAlunosAdesaoLegado.ToArray());
                            }
                        }
                    }

                    if (provaSerap.FormatoTai)
                        await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.AlunoProvaProficienciaPorProvaSync, provaSerap.Id));
                }

            }
            catch (Exception ex)
            {
                servicoLog.Registrar(ex);
                return false;
            }

            return true;
        }

        private void LogarAlunosNaoSincronizados(long provaLegadoId, long codigoUe, long codigoTurma, long[] raAlunos)
        {
            var msg = $"Alunos não sincronizados no serap estudantes para adesão da prova {provaLegadoId}. ";
            msg += $"CodigoUE: {codigoUe}, CodigoTurma: {codigoTurma}, RaAlunos: {string.Join(",", raAlunos)}.";
            servicoLog.Registrar(LogNivel.Informacao, msg);
        }

        private int ObterTipoTurno(TipoTurnoSerapLegado tipoTurnoSerapLegado)
        {
            return tipoTurnoSerapLegado switch
            {
                TipoTurnoSerapLegado.Integral => (int)TipoTurno.Integral,
                TipoTurnoSerapLegado.Vespertino => (int)TipoTurno.Vespertino,
                TipoTurnoSerapLegado.Noite => (int)TipoTurno.Noite,
                TipoTurnoSerapLegado.Intermediario => (int)TipoTurno.Intermediario,
                TipoTurnoSerapLegado.Tarde => (int)TipoTurno.Tarde,
                TipoTurnoSerapLegado.Manha => (int)TipoTurno.Manha,
                _ => throw new Exception($"Tipo turno não encontrado: {(int)tipoTurnoSerapLegado}")
            };
        }

        private async Task EnviarParaFilaTratarAdesaoProvaAluno(List<ProvaAdesao> adesaoParaInserir)
        {
            foreach (var adesao in adesaoParaInserir)
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarAdesaoProvaAluno, adesao));
        }
    }
}
