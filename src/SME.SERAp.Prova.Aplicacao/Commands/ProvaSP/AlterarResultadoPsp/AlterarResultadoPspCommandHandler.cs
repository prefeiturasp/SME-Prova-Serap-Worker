﻿using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlterarResultadoPspCommandHandler : IRequestHandler<AlterarResultadoPspCommand, bool>
    {

        private readonly IRepositorioResultadoSme repositorioResultadoSme;
        private readonly IRepositorioResultadoDre repositorioResultadoDre;
        private readonly IRepositorioResultadoEscola repositorioResultadoEscola;
        private readonly IRepositorioResultadoTurma repositorioResultadoTurma;
        private readonly IRepositorioResultadoAluno repositorioResultadoAluno;
        private readonly IRepositorioParticipacaoTurma repositorioParticipacaoTurma;
        private readonly IRepositorioParticipacaoTurmaAreaConhecimento repositorioParticipacaoTurmaAreaConhecimento;
        private readonly IRepositorioParticipacaoUe repositorioParticipacaoUe;
        private readonly IRepositorioParticipacaoUeAreaConhecimento repositorioParticipacaoUeAreaConhecimento;

        private ObjResultadoPspDto ObjResultado;

        public AlterarResultadoPspCommandHandler(IRepositorioResultadoSme repositorioResultadoSme,
                                                 IRepositorioResultadoDre repositorioResultadoDre,
                                                 IRepositorioResultadoEscola repositorioResultadoEscola,
                                                 IRepositorioResultadoTurma repositorioResultadoTurma,
                                                 IRepositorioResultadoAluno repositorioResultadoAluno,
                                                 IRepositorioParticipacaoTurma repositorioParticipacaoTurma,
                                                 IRepositorioParticipacaoTurmaAreaConhecimento repositorioParticipacaoTurmaAreaConhecimento,
                                                 IRepositorioParticipacaoUe repositorioParticipacaoUe,
                                                 IRepositorioParticipacaoUeAreaConhecimento repositorioParticipacaoUeAreaConhecimento)
        {
            this.repositorioResultadoSme = repositorioResultadoSme ?? throw new System.ArgumentNullException(nameof(repositorioResultadoSme));
            this.repositorioResultadoDre = repositorioResultadoDre ?? throw new System.ArgumentNullException(nameof(repositorioResultadoDre));
            this.repositorioResultadoEscola = repositorioResultadoEscola ?? throw new System.ArgumentNullException(nameof(repositorioResultadoEscola));
            this.repositorioResultadoTurma = repositorioResultadoTurma ?? throw new System.ArgumentNullException(nameof(repositorioResultadoTurma));
            this.repositorioResultadoAluno = repositorioResultadoAluno ?? throw new System.ArgumentNullException(nameof(repositorioResultadoAluno));
            this.repositorioParticipacaoTurma = repositorioParticipacaoTurma ?? throw new System.ArgumentNullException(nameof(repositorioParticipacaoTurma));
            this.repositorioParticipacaoTurmaAreaConhecimento = repositorioParticipacaoTurmaAreaConhecimento ?? throw new System.ArgumentNullException(nameof(repositorioParticipacaoTurmaAreaConhecimento));
            this.repositorioParticipacaoUe = repositorioParticipacaoUe ?? throw new System.ArgumentNullException(nameof(repositorioParticipacaoUe));
            this.repositorioParticipacaoUeAreaConhecimento = repositorioParticipacaoUeAreaConhecimento ?? throw new System.ArgumentNullException(nameof(repositorioParticipacaoUeAreaConhecimento));
        }

        public async Task<bool> Handle(AlterarResultadoPspCommand request, CancellationToken cancellationToken)
        {
            ObjResultado = request.Resultado;
            switch (request.Resultado.TipoResultado)
            {
                case TipoResultadoPsp.ResultadoAluno:
                    return await AlterarResultadoAluno();
                case TipoResultadoPsp.ResultadoSme:
                    return await AlterarResultadoSME();
                case TipoResultadoPsp.ResultadoDre:
                    return await AlterarResultadoDre();
                case TipoResultadoPsp.ResultadoEscola:
                    return await AlterarResultadoEscola();
                case TipoResultadoPsp.ResultadoTurma:
                    return await AlterarResultadoTurma();
                case TipoResultadoPsp.ResultadoParticipacaoTurma:
                    return await AlterarParticipacaoTurma();
                case TipoResultadoPsp.ParticipacaoTurmaAreaConhecimento:
                    return await AlterarParticipacaoTurmaAreaConhecimento();
                case TipoResultadoPsp.ResultadoParticipacaoUe:
                    return await AlterarParticipacaoUe();
                case TipoResultadoPsp.ParticipacaoUeAreaConhecimento:
                    return await AlterarParticipacaoUeAreaConhecimento();
                default:
                    return false;
            }
        }

        private async Task<bool> AlterarResultadoAluno()
        {
            var resultadoInserir = (ResultadoAluno)ObjResultado.Resultado;
            var result = await repositorioResultadoAluno.AlterarAsync(resultadoInserir);
            return result > 0;
        }
        private async Task<bool> AlterarResultadoTurma()
        {
            var resultadoInserir = (ResultadoTurma)ObjResultado.Resultado;
            var result = await repositorioResultadoTurma.AlterarAsync(resultadoInserir);
            return result > 0;
        }
        private async Task<bool> AlterarResultadoEscola()
        {
            var resultadoInserir = (ResultadoEscola)ObjResultado.Resultado;
            var result = await repositorioResultadoEscola.AlterarAsync(resultadoInserir);
            return result > 0;
        }
        private async Task<bool> AlterarResultadoDre()
        {
            var resultadoInserir = (ResultadoDre)ObjResultado.Resultado;
            var result = await repositorioResultadoDre.AlterarAsync(resultadoInserir);
            return result > 0;
        }

        private async Task<bool> AlterarResultadoSME()
        {
            var resultadoInserir = (ResultadoSme)ObjResultado.Resultado;
            var result = await repositorioResultadoSme.AlterarAsync(resultadoInserir);
            return result > 0;
        }

        private async Task<bool> AlterarParticipacaoTurma()
        {
            var participacaoTurma = (ParticipacaoTurma)ObjResultado.Resultado;
            var result = await repositorioParticipacaoTurma.AlterarAsync(participacaoTurma);
            return result > 0;
        }

        private async Task<bool> AlterarParticipacaoTurmaAreaConhecimento()
        {
            var participacaoTurma = (ParticipacaoTurmaAreaConhecimento)ObjResultado.Resultado;
            var result = await repositorioParticipacaoTurmaAreaConhecimento.AlterarAsync(participacaoTurma);
            return result > 0;
        }

        private async Task<bool> AlterarParticipacaoUe()
        {
            var participacaoTurma = (ParticipacaoUe)ObjResultado.Resultado;
            var result = await repositorioParticipacaoUe.AlterarAsync(participacaoTurma);
            return result > 0;
        }

        private async Task<bool> AlterarParticipacaoUeAreaConhecimento()
        {
            var participacaoTurma = (ParticipacaoUeAreaConhecimento)ObjResultado.Resultado;
            var result = await repositorioParticipacaoUeAreaConhecimento.AlterarAsync(participacaoTurma);
            return result > 0;
        }
    }
}
