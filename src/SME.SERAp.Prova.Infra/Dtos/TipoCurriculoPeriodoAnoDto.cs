using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Infra
{
    public class TipoCurriculoPeriodoAnoDto
    {
        public long TcpId { get; set; }
        public string Ano { get; set; }
        public Modalidade Modalidade { get; set; }
        public int EtapaEja { get; set; }
    }
}
