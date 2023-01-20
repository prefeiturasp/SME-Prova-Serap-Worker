using MediatR;
using SME.SERAp.Prova.Aplicaca;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ImportarProficienciaSmeUseCase : AbstractUseCase, IImportarProficienciaSmeUseCase
    {

        private readonly IServicoLog servicoLog;
        private readonly PathOptions pathOptions;
        private ArquivoResultadoPspDto arquivoResultadoPsp = null;

        public ImportarProficienciaSmeUseCase(IMediator mediator, IServicoLog servicoLog, PathOptions pathOptions) : base(mediator)
        {
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
            this.pathOptions = pathOptions ?? throw new ArgumentNullException(nameof(pathOptions));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var IdArquivoResultadoPsp = long.Parse(mensagemRabbit.Mensagem.ToString());

                arquivoResultadoPsp = await mediator.Send(new ObterTipoResultadoPspQuery(IdArquivoResultadoPsp));
                await mediator.Send(new AtualizarStatusArquivoResultadoPspCommand(IdArquivoResultadoPsp, StatusImportacao.EmAndamento));

                var registroProvaCsv = new RegistroProficienciaPspCsvDto();
                registroProvaCsv.ProcessoId = arquivoResultadoPsp.Id;

                var csvUtil = new ResultadoPsp();
                using (var csv = csvUtil.ObterReaderArquivoResultadosPsp(pathOptions, arquivoResultadoPsp.NomeArquivo))
                {
                    var listaCsvResultados = csv.GetRecords<ResultadoSmeDto>().ToList();
                    foreach (var objCsvResultado in listaCsvResultados)
                    {
                        registroProvaCsv.Registro = objCsvResultado;
                        await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarResultadoSmePsp, registroProvaCsv));
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                servicoLog.Registrar($"Fila {GetType().Name} Id: {arquivoResultadoPsp?.Id} --- Mensagem -- {mensagemRabbit}-- Erro ao processar o arquivo  {arquivoResultadoPsp?.NomeOriginalArquivo} ", ex);
                await mediator.Send(new AtualizarStatusArquivoResultadoPspCommand((long)arquivoResultadoPsp?.Id, StatusImportacao.Erro));
                return false;
            }
        }
    }
}
