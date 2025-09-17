using MediatR;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Aplicacao.Queries.ObterAnoProva;
using SME.SERAp.Prova.Aplicacao.Queries.ObterResultadoAlunoProvaSp;
using SME.SERAp.Prova.Dominio.Entidades;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Extensions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.UseCase
{
    public class BuscarAlunoProvaSpProficienciaUseCase : AbstractUseCase, IBuscarAlunoProvaSpProficienciaUseCase
    {
        private readonly IServicoLog servicoLog;

        public BuscarAlunoProvaSpProficienciaUseCase(IMediator mediator, IServicoLog servicoLog) : base(mediator)
        {
            this.servicoLog = servicoLog;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var boletimProvaAluno = mensagemRabbit.ObterObjetoMensagem<BoletimProvaAluno>();
                if (boletimProvaAluno is null)
                    throw new Exception("Mensagem inválida.");

                var areaDoConhecimentoId = ObterAreaDoConhecimento(boletimProvaAluno.DisciplinaId);
                var alunoMatricula = boletimProvaAluno.AlunoRa.ToString();
                var edicaoProvaSp = await ObterEdicaoProvaSp(boletimProvaAluno.ProvaId);

                var ResultadoAlunoProvaSp = await mediator
                    .Send(new ObterResultadoAlunoProvaSpQuery(edicaoProvaSp, areaDoConhecimentoId, alunoMatricula));

                if (ResultadoAlunoProvaSp is null)
                    return true;

                var anoEscolar = ResultadoAlunoProvaSp.AnoEscolar?.ConverterParaInt() ?? 0;
                var anoLetivo = ResultadoAlunoProvaSp.Edicao?.ConverterParaInt() ?? 0;

                var alunoProvaSpProficiencia = new AlunoProvaSpProficiencia
                {
                    AlunoRa = boletimProvaAluno.AlunoRa,
                    DisciplinaId = boletimProvaAluno.DisciplinaId,
                    AnoEscolar = anoEscolar,
                    AnoLetivo = anoLetivo,
                    NivelProficiencia = ResultadoAlunoProvaSp.NivelProficiencia,
                    Proficiencia = ResultadoAlunoProvaSp.Valor,
                    DataAtualizacao = DateTime.Now,
                    UeCodigo = ResultadoAlunoProvaSp.CodigoUe
                };

                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarAlunoProvaSpProficiencia, alunoProvaSpProficiencia));

                return true;
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(ex);
                return false;
            }
        }

        private async Task<int> ObterEdicaoProvaSp(long provaId)
        {
            var anoProva = await mediator.Send(new ObterAnoProvaQuery(provaId));
            if (anoProva is null || anoProva == 0)
                throw new Exception($"Não foi possível identificar o ano da prova. ProvaId: {provaId}");

            var edicao = anoProva.Value - 1;
            return edicao;
        }

        private int ObterAreaDoConhecimento(long disciplinaId)
        {
            switch(disciplinaId)
            {
                case 4:
                    return 3; //Matemática
                case 5:
                    return 2; //Língua Portuguesa
                case 2:
                case 6:
                case 7:
                    return 1; //Ciências
                default:
                    return 0;
            }
        }
    }
}
