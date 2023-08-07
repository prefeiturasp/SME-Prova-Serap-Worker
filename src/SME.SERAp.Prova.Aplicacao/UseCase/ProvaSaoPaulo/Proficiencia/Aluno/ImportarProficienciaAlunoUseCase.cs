using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ImportarProficienciaAlunoUseCase : AbstractImportarProficienciaPspUseCase, IImportarProficienciaAlunoUseCase
    {
        private const TipoResultadoPsp TipoResultadoProcesso = TipoResultadoPsp.ResultadoAluno;

        public ImportarProficienciaAlunoUseCase(IMediator mediator,
                                                 IServicoLog servicoLog,
                                                 PathOptions pathOptions)
                                                 : base(mediator, servicoLog, pathOptions) { }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var idArquivoResultadoPsp = long.Parse(mensagemRabbit.Mensagem.ToString() ?? string.Empty);
                var arquivoResultadoPsp = await mediator.Send(new ObterTipoResultadoPspQuery(idArquivoResultadoPsp));

                if (arquivoResultadoPsp == null) 
                    return false;
                
                PopularArquivoResultado(arquivoResultadoPsp);

                await AtualizaStatusDoProcesso(idArquivoResultadoPsp, StatusImportacao.EmAndamento);

                using (var csv = ResultadoPsp.ObterReaderArquivoResultadosPsp(pathOptions, arquivoResultadoPsp.NomeArquivo))
                {
                    var listaCsvResultados = csv.GetRecords<ResultadoAlunoDto>().ToList();

                    foreach (var dto in listaCsvResultados.Select(objCsvResultado =>
                                 new RegistroProficienciaPspCsvDto(arquivoResultadoPsp.Id, objCsvResultado)))
                    {
                        await publicarFilaTratar(dto, TipoResultadoProcesso);
                    }
                }
                
                await publicarFilaTratarStatusProcesso(idArquivoResultadoPsp);
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
