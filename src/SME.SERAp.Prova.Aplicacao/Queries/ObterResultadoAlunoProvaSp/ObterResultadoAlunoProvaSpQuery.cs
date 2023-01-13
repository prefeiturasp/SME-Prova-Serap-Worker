using MediatR;
using SME.SERAp.Prova.Dominio.Entidades;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao.Queries
{
    public class ObterResultadoAlunoProvaSpQuery : IRequest<ResultadoAluno>
    {
        public ObterResultadoAlunoProvaSpQuery(ArquivoProvaPspCVSDto arquivoProvaPspCVSDto)
        {
            ArquivoProvaPspCVSDto = arquivoProvaPspCVSDto;
        }


        public ArquivoProvaPspCVSDto ArquivoProvaPspCVSDto { get; set; }
    }
}
