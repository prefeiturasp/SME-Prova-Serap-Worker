using System;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio;
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
        private readonly IRepositorioResultadoCicloSme repositorioResultadoCicloSme;
        private readonly IRepositorioResultadoCicloEscola repositorioResultadoCicloEscola;        

        private ObjResultadoPspDto objResultado;

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
                                                 IRepositorioResultadoCicloSme repositorioResultadoCicloSme,
                                                 IRepositorioResultadoCicloEscola repositorioResultadoCicloEscola)
        {
            this.repositorioResultadoSme = repositorioResultadoSme ?? throw new ArgumentNullException(nameof(repositorioResultadoSme));
            this.repositorioResultadoDre = repositorioResultadoDre ?? throw new ArgumentNullException(nameof(repositorioResultadoDre));
            this.repositorioResultadoEscola = repositorioResultadoEscola ?? throw new ArgumentNullException(nameof(repositorioResultadoEscola));
            this.repositorioResultadoTurma = repositorioResultadoTurma ?? throw new ArgumentNullException(nameof(repositorioResultadoTurma));
            this.repositorioResultadoAluno = repositorioResultadoAluno ?? throw new ArgumentNullException(nameof(repositorioResultadoAluno));
            this.repositorioParticipacaoTurma = repositorioParticipacaoTurma ?? throw new ArgumentNullException(nameof(repositorioParticipacaoTurma));
            this.repositorioParticipacaoTurmaAreaConhecimento = repositorioParticipacaoTurmaAreaConhecimento ?? throw new ArgumentNullException(nameof(repositorioParticipacaoTurmaAreaConhecimento));
            this.repositorioParticipacaoUe = repositorioParticipacaoUe ?? throw new ArgumentNullException(nameof(repositorioParticipacaoUe));
            this.repositorioParticipacaoUeAreaConhecimento = repositorioParticipacaoUeAreaConhecimento ?? throw new ArgumentNullException(nameof(repositorioParticipacaoUeAreaConhecimento));
            this.repositorioParticipacaoDre = repositorioParticipacaoDre ?? throw new ArgumentNullException(nameof(repositorioParticipacaoDre));
            this.repositorioParticipacaoDreAreaConhecimento = repositorioParticipacaoDreAreaConhecimento ?? throw new ArgumentNullException(nameof(repositorioParticipacaoDreAreaConhecimento));
            this.repositorioParticipacaoSme = repositorioParticipacaoSme ?? throw new ArgumentNullException(nameof(repositorioParticipacaoSme));
            this.repositorioParticipacaoSmeAreaConhecimento = repositorioParticipacaoSmeAreaConhecimento ?? throw new ArgumentNullException(nameof(repositorioParticipacaoSmeAreaConhecimento));
            this.repositorioResultadoCicloSme = repositorioResultadoCicloSme ?? throw new ArgumentNullException(nameof(repositorioResultadoCicloSme));
            this.repositorioResultadoCicloEscola = repositorioResultadoCicloEscola ?? throw new ArgumentNullException(nameof(repositorioResultadoCicloEscola));
        }

        public async Task<bool> Handle(IncluirResultadoPspCommand request, CancellationToken cancellationToken)
        {
            objResultado = request.Resultado;

            return request.Resultado.TipoResultado switch
            {
                TipoResultadoPsp.ResultadoAluno => await IncluirResultadoAluno(),
                TipoResultadoPsp.ResultadoSme => await IncluirResultadoSme(),
                TipoResultadoPsp.ResultadoDre => await IncluirResultadoDre(),
                TipoResultadoPsp.ResultadoEscola => await IncluirResultadoEscola(),
                TipoResultadoPsp.ResultadoTurma => await IncluirResultadoTurma(),
                TipoResultadoPsp.ResultadoParticipacaoTurma => await IncluirParticipacaoTurma(),
                TipoResultadoPsp.ParticipacaoTurmaAreaConhecimento => await IncluirParticipacaoTurmaAreaConhecimento(),
                TipoResultadoPsp.ResultadoParticipacaoUe => await IncluirParticipacaoUe(),
                TipoResultadoPsp.ParticipacaoUeAreaConhecimento => await IncluirParticipacaoUeAreaConhecimento(),
                TipoResultadoPsp.ParticipacaoDre => await IncluirParticipacaoDre(),
                TipoResultadoPsp.ParticipacaoDreAreaConhecimento => await IncluirParticipacaoDreAreaConhecimento(),
                TipoResultadoPsp.ParticipacaoSme => await IncluirParticipacaoSme(),
                TipoResultadoPsp.ParticipacaoSmeAreaConhecimento => await IncluirParticipacaoSmeAreaConhecimento(),
                TipoResultadoPsp.ResultadoCicloSme => await IncluirResultadoCicloSme(),
                TipoResultadoPsp.ResultadoCicloEscola => await IncluirResultadoCicloEscola(),                
                _ => false
            };
        }

        private async Task<bool> IncluirParticipacaoTurma()
        {
            var participacaoInserir = (ParticipacaoTurma)objResultado.Resultado;
            var result = await repositorioParticipacaoTurma.IncluirAsync(participacaoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirParticipacaoTurmaAreaConhecimento()
        {
            var participacaoInserir = (ParticipacaoTurmaAreaConhecimento)objResultado.Resultado;
            var result = await repositorioParticipacaoTurmaAreaConhecimento.IncluirAsync(participacaoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirResultadoAluno()
        {
            var resultadoInserir = (ResultadoAluno)objResultado.Resultado;
            var result = await repositorioResultadoAluno.IncluirAsync(resultadoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirResultadoSme()
        {
            var resultadoInserir = (ResultadoSme)objResultado.Resultado;
            var result = await repositorioResultadoSme.IncluirAsync(resultadoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirResultadoDre()
        {
            var resultadoInserir = (ResultadoDre)objResultado.Resultado;
            var result = await repositorioResultadoDre.IncluirAsync(resultadoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirResultadoEscola()
        {
            var resultadoInserir = (ResultadoEscola)objResultado.Resultado;
            var result = await repositorioResultadoEscola.IncluirAsync(resultadoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirResultadoTurma()
        {
            var resultadoInserir = (ResultadoTurma)objResultado.Resultado;
            var result = await repositorioResultadoTurma.IncluirAsync(resultadoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirParticipacaoUe()
        {
            var participacaoInserir = (ParticipacaoUe)objResultado.Resultado;
            var result = await repositorioParticipacaoUe.IncluirAsync(participacaoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirParticipacaoUeAreaConhecimento()
        {
            var participacaoInserir = (ParticipacaoUeAreaConhecimento)objResultado.Resultado;
            var result = await repositorioParticipacaoUeAreaConhecimento.IncluirAsync(participacaoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirParticipacaoDre()
        {
            var participacaoInserir = (ParticipacaoDre)objResultado.Resultado;
            var result = await repositorioParticipacaoDre.IncluirAsync(participacaoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirParticipacaoDreAreaConhecimento()
        {
            var participacaoInserir = (ParticipacaoDreAreaConhecimento)objResultado.Resultado;
            var result = await repositorioParticipacaoDreAreaConhecimento.IncluirAsync(participacaoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirParticipacaoSme()
        {
            var participacaoInserir = (ParticipacaoSme)objResultado.Resultado;
            var result = await repositorioParticipacaoSme.IncluirAsync(participacaoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirParticipacaoSmeAreaConhecimento()
        {
            var participacaoInserir = (ParticipacaoSmeAreaConhecimento)objResultado.Resultado;
            var result = await repositorioParticipacaoSmeAreaConhecimento.IncluirAsync(participacaoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirResultadoCicloSme()
        {
            var resultadoInserir = (ResultadoCicloSme)objResultado.Resultado;
            var result = await repositorioResultadoCicloSme.IncluirAsync(resultadoInserir);
            return result > 0;
        }
        
        private async Task<bool> IncluirResultadoCicloEscola()
        {
            var resultadoInserir = (ResultadoCicloEscola)objResultado.Resultado;
            var result = await repositorioResultadoCicloEscola.IncluirAsync(resultadoInserir);
            return result > 0;
        }        
    }
}
