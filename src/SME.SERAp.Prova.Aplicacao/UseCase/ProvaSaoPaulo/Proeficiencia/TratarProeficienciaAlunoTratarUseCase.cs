using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Prova.Aplicaca;
using SME.SERAp.Prova.Aplicacao.Commands.ProvaSP.ProvaResultado;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Aplicacao.Queries;
using SME.SERAp.Prova.Dominio.Entidades;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProeficienciaAlunoTratarUseCase : AbstractUseCase, ITratarProeficienciaAlunoTratarUseCase
    {
        private readonly IServicoLog servicoLog;
        private readonly IModel model;
        public TratarProeficienciaAlunoTratarUseCase(IMediator mediator, IServicoLog servicoLog, IModel model) : base(mediator)
        {
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
            this.model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var registroProvaPspCVSDto = mensagemRabbit.ObterObjetoMensagem<RegistroProvaPspCVSDto>();
            var objCsvResultadoAluno = registroProvaPspCVSDto.ArquivoProvaPspCVSDto;

            try
            {
                var resultadoAlunoBanco = await mediator.Send(new ObterResultadoAlunoProvaSpQuery(objCsvResultadoAluno));

                if (resultadoAlunoBanco == null)
                {
                    var resultadoAlunoEntidade = new ResultadoAluno()
                    {
                        Edicao = objCsvResultadoAluno.Edicao,
                        uad_sigla = objCsvResultadoAluno.uad_sigla,
                        esc_codigo = objCsvResultadoAluno.esc_codigo,
                        AnoEscolar = objCsvResultadoAluno.AnoEscolar,
                        tur_codigo = objCsvResultadoAluno.tur_codigo,
                        tur_id = objCsvResultadoAluno.tur_id,
                        alu_matricula = objCsvResultadoAluno.alu_matricula,
                        alu_nome = objCsvResultadoAluno.alu_nome,
                        NivelProficienciaID = objCsvResultadoAluno.NivelProficienciaID,
                        AreaConhecimentoID = objCsvResultadoAluno.AreaConhecimentoID,
                        Valor = ObterValor(objCsvResultadoAluno.Valor)
                    };

                    await mediator.Send(new IncluirResultadoAlunoCommand(resultadoAlunoEntidade));
                }

                var qtd = model.MessageCount(RotasRabbit.TratarResultadoAlunoPsp);

                if (qtd == 0)
                    await AtualizaStatusDoArquivo(registroProvaPspCVSDto);

                return true;

            }
            catch (Exception ex)
            {
                servicoLog.Registrar($"Fila TratarProeficienciaAlunoTratarUseCase ObjetoMensagem: {objCsvResultadoAluno}, Erro ao processar o registro do Arquivo {registroProvaPspCVSDto.IdArquivo}", ex);
                await mediator.Send(new AtualizarStatusArquivoResultadoPspCommand(registroProvaPspCVSDto.IdArquivo, StatusImportacao.Erro));
                return false;
            }
        }

        private async Task AtualizaStatusDoArquivo(RegistroProvaPspCVSDto registroProvaPspCVSDto)
        {
            var arquivoResultadoPspDto = await mediator.Send(new ObterTipoResultadoPspQuery(registroProvaPspCVSDto.IdArquivo));
            if (arquivoResultadoPspDto.State != (long)StatusImportacao.Erro)
                await mediator.Send(new AtualizarStatusArquivoResultadoPspCommand(registroProvaPspCVSDto.IdArquivo, StatusImportacao.Processado));
        }

        private string ObterValor(string valor)
        {
            if (string.IsNullOrEmpty(valor)) return "0";
            decimal dec_valor = 0;
            if (decimal.TryParse(valor, out dec_valor))
            {
                return Math.Round(Convert.ToDecimal(valor), 2).ToString().Replace(",",".");
            }
            throw new ArgumentException($"não foi possível converter o valor para decimal: {valor}");
        }

    }
}