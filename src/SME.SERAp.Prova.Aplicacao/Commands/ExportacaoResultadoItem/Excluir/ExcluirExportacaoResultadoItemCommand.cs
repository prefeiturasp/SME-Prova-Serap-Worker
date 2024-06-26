﻿using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExcluirExportacaoResultadoItemCommand : IRequest<bool>
    {
        public ExcluirExportacaoResultadoItemCommand(long itemId, long? processoId = null)
        {
            ItemId = itemId;
            ProcessoId = processoId;
        }

        public long? ProcessoId { get; }
        public long ItemId { get; }
    }
}
