using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Infra.Dtos
{
    public class ExportacaoAlunoProvaResultadoQuestaoDto
    {
        public ExportacaoResultado ExportacaoResultado { get; set; }

        public bool PossuiDeficiencia { get; set; }

        public bool EhAdesaoATodos { get; set; }

        public ExportacaoResultadoStatus status { get; set; }

        public ConsolidadoProvaRespostaDto ConsolidadoProvaRespostaDto { get; set; }

    }
}
