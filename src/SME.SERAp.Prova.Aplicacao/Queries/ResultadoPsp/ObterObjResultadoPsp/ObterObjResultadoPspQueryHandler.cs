using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Dominio.Entidades.ProficienciaPsp;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using System.Threading;
using System.Threading.Tasks;

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
        private readonly IRepositorioResultadoCicloDre repositorioResultadoCicloDre;
        private ObjResultadoPspDto ObjResultado;

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
                                                IRepositorioResultadoCicloDre repositorioResultadoCicloDre)
        {
            this.repositorioResultadoSme = repositorioResultadoSme ?? throw new System.ArgumentNullException(nameof(repositorioResultadoSme));
            this.repositorioResultadoDre = repositorioResultadoDre ?? throw new System.ArgumentNullException(nameof(repositorioResultadoDre));
            this.repositorioResultadoEscola = repositorioResultadoEscola ?? throw new System.ArgumentNullException(nameof(repositorioResultadoEscola));
            this.repositorioResultadoTurma = repositorioResultadoTurma ?? throw new System.ArgumentNullException(nameof(repositorioResultadoTurma));
            this.repositorioResultadoAluno = repositorioResultadoAluno ?? throw new System.ArgumentNullException(nameof(repositorioResultadoAluno));
            this.repositorioParticipacaoTurma = repositorioParticipacaoTurma ?? throw new System.ArgumentException(nameof(repositorioParticipacaoTurma));
            this.repositorioParticipacaoTurmaAreaConhecimento = repositorioParticipacaoTurmaAreaConhecimento ?? throw new System.ArgumentException(nameof(repositorioParticipacaoTurmaAreaConhecimento));
            this.repositorioParticipacaoUe = repositorioParticipacaoUe ?? throw new System.ArgumentException(nameof(repositorioParticipacaoUe));
            this.repositorioParticipacaoUeAreaConhecimento = repositorioParticipacaoUeAreaConhecimento ?? throw new System.ArgumentException(nameof(repositorioParticipacaoUeAreaConhecimento));
            this.repositorioParticipacaoDre = repositorioParticipacaoDre ?? throw new System.ArgumentNullException(nameof(repositorioParticipacaoDre));
            this.repositorioParticipacaoDreAreaConhecimento = repositorioParticipacaoDreAreaConhecimento ?? throw new System.ArgumentNullException(nameof(repositorioParticipacaoDreAreaConhecimento));
            this.repositorioParticipacaoSme = repositorioParticipacaoSme ?? throw new System.ArgumentNullException(nameof(repositorioParticipacaoSme));
            this.repositorioParticipacaoSmeAreaConhecimento = repositorioParticipacaoSmeAreaConhecimento ?? throw new System.ArgumentNullException(nameof(repositorioParticipacaoSmeAreaConhecimento));
            this.repositorioResultadoCicloDre = repositorioResultadoCicloDre ?? throw new System.ArgumentNullException(nameof(repositorioResultadoCicloDre));
        }

        public async Task<ObjResultadoPspDto> Handle(ObterObjResultadoPspQuery request, CancellationToken cancellationToken)
        {
            ObjResultado = request.Resultado;
            object result = await ObterResultado();
            if (result == null) return null;
            return new ObjResultadoPspDto(ObjResultado.TipoResultado, result);
        }

        private async Task<object> ObterResultado()
        {
            switch (ObjResultado.TipoResultado)
            {
                case TipoResultadoPsp.ResultadoAluno:
                    return await ObterResultadoAluno();
                case TipoResultadoPsp.ResultadoSme:
                    return await ObterResultadoSME();
                case TipoResultadoPsp.ResultadoDre:
                    return await ObterResultadoDre();
                case TipoResultadoPsp.ResultadoEscola:
                    return await ObterResultadoEscola();
                case TipoResultadoPsp.ResultadoTurma:
                    return await ObterResultadoTurma();
                case TipoResultadoPsp.ResultadoParticipacaoTurma:
                    return await ObterParticipacaoTurma();
                case TipoResultadoPsp.ParticipacaoTurmaAreaConhecimento:
                    return await ObterParticipacaoTurmaAreaConhecimento();
                case TipoResultadoPsp.ResultadoParticipacaoUe:
                    return await ObterParticipacaoUe();
                case TipoResultadoPsp.ParticipacaoUeAreaConhecimento:
                    return await ObterParticipacaoUeAreaConhecimento();
                case TipoResultadoPsp.ParticipacaoDre:
                    return await ObterParticipacaoDre();
                case TipoResultadoPsp.ParticipacaoDreAreaConhecimento:
                    return await ObterParticipacaoDreAreaConhecimento();
                case TipoResultadoPsp.ParticipacaoSme:
                    return await ObterParticipacaoSme();
                case TipoResultadoPsp.ParticipacaoSmeAreaConhecimento:
                    return await ObterParticipacaoSmeAreaConhecimento();
                case TipoResultadoPsp.ResultadoCicloDre:
                    return await ObterResultadoCicloDre();
                default:
                    return null;
            }
        }


        private async Task<ParticipacaoTurma> ObterParticipacaoTurmaAreaConhecimento()
        {
            var participacaoTurma = (ParticipacaoTurmaAreaConhecimentoDto)ObjResultado.Resultado;
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
            var participacaoTurma = (ParticipacaoTurmaDto)ObjResultado.Resultado;
            return await repositorioParticipacaoTurma.ObterParticipacaoTurma(participacaoTurma.Edicao, participacaoTurma.uad_sigla, participacaoTurma.esc_codigo, participacaoTurma.AnoEscolar, participacaoTurma.tur_codigo);
        }

        private async Task<ResultadoAluno> ObterResultadoAluno()
        {
            var resultadoBusca = (ResultadoAlunoDto)ObjResultado.Resultado;
            return await repositorioResultadoAluno.ObterProficienciaAluno(resultadoBusca.Edicao, resultadoBusca.alu_matricula, resultadoBusca.AreaConhecimentoID);
        }
        private async Task<ResultadoTurma> ObterResultadoTurma()
        {
            var resultadoBusca = (ResultadoTurmaDto)ObjResultado.Resultado;
            return await repositorioResultadoTurma.ObterResultadoTurma(resultadoBusca.Edicao, resultadoBusca.AreaConhecimentoID, resultadoBusca.EscCodigo, resultadoBusca.TurCodigo);
        }
        private async Task<ResultadoEscola> ObterResultadoEscola()
        {
            var resultadoBusca = (ResultadoEscolaDto)ObjResultado.Resultado;
            return await repositorioResultadoEscola.ObterResultadoEscola(resultadoBusca.Edicao, resultadoBusca.AreaConhecimentoID, resultadoBusca.EscCodigo, resultadoBusca.AnoEscolar);
        }
        private async Task<ResultadoDre> ObterResultadoDre()
        {
            var resultadoBusca = (ResultadoDreDto)ObjResultado.Resultado;
            return await repositorioResultadoDre.ObterResultadoDre(resultadoBusca.Edicao, resultadoBusca.AreaConhecimentoID, resultadoBusca.UadSigla, resultadoBusca.AnoEscolar);
        }

        private async Task<ResultadoSme> ObterResultadoSME()
        {
            var resultadoBusca = (ResultadoSmeDto)ObjResultado.Resultado;
            return await repositorioResultadoSme.ObterResultadoSme(resultadoBusca.Edicao, resultadoBusca.AreaConhecimentoID, resultadoBusca.AnoEscolar);
        }

        private async Task<ParticipacaoUe> ObterParticipacaoUeAreaConhecimento()
        {
            var participacao = (ParticipacaoUeAreaConhecimentoDto)ObjResultado.Resultado;
            return await repositorioParticipacaoUeAreaConhecimento.
                ObterParticipacaoUeAreaConhecimento(participacao.Edicao,
                                                       participacao.uad_sigla,
                                                       participacao.AreaConhecimentoID,
                                                       participacao.esc_codigo,
                                                       participacao.AnoEscolar);
        }

        private async Task<ParticipacaoUe> ObterParticipacaoUe()
        {
            var participacao = (ParticipacaoUeDto)ObjResultado.Resultado;
            return await repositorioParticipacaoUe.ObterParticipacaoUe(participacao.Edicao, participacao.uad_sigla, participacao.esc_codigo, participacao.AnoEscolar);
        }

        private async Task<ParticipacaoDre> ObterParticipacaoDre()
        {
            var participacao = (ParticipacaoDreDto)ObjResultado.Resultado;
            return await repositorioParticipacaoDre.ObterParticipacaoDre(participacao.Edicao, participacao.uad_sigla, participacao.AnoEscolar);
        }

        private async Task<ParticipacaoDreAreaConhecimento> ObterParticipacaoDreAreaConhecimento()
        {
            var participacao = (ParticipacaoDreAreaConhecimentoDto)ObjResultado.Resultado;
            return await repositorioParticipacaoDreAreaConhecimento.ObterParticipacaoDreAreaConhecimento(participacao.Edicao, participacao.AreaConhecimentoID, participacao.uad_sigla, participacao.AnoEscolar);
        }

        private async Task<ParticipacaoSme> ObterParticipacaoSme()
        {
            var participacao = (ParticipacaoSmeDto)ObjResultado.Resultado;
            return await repositorioParticipacaoSme.ObterParticipacaoSme(participacao.Edicao, participacao.AnoEscolar);
        }

        private async Task<ParticipacaoSmeAreaConhecimento> ObterParticipacaoSmeAreaConhecimento()
        {
            var participacao = (ParticipacaoSmeAreaConhecimentoDto)ObjResultado.Resultado;
            return await repositorioParticipacaoSmeAreaConhecimento.ObterParticipacaoSmeAreaConhecimento(participacao.Edicao, participacao.AreaConhecimentoID, participacao.AnoEscolar);
        }

        private async Task<ResultadoCicloDre> ObterResultadoCicloDre()
        {
            var resultado = (ResultadoCicloDreDto)ObjResultado.Resultado;
            return await repositorioResultadoCicloDre.ObterResultadoCicloDre
                                                       (resultado.Edicao,
                                                       resultado.AreaConhecimentoId,
                                                       resultado.DreSigla,
                                                       resultado.CicloId);
        }
    }
}
