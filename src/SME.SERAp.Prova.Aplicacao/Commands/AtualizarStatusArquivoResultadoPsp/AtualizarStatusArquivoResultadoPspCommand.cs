using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicaca
{
    public class AtualizarStatusArquivoResultadoPspCommand : IRequest<bool>
    {
        public AtualizarStatusArquivoResultadoPspCommand(long id, StatusImportacao statusImportacao)
        {
            Id = id;
            Status = statusImportacao;
        }

        public long Id { get; set; }
        public StatusImportacao Status { get; set; }
    }
}