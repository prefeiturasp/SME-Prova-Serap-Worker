﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterObjResultadoPspQueryHandler : IRequestHandler<ObterObjResultadoPspQuery, ObjResultadoPspDto>
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
        private readonly IRepositorioParticipacaoDre repositorioParticipacaoDre;
        private readonly IRepositorioParticipacaoDreAreaConhecimento repositorioParticipacaoDreAreaConhecimento;
        private readonly IRepositorioParticipacaoSme repositorioParticipacaoSme;
        private readonly IRepositorioParticipacaoSmeAreaConhecimento repositorioParticipacaoSmeAreaConhecimento;
        private readonly IRepositorioResultadoCicloSme repositorioResultadoCicloSme;
        
        private ObjResultadoPspDto objResultado;

        public ObterObjResultadoPspQueryHandler(IRepositorioResultadoSme repositorioResultadoSme,
                                                IRepositorioResultadoDre repositorioResultadoDre,
                                                IRepositorioResultadoEscola repositorioResultadoEscola,
                                                IRepositorioResultadoTurma repositorioResultadoTurma,
                                                IRepositorioResultadoAluno repositorioResultadoAluno,
                                                IRepositorioParticipacaoTurma repositorioParticipacaoTurma,
                                                IRepositorioParticipacaoTurmaAreaConhecimento repositorioParticipacaoTurmaAreaConhecimento,
                                                IRepositorioParticipacaoUe repositorioParticipacaoUe,
                                                IRepositorioParticipacaoUeAreaConhecimento repositorioParticipacaoUeAreaConhecimento,
                                                IRepositorioParticipacaoDre repositorioParticipacaoDre,
                                                IRepositorioParticipacaoDreAreaConhecimento repositorioParticipacaoDreAreaConhecimento,
                                                IRepositorioParticipacaoSme repositorioParticipacaoSme,
                                                IRepositorioParticipacaoSmeAreaConhecimento repositorioParticipacaoSmeAreaConhecimento,
                                                IRepositorioResultadoCicloSme repositorioResultadoCicloSme)
        {
            this.repositorioResultadoSme = repositorioResultadoSme ?? throw new ArgumentNullException(nameof(repositorioResultadoSme));
            this.repositorioResultadoDre = repositorioResultadoDre ?? throw new ArgumentNullException(nameof(repositorioResultadoDre));
            this.repositorioResultadoEscola = repositorioResultadoEscola ?? throw new ArgumentNullException(nameof(repositorioResultadoEscola));
            this.repositorioResultadoTurma = repositorioResultadoTurma ?? throw new ArgumentNullException(nameof(repositorioResultadoTurma));
            this.repositorioResultadoAluno = repositorioResultadoAluno ?? throw new ArgumentNullException(nameof(repositorioResultadoAluno));
            this.repositorioParticipacaoTurma = repositorioParticipacaoTurma ?? throw new ArgumentException(nameof(repositorioParticipacaoTurma));
            this.repositorioParticipacaoTurmaAreaConhecimento = repositorioParticipacaoTurmaAreaConhecimento ?? throw new ArgumentException(nameof(repositorioParticipacaoTurmaAreaConhecimento));
            this.repositorioParticipacaoUe = repositorioParticipacaoUe ?? throw new ArgumentException(nameof(repositorioParticipacaoUe));
            this.repositorioParticipacaoUeAreaConhecimento = repositorioParticipacaoUeAreaConhecimento ?? throw new ArgumentException(nameof(repositorioParticipacaoUeAreaConhecimento));
            this.repositorioParticipacaoDre = repositorioParticipacaoDre ?? throw new ArgumentNullException(nameof(repositorioParticipacaoDre));
            this.repositorioParticipacaoDreAreaConhecimento = repositorioParticipacaoDreAreaConhecimento ?? throw new ArgumentNullException(nameof(repositorioParticipacaoDreAreaConhecimento));
            this.repositorioParticipacaoSme = repositorioParticipacaoSme ?? throw new ArgumentNullException(nameof(repositorioParticipacaoSme));
            this.repositorioParticipacaoSmeAreaConhecimento = repositorioParticipacaoSmeAreaConhecimento ?? throw new ArgumentNullException(nameof(repositorioParticipacaoSmeAreaConhecimento));
            this.repositorioResultadoCicloSme = repositorioResultadoCicloSme ?? throw new ArgumentNullException(nameof(repositorioResultadoCicloSme));
        }

        public async Task<ObjResultadoPspDto> Handle(ObterObjResultadoPspQuery request, CancellationToken cancellationToken)
        {
            objResultado = request.Resultado;
            var result = await ObterResultado();
            return result == null ? null : new ObjResultadoPspDto(objResultado.TipoResultado, result);
        }

        private async Task<object> ObterResultado()
        {
            return objResultado.TipoResultado switch
            {
                TipoResultadoPsp.ResultadoAluno => await ObterResultadoAluno(),
                TipoResultadoPsp.ResultadoSme => await ObterResultadoSme(),
                TipoResultadoPsp.ResultadoDre => await ObterResultadoDre(),
                TipoResultadoPsp.ResultadoEscola => await ObterResultadoEscola(),
                TipoResultadoPsp.ResultadoTurma => await ObterResultadoTurma(),
                TipoResultadoPsp.ResultadoParticipacaoTurma => await ObterParticipacaoTurma(),
                TipoResultadoPsp.ParticipacaoTurmaAreaConhecimento => await ObterParticipacaoTurmaAreaConhecimento(),
                TipoResultadoPsp.ResultadoParticipacaoUe => await ObterParticipacaoUe(),
                TipoResultadoPsp.ParticipacaoUeAreaConhecimento => await ObterParticipacaoUeAreaConhecimento(),
                TipoResultadoPsp.ParticipacaoDre => await ObterParticipacaoDre(),
                TipoResultadoPsp.ParticipacaoDreAreaConhecimento => await ObterParticipacaoDreAreaConhecimento(),
                TipoResultadoPsp.ParticipacaoSme => await ObterParticipacaoSme(),
                TipoResultadoPsp.ParticipacaoSmeAreaConhecimento => await ObterParticipacaoSmeAreaConhecimento(),
                TipoResultadoPsp.ResultadoCicloSme => await ObterResultadoCicloSme(),
                _ => null
            };
        }

        private async Task<ParticipacaoTurma> ObterParticipacaoTurmaAreaConhecimento()
        {
            var participacaoTurma = (ParticipacaoTurmaAreaConhecimentoDto)objResultado.Resultado;
            
            return await repositorioParticipacaoTurmaAreaConhecimento.
                ObterParticipacaoTurmaAreaConhecimento(participacaoTurma.Edicao,
                                                       participacaoTurma.uad_sigla,
                                                       participacaoTurma.AreaConhecimentoID,
                                                       participacaoTurma.esc_codigo,
                                                       participacaoTurma.AnoEscolar,
                                                       participacaoTurma.tur_codigo);
        }

        private async Task<ParticipacaoTurma> ObterParticipacaoTurma()
        {
            var participacaoTurma = (ParticipacaoTurmaDto)objResultado.Resultado;

            return await repositorioParticipacaoTurma.ObterParticipacaoTurma(participacaoTurma.Edicao,
                participacaoTurma.uad_sigla, participacaoTurma.esc_codigo, participacaoTurma.AnoEscolar,
                participacaoTurma.tur_codigo);
        }

        private async Task<ResultadoAluno> ObterResultadoAluno()
        {
            var resultadoBusca = (ResultadoAlunoDto)objResultado.Resultado;
            return await repositorioResultadoAluno.ObterProficienciaAluno(resultadoBusca.Edicao, resultadoBusca.alu_matricula, resultadoBusca.AreaConhecimentoID);
        }
        
        private async Task<ResultadoTurma> ObterResultadoTurma()
        {
            var resultadoBusca = (ResultadoTurmaDto)objResultado.Resultado;
            return await repositorioResultadoTurma.ObterResultadoTurma(resultadoBusca.Edicao, resultadoBusca.AreaConhecimentoID, resultadoBusca.EscCodigo, resultadoBusca.TurCodigo);
        }
        
        private async Task<ResultadoEscola> ObterResultadoEscola()
        {
            var resultadoBusca = (ResultadoEscolaDto)objResultado.Resultado;
            return await repositorioResultadoEscola.ObterResultadoEscola(resultadoBusca.Edicao, resultadoBusca.AreaConhecimentoID, resultadoBusca.EscCodigo, resultadoBusca.AnoEscolar);
        }
        
        private async Task<ResultadoDre> ObterResultadoDre()
        {
            var resultadoBusca = (ResultadoDreDto)objResultado.Resultado;
            return await repositorioResultadoDre.ObterResultadoDre(resultadoBusca.Edicao, resultadoBusca.AreaConhecimentoID, resultadoBusca.UadSigla, resultadoBusca.AnoEscolar);
        }

        private async Task<ResultadoSme> ObterResultadoSme()
        {
            var resultadoBusca = (ResultadoSmeDto)objResultado.Resultado;
            return await repositorioResultadoSme.ObterResultadoSme(resultadoBusca.Edicao, resultadoBusca.AreaConhecimentoID, resultadoBusca.AnoEscolar);
        }

        private async Task<ParticipacaoUe> ObterParticipacaoUeAreaConhecimento()
        {
            var participacao = (ParticipacaoUeAreaConhecimentoDto)objResultado.Resultado;
            return await repositorioParticipacaoUeAreaConhecimento.
                ObterParticipacaoUeAreaConhecimento(participacao.Edicao,
                                                       participacao.uad_sigla,
                                                       participacao.AreaConhecimentoID,
                                                       participacao.esc_codigo,
                                                       participacao.AnoEscolar);
        }

        private async Task<ParticipacaoUe> ObterParticipacaoUe()
        {
            var participacao = (ParticipacaoUeDto)objResultado.Resultado;
            return await repositorioParticipacaoUe.ObterParticipacaoUe(participacao.Edicao, participacao.uad_sigla, participacao.esc_codigo, participacao.AnoEscolar);
        }

        private async Task<ParticipacaoDre> ObterParticipacaoDre()
        {
            var participacao = (ParticipacaoDreDto)objResultado.Resultado;
            return await repositorioParticipacaoDre.ObterParticipacaoDre(participacao.Edicao, participacao.uad_sigla, participacao.AnoEscolar);
        }

        private async Task<ParticipacaoDreAreaConhecimento> ObterParticipacaoDreAreaConhecimento()
        {
            var participacao = (ParticipacaoDreAreaConhecimentoDto)objResultado.Resultado;
            return await repositorioParticipacaoDreAreaConhecimento.ObterParticipacaoDreAreaConhecimento(participacao.Edicao, participacao.AreaConhecimentoID, participacao.uad_sigla, participacao.AnoEscolar);
        }

        private async Task<ParticipacaoSme> ObterParticipacaoSme()
        {
            var participacao = (ParticipacaoSmeDto)objResultado.Resultado;
            return await repositorioParticipacaoSme.ObterParticipacaoSme(participacao.Edicao, participacao.AnoEscolar);
        }

        private async Task<ParticipacaoSmeAreaConhecimento> ObterParticipacaoSmeAreaConhecimento()
        {
            var participacao = (ParticipacaoSmeAreaConhecimentoDto)objResultado.Resultado;
            return await repositorioParticipacaoSmeAreaConhecimento.ObterParticipacaoSmeAreaConhecimento(participacao.Edicao, participacao.AreaConhecimentoID, participacao.AnoEscolar);
        }

        private async Task<ResultadoCicloSme> ObterResultadoCicloSme()
        {
            var resultado = (ResultadoCicloSme)objResultado.Resultado;
            return await repositorioResultadoCicloSme.ObterResultadoCicloSme(resultado.Edicao,
                resultado.AreaConhecimentoId, resultado.CicloId);
        }
    }
}
