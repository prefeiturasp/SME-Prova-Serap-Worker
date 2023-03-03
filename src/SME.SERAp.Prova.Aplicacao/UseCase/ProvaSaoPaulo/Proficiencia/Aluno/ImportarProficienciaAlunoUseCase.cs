using MediatR;
using SME.SERAp.Prova.Aplicaca;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.UseCase.ProvaSaoPaulo.Proeficiencia
{
    public class ImportarProficienciaAlunoUseCase : AbstractUseCase, IImportarProeficienciaAlunoUseCase
    {
        private readonly IServicoLog servicoLog;
        private readonly PathOptions pathOptions;

        public ImportarProficienciaAlunoUseCase(IMediator mediator, IServicoLog servicoLog, PathOptions pathOptions) : base(mediator)
        {
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
            this.pathOptions = pathOptions ?? throw new ArgumentNullException(nameof(pathOptions));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var nomeArquivo = "";

            try
            {
                var idArquivoResultadoPsp = long.Parse(mensagemRabbit.Mensagem.ToString());
                
                await mediator.Send(new AtualizarStatusArquivoResultadoPspCommand(idArquivoResultadoPsp, StatusImportacao.EmAndamento));
                
                var arquivoResultadoPspDto = await mediator.Send(new ObterTipoResultadoPspQuery(idArquivoResultadoPsp));

                var registroProvaCsv = new RegistroProvaPspCVSDto
                {
                    IdArquivo = arquivoResultadoPspDto.Id
                };
                
                nomeArquivo = arquivoResultadoPspDto.NomeArquivo;
                
                using var csv = ResultadoPsp.ObterReaderArquivoResultadosPsp(pathOptions, arquivoResultadoPspDto.NomeArquivo);
                
                var listaCsvResultadoAluno = csv.GetRecords<ArquivoProvaPspCVSDto>();
                foreach (var objCsvResultadoAluno in listaCsvResultadoAluno)
                {
                    registroProvaCsv.ArquivoProvaPspCVSDto = objCsvResultadoAluno;
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarResultadoAlunoPsp, registroProvaCsv));
                }
            }
            catch (Exception ex)
            {
                var idAquivo = long.Parse(mensagemRabbit.Mensagem.ToString());
                servicoLog.Registrar($"Fila ImportarProficienciaAlunoUseCase Id: {mensagemRabbit.Mensagem.ToString()} --- Mensagem -- {mensagemRabbit}-- Erro ao processar o arquivo  {nomeArquivo} ", ex);
                await mediator.Send(new AtualizarStatusArquivoResultadoPspCommand(idAquivo, StatusImportacao.Erro));
                return false;
            }
            return true;
        }
    }
}
