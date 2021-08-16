using MediatR;
using System;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvaLegadoParaSeremSincronizadasQuery : IRequest<IEnumerable<long>>
    {
        public ObterProvaLegadoParaSeremSincronizadasQuery(DateTime ultimaExecucao)
        {
            UltimaExecucao = ultimaExecucao;
        }

        public DateTime UltimaExecucao { get; set; }
    }
}