using MediatR;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.Interfaces;
using SME.SERAp.Prova.Infra;
using System.Threading.Tasks;
using System;
using SME.SERAp.Prova.Infra.Dtos;
using System.Linq;

namespace SME.SERAp.Prova.Aplicacao.UseCase
{
    public class ImportarResultadoCicloDreUseCase : AbstractImportarProficienciaPspUseCase, IImportarResultadoCicloDreUseCase
    {

        private TipoResultadoPsp tipoResultadoProcesso = TipoResultadoPsp.ResultadoCicloDre;
        public ImportarResultadoCicloDreUseCase(IMediator mediator,
                                             IServicoLog servicoLog,
                                             PathOptions pathOptions)
                                             : base(mediator, servicoLog, pathOptions) { }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var IdArquivoResultadoPsp = long.Parse(mensagemRabbit.Mensagem.ToString());
                var arquivoResultadoPsp = await mediator.Send(new ObterTipoResultadoPspQuery(IdArquivoResultadoPsp));
                if (arquivoResultadoPsp == null) return false;
                PopularArquivoResultado(arquivoResultadoPsp);

                await AtualizaStatusDoProcesso(IdArquivoResultadoPsp, StatusImportacao.EmAndamento);

                using (var csv = ResultadoPsp.ObterReaderArquivoResultadosPsp(pathOptions, arquivoResultadoPsp.NomeArquivo))
                {
                    var listaCsvResultados = csv.GetRecords<ResultadoCicloDreDto>().ToList();
                    foreach (var objCsvResultado in listaCsvResultados)
                    {
                        var dto = new RegistroProficienciaPspCsvDto(arquivoResultadoPsp.Id, objCsvResultado);
                        ValidarAnoEdicao(objCsvResultado.Edicao);
                        await publicarFilaTratar(dto, tipoResultadoProcesso);
                    }
                }
                await publicarFilaTratarStatusProcesso(IdArquivoResultadoPsp);
                return true;
            }
            catch (Exception ex)
            {
                await RegistrarErroEAtualizarStatusProcesso(GetType().Name, mensagemRabbit, ex);
                return false;
            }
        }
    }
}
