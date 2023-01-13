using MediatR;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using SME.SERAp.Prova.Aplicacao.Queries;
using SME.SERAp.Prova.Infra.Dtos;
using SME.SERAp.Prova.Aplicacao.Commands.ProvaSP.ProvaResultado;
using SME.SERAp.Prova.Dominio.Entidades;
using Npgsql.TypeHandlers.GeometricHandlers;

namespace SME.SERAp.Prova.Aplicacao.UseCase.ProvaSaoPaulo.Proeficiencia
{
    public class ImportarProficienciaAlunoUseCase : AbstractUseCase, IImportarProeficienciaAlunoUseCase
    {
        public ImportarProficienciaAlunoUseCase(IMediator mediator) : base(mediator) { }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                //var contadorLinha = 0;
                //var linhaInicio = 10;
                /// Mudar  [ArquivoResultadoPsp] para  2 - Em andamento

                var IdArquivoResultadoPsp = long.Parse(mensagemRabbit.Mensagem.ToString());
                var arquivoResultadoPspDto = await mediator.Send(new ObterTipoResultadoPspQuery(IdArquivoResultadoPsp));
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    Delimiter = ";"
                };

                using (var reader = new StreamReader($"C:\\SISTEMAS\\SERAP\\{arquivoResultadoPspDto.NomeArquivo}"))
                using (var csv = new CsvReader(reader, config))
                {
                    var listaCsvResultadoAluno = csv.GetRecords<ArquivoProvaPspCVSDto>();

                  
                    foreach (var objCsvResultadoAluno in listaCsvResultadoAluno)
                    {

                        //if (contadorLinha == linhaInicio)
                        //{
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
                                    Valor = Decimal.Parse(objCsvResultadoAluno.Valor)

                                };

                                await mediator.Send(new IncluirResultadoAlunoCommand(resultadoAlunoEntidade));
                            }
                        //    contadorLinha++;

                        }
                    }
                
            }
            catch (Exception ex)
            {
                // Chamar a fila de erro (  // Mudar para  erro dentro da fila Erro4 
                throw ex;
            }

            return true;
        }
    }
}
