using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ImportarProficienciaCicloSmeUseCase : AbstractImportarProficienciaPspUseCase, IImportarProficienciaCicloSmeUseCase
    {
        public ImportarProficienciaCicloSmeUseCase(IMediator mediator, IServicoLog servicoLog,
            PathOptions pathOptions) : base(mediator, servicoLog, pathOptions)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            try
            {
                var idArquivoResultadoPsp = long.Parse(param.Mensagem.ToString() ?? string.Empty);

                if (idArquivoResultadoPsp == 0)
                    return false;
                
                var arquivoResultadoPsp = await Mediator.Send(new ObterTipoResultadoPspQuery(idArquivoResultadoPsp));
                
                if (arquivoResultadoPsp == null)
                    return false;
                
                PopularArquivoResultado(arquivoResultadoPsp);

                await AtualizaStatusDoProcesso(idArquivoResultadoPsp, StatusImportacao.EmAndamento);

                using (var csv = ResultadoPsp.ObterReaderArquivoResultadosPsp(PathOptions, arquivoResultadoPsp.NomeArquivo))
                {
                    var listaCsvResultados = csv.GetRecords<ResultadoCicloSmeDto>().ToList();
                    
                    foreach (var objCsvResultado in listaCsvResultados)
                    {
                        var dto = new RegistroProficienciaPspCsvDto(arquivoResultadoPsp.Id, objCsvResultado);
                        
                        ValidarAnoEdicao(objCsvResultado.Edicao);
                        
                        await PublicarFilaTratar(dto, TipoResultadoPsp.ResultadoCicloSme);
                    }
                }

                await PublicarFilaTratarStatusProcesso(idArquivoResultadoPsp);
                return true;
            }
            catch (Exception ex)
            {
                await RegistrarErroEAtualizarStatusProcesso(GetType().Name, param, ex);
                return false;
            }
        }
    }
}