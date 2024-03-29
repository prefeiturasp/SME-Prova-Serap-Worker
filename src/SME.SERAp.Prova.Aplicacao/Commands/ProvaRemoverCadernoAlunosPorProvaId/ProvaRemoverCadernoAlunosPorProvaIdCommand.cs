﻿using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaRemoverCadernoAlunosPorProvaIdCommand : IRequest<bool>
    {
        public ProvaRemoverCadernoAlunosPorProvaIdCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
