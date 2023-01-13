using MediatR;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SME.SERAp.Prova.Dominio.Entidades;

namespace SME.SERAp.Prova.Aplicacao.Queries
{
    public class ObterResultadoAlunoProvaSpQueryHandler : IRequestHandler<ObterResultadoAlunoProvaSpQuery, ResultadoAluno>
    {

        private readonly IRepositorioResultadoAluno repositorioResultadoAluno;

        public ObterResultadoAlunoProvaSpQueryHandler(IRepositorioResultadoAluno repositorioResultadoAluno)
        {
            this.repositorioResultadoAluno = repositorioResultadoAluno ?? throw new System.ArgumentNullException(nameof(repositorioResultadoAluno));
        }

        public async Task<ResultadoAluno> Handle(ObterResultadoAlunoProvaSpQuery request, CancellationToken cancellationToken)
        {
            return await repositorioResultadoAluno.ObterProficienciaAluno(request.ArquivoProvaPspCVSDto.Edicao, request.ArquivoProvaPspCVSDto.alu_matricula, request.ArquivoProvaPspCVSDto.tur_id, request.ArquivoProvaPspCVSDto.AreaConhecimentoID);
        }
    }
}