using MediatR;
using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AtualizarValorProficienciaAlunoCommand : IRequest<bool>
    {
        public AtualizarValorProficienciaAlunoCommand(AlunoProvaProficiencia alunoProvaProficiencia)
        {
            AlunoProvaProficiencia = alunoProvaProficiencia;
        }

        public AlunoProvaProficiencia AlunoProvaProficiencia { get; set; }
    }
}
