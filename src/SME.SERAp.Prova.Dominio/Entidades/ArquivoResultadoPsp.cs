using System;

namespace SME.SERAp.Prova.Dominio.Entidades
{
    public class ArquivoResultadoPsp : EntidadeBase
    {
        public ArquivoResultadoPsp() { }

        public long CodigoTipoResultado { get; set; }
        public string NomeArquivo { get; set;}
        public string NomeOriginalArquivo { get; set;}
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int State { get; set; }
    }
}
