using MediatR;
using SME.SERAp.Prova.Aplicaca;
using SME.SERAp.Prova.Dominio;
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
                if (arquivoResultadoPsp == null) return false;

                await mediator.Send(new AtualizarStatusArquivoResultadoPspCommand(IdArquivoResultadoPsp, StatusImportacao.EmAndamento));

                using (var csv = ResultadoPsp.ObterReaderArquivoResultadosPsp(pathOptions, arquivoResultadoPsp.NomeArquivo))
                {
                    var listaCsvResultados = csv.GetRecords<ResultadoSmeDto>().ToList();
                    foreach (var objCsvResultado in listaCsvResultados)
                    {
                        var dto = new RegistroProficienciaPspCsvDto(arquivoResultadoPsp.Id, objCsvResultado);
                        await publicarFilaTratar(dto);
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

        private async Task publicarFilaTratar(RegistroProficienciaPspCsvDto dto)
        {
            string fila = ResultadoPsp.ObterFilaTratarPorTipoResultadoPsp((TipoResultadoPsp)arquivoResultadoPsp.CodigoTipoResultado);
            if (!string.IsNullOrEmpty(fila))
                await mediator.Send(new PublicaFilaRabbitCommand(fila, dto));
        }
    }
}
