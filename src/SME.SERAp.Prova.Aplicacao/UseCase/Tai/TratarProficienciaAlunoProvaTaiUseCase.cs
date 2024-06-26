﻿using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProficienciaAlunoProvaTaiUseCase : AbstractUseCase, ITratarProficienciaAlunoProvaTaiUseCase
    {
        public TratarProficienciaAlunoProvaTaiUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var proficienciaAlunoProvaTai = mensagemRabbit.ObterObjetoMensagem<ProficienciaAlunoProvaTaiDto>();
            if (proficienciaAlunoProvaTai == null)
                throw new NegocioException("É preciso informar os dados de proficiencia.");

            await Validacoes(proficienciaAlunoProvaTai);

            var proficienciasTiposParaAlterar = new[] { AlunoProvaProficienciaTipo.Inicial, AlunoProvaProficienciaTipo.Final };
            AlunoProvaProficiencia proficiencia = null;

            if (proficienciasTiposParaAlterar.Contains((AlunoProvaProficienciaTipo)proficienciaAlunoProvaTai.Tipo))
            {
                proficiencia = await mediator.Send(new ObterProficienciaAlunoQuery(proficienciaAlunoProvaTai.ProvaId,
                    proficienciaAlunoProvaTai.AlunoId,
                    (AlunoProvaProficienciaTipo)proficienciaAlunoProvaTai.Tipo,
                    (AlunoProvaProficienciaOrigem)proficienciaAlunoProvaTai.Origem));
            }

            if (proficiencia == null)
            {
                await mediator.Send(new IncluirAlunoProvaProficienciaCommand(new AlunoProvaProficiencia
                {
                    AlunoId = proficienciaAlunoProvaTai.AlunoId,
                    Ra = proficienciaAlunoProvaTai.AlunoRa,
                    ProvaId = proficienciaAlunoProvaTai.ProvaId,
                    DisciplinaId = proficienciaAlunoProvaTai.DisciplinaId,
                    Origem = (AlunoProvaProficienciaOrigem)proficienciaAlunoProvaTai.Origem,
                    Tipo = (AlunoProvaProficienciaTipo)proficienciaAlunoProvaTai.Tipo,
                    Proficiencia = proficienciaAlunoProvaTai.Proficiencia,
                    UltimaAtualizacao = DateTime.Now,
                    ErroMedida = proficienciaAlunoProvaTai.ErroMedida
                }));
            }
            else
            {
                proficiencia.Proficiencia = proficienciaAlunoProvaTai.Proficiencia;
                proficiencia.UltimaAtualizacao = DateTime.Now;
                proficiencia.ErroMedida = proficienciaAlunoProvaTai.ErroMedida;
                await mediator.Send(new AtualizarValorProficienciaAlunoCommand(proficiencia));
            }

            return true;
        }

        private async Task Validacoes(ProficienciaAlunoProvaTaiDto proficienciaAlunoProvaTai)
        {
            var prova = await mediator.Send(new ObterProvaPorIdQuery(proficienciaAlunoProvaTai.ProvaId));
            
            if (prova == null)
                throw new NegocioException("Prova não encontrada.");

            if (!prova.FormatoTai)
                throw new NegocioException("Prova não é formato TAI.");
        }
    }
}
