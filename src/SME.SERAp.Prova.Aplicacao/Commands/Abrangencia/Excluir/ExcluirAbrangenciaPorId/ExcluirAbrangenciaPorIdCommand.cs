﻿using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExcluirAbrangenciaPorIdCommand : IRequest<bool>
    {
        public ExcluirAbrangenciaPorIdCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
