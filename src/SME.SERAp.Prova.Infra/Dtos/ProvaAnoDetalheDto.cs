using System;

namespace SME.SERAp.Prova.Infra
{
    public class ProvaAnoDetalheDto : DtoBase
    {
        public ProvaAnoDetalheDto()
        {

        }
        public int TcpId { get; set; }
        public string TcpNome { get; set; }
        public int TneId { get; set; }
        public string TneNome { get; set; }
        public int TmeId { get; set; }
        public string TmeNome { get; set; }

        //public string Ano { get; set; }
        //public int CurId { get; set; }
        //public string CurCodigo { get; set; }
        //public int TmeId { get; set; }
        //public int TneId { get; set; }
    }
}
