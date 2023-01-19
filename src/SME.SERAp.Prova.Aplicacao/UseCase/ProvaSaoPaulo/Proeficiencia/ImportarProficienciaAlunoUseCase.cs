using CsvHelper;
using CsvHelper.Configuration;
using MediatR;
using SME.SERAp.Prova.Aplicaca;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.UseCase.ProvaSaoPaulo.Proeficiencia
{
    public class ImportarProficienciaAlunoUseCase : AbstractUseCase, IImportarProeficienciaAlunoUseCase
    {
        private readonly IServicoLog servicoLog;

        public ImportarProficienciaAlunoUseCase(IMediator mediator, IServicoLog servicoLog) : base(mediator)
        {
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));

        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var nomeArquivo = "";

            try
            {
                var IdArquivoResultadoPsp = long.Parse(mensagemRabbit.Mensagem.ToString());
                await mediator.Send(new AtualizarStatusArquivoResultadoPspCommand(IdArquivoResultadoPsp, StatusImportacao.EmAndamento));
                var arquivoResultadoPspDto = await mediator.Send(new ObterTipoResultadoPspQuery(IdArquivoResultadoPsp));

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    Delimiter = ";"
                };


                var registroProvaCsv = new RegistroProvaPspCVSDto();
                registroProvaCsv.IdArquivo = arquivoResultadoPspDto.Id;
                nomeArquivo = arquivoResultadoPspDto.NomeArquivo;

                using (var reader = new StreamReader($"{Environment.GetEnvironmentVariable("PathArquivos")}/{"ResultadoPsp"}/{arquivoResultadoPspDto.NomeArquivo}"))
                using (var csv = new CsvReader(reader, config))
                {
                    var listaCsvResultadoAluno = csv.GetRecords<ArquivoProvaPspCVSDto>();
                    foreach (var objCsvResultadoAluno in listaCsvResultadoAluno)
                    {
                        registroProvaCsv.ArquivoProvaPspCVSDto = objCsvResultadoAluno;
                        await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarResultadoAlunoPsp, registroProvaCsv));
                    }
                }
            }
            catch (Exception ex)
            {
                var IdAquivo = long.Parse(mensagemRabbit.Mensagem.ToString());
                servicoLog.Registrar($"Fila ImportarProficienciaAlunoUseCase Id: {mensagemRabbit.Mensagem.ToString()} --- Mensagem -- {mensagemRabbit}-- Erro ao processar o arquivo  {nomeArquivo} ", ex);
                await mediator.Send(new AtualizarStatusArquivoResultadoPspCommand(IdAquivo, StatusImportacao.Erro));
                return false;
            }
            return true;
        }
    }
}
