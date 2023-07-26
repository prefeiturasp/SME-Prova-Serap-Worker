using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Dominio.Entidades.ProficienciaPsp;
using SME.SERAp.Prova.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class IncluirResultadoPspCommandHandler : IRequestHandler<IncluirResultadoPspCommand, bool>
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

        public IncluirResultadoPspCommandHandler(IRepositorioResultadoSme repositorioResultadoSme,
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
            this.repositorioParticipacaoTurma = repositorioParticipacaoTurma ?? throw new System.ArgumentNullException(nameof(repositorioParticipacaoTurma));
            this.repositorioParticipacaoTurmaAreaConhecimento = repositorioParticipacaoTurmaAreaConhecimento ?? throw new System.ArgumentNullException(nameof(repositorioParticipacaoTurmaAreaConhecimento));
            this.repositorioParticipacaoUe = repositorioParticipacaoUe ?? throw new System.ArgumentNullException(nameof(repositorioParticipacaoUe));
            this.repositorioParticipacaoUeAreaConhecimento = repositorioParticipacaoUeAreaConhecimento ?? throw new System.ArgumentNullException(nameof(repositorioParticipacaoUeAreaConhecimento));
            this.repositorioParticipacaoDre = repositorioParticipacaoDre ?? throw new System.ArgumentNullException(nameof(repositorioParticipacaoDre));
            this.repositorioParticipacaoDreAreaConhecimento = repositorioParticipacaoDreAreaConhecimento ?? throw new System.ArgumentNullException(nameof(repositorioParticipacaoDreAreaConhecimento));
            this.repositorioParticipacaoSme = repositorioParticipacaoSme ?? throw new System.ArgumentNullException(nameof(repositorioParticipacaoSme));
            this.repositorioParticipacaoSmeAreaConhecimento = repositorioParticipacaoSmeAreaConhecimento ?? throw new System.ArgumentNullException(nameof(repositorioParticipacaoSmeAreaConhecimento));
            this.repositorioResultadoCicloDre = repositorioResultadoCicloDre ?? throw new System.ArgumentNullException(nameof(repositorioResultadoCicloDre));

        }

        public async Task<bool> Handle(IncluirResultadoPspCommand request, CancellationToken cancellationToken)
        {
            ObjResultado = request.Resultado;
            switch (request.Resultado.TipoResultado)
            {
                case TipoResultadoPsp.ResultadoAluno:
                    return await IncluirResultadoAluno();
                case TipoResultadoPsp.ResultadoSme:
                    return await IncluirResultadoSME();
                case TipoResultadoPsp.ResultadoDre:
                    return await IncluirResultadoDre();
                case TipoResultadoPsp.ResultadoEscola:
                    return await IncluirResultadoEscola();
                case TipoResultadoPsp.ResultadoTurma:
                    return await IncluirResultadoTurma();
                case TipoResultadoPsp.ResultadoParticipacaoTurma:
                    return await IncluirParticipacaoTurma();
                case TipoResultadoPsp.ParticipacaoTurmaAreaConhecimento:
                    return await IncluirParticipacaoTurmaAreaConhecimento();
                case TipoResultadoPsp.ResultadoParticipacaoUe:
                    return await IncluirParticipacaoUe();
                case TipoResultadoPsp.ParticipacaoUeAreaConhecimento:
                    return await IncluirParticipacaoUeAreaConhecimento();
                case TipoResultadoPsp.ParticipacaoDre:
                    return await IncluirParticipacaoDre();
                case TipoResultadoPsp.ParticipacaoDreAreaConhecimento:
                    return await IncluirParticipacaoDreAreaConhecimento();
                case TipoResultadoPsp.ParticipacaoSme:
                    return await IncluirParticipacaoSme();
                case TipoResultadoPsp.ParticipacaoSmeAreaConhecimento:
                    return await IncluirParticipacaoSmeAreaConhecimento();
                case TipoResultadoPsp.ResultadoCicloDre:
                    return await IncluirResultadoCicloDre();
                default:
                    return false;
            }
        }

        private async Task<bool> IncluirParticipacaoTurma()
        {
            var participacaoInserir = (ParticipacaoTurma)ObjResultado.Resultado;
            var result = await repositorioParticipacaoTurma.IncluirAsync(participacaoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirParticipacaoTurmaAreaConhecimento()
        {
            var participacaoInserir = (ParticipacaoTurmaAreaConhecimento)ObjResultado.Resultado;
            var result = await repositorioParticipacaoTurmaAreaConhecimento.IncluirAsync(participacaoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirResultadoAluno()
        {
            var resultadoInserir = (ResultadoAluno)ObjResultado.Resultado;
            var result = await repositorioResultadoAluno.IncluirAsync(resultadoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirResultadoSME()
        {
            var resultadoInserir = (ResultadoSme)ObjResultado.Resultado;
            var result = await repositorioResultadoSme.IncluirAsync(resultadoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirResultadoDre()
        {
            var resultadoInserir = (ResultadoDre)ObjResultado.Resultado;
            var result = await repositorioResultadoDre.IncluirAsync(resultadoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirResultadoEscola()
        {
            var resultadoInserir = (ResultadoEscola)ObjResultado.Resultado;
            var result = await repositorioResultadoEscola.IncluirAsync(resultadoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirResultadoTurma()
        {
            var resultadoInserir = (ResultadoTurma)ObjResultado.Resultado;
            var result = await repositorioResultadoTurma.IncluirAsync(resultadoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirParticipacaoUe()
        {
            var participacaoInserir = (ParticipacaoUe)ObjResultado.Resultado;
            var result = await repositorioParticipacaoUe.IncluirAsync(participacaoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirParticipacaoUeAreaConhecimento()
        {
            var participacaoInserir = (ParticipacaoUeAreaConhecimento)ObjResultado.Resultado;
            var result = await repositorioParticipacaoUeAreaConhecimento.IncluirAsync(participacaoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirParticipacaoDre()
        {
            var participacaoInserir = (ParticipacaoDre)ObjResultado.Resultado;
            var result = await repositorioParticipacaoDre.IncluirAsync(participacaoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirParticipacaoDreAreaConhecimento()
        {
            var participacaoInserir = (ParticipacaoDreAreaConhecimento)ObjResultado.Resultado;
            var result = await repositorioParticipacaoDreAreaConhecimento.IncluirAsync(participacaoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirParticipacaoSme()
        {
            var participacaoInserir = (ParticipacaoSme)ObjResultado.Resultado;
            var result = await repositorioParticipacaoSme.IncluirAsync(participacaoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirParticipacaoSmeAreaConhecimento()
        {
            var participacaoInserir = (ParticipacaoSmeAreaConhecimento)ObjResultado.Resultado;
            var result = await repositorioParticipacaoSmeAreaConhecimento.IncluirAsync(participacaoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirResultadoCicloDre()
        {
            var resultadoInserir = (ResultadoCicloDre)ObjResultado.Resultado;
            var result = await repositorioResultadoCicloDre.IncluirAsync(resultadoInserir);
            return result > 0;
        }
    }
}
