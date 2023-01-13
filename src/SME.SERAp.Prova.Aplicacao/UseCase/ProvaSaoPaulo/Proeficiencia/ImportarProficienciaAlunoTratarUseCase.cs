using MediatR;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Aplicacao.UseCase.ProvaSaoPaulo.Proeficiencia;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ImportarProficienciaAlunoTratarUseCase : AbstractUseCase, IImportarProeficienciaAlunoTratarUseCase
    {
        public ImportarProficienciaAlunoTratarUseCase(IMediator mediator) : base(mediator) { }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dto = mensagemRabbit.ObterObjetoMensagem<ArquivoProvaPspCVSDto>();
            


            return true;
        }
    }
}