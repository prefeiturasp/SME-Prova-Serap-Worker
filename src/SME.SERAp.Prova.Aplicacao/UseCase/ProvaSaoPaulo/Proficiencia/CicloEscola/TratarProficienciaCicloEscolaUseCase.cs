﻿using System;
using System.Threading.Tasks;
using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProficienciaCicloEscolaUseCase : AbstractTratarProficienciaPspUseCase, ITratarProficienciaCicloEscolaUseCase
    {
        public TratarProficienciaCicloEscolaUseCase(IMediator mediator, IServicoLog servicoLog, IModel model) : base(
            mediator, servicoLog, model)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var registroProficienciaPsp = param.ObterObjetoMensagem<RegistroProficienciaPspCsvDto>();
            PopularRegistroProficienciaPsp(registroProficienciaPsp);
            
            var objResultadoCsv = registroProficienciaPsp.ObterObjetoRegistro<ResultadoCicloEscolaDto>();
            PopularTipoResultadoProcesso(TipoResultadoPsp.ResultadoCicloEscola);

            try
            {
                var resultadoBanco = await ObterResultadoBanco(objResultadoCsv);
                var resultadoEntidade = MapearParaEntidade(objResultadoCsv);

                if (resultadoBanco == null)
                    await Inserir(resultadoEntidade);
                else
                    await Alterar(resultadoEntidade);

                await VerificaSeFinalizaProcesso();

                return true;
            }
            catch (Exception ex)
            {
                await RegistrarErroEAtualizarStatusProcesso(GetType().Name, ex);
                return false;
            }
        }
        
        private static ResultadoCicloEscola MapearParaEntidade(ResultadoCicloEscolaDto objResultadoCsv)
        {
            return new ResultadoCicloEscola
            {
                Edicao = objResultadoCsv.Edicao,
                AreaConhecimentoId = objResultadoCsv.AreaConhecimentoId,
                UadSigla = objResultadoCsv.UadSigla,
                EscCodigo = objResultadoCsv.EscCodigo,
                CicloId = objResultadoCsv.CicloId,
                Valor = objResultadoCsv.Valor,
                NivelProficienciaId = objResultadoCsv.NivelProficienciaId,                
                TotalAlunos = objResultadoCsv.TotalAlunos,
                PercentualAbaixoDoBasico = objResultadoCsv.PercentualAbaixoDoBasico,
                PercentualBasico = objResultadoCsv.PercentualBasico,
                PercentualAdequado = objResultadoCsv.PercentualAdequado,
                PercentualAvancado = objResultadoCsv.PercentualAvancado,
                PercentualAlfabetizado = objResultadoCsv.PercentualAlfabetizado
            };
        }        
    }
}