using System;

namespace SME.SERAp.Prova.Dominio
{
    public class Dre : EntidadeBase
    {        
        public string CodigoDre { get; set; }
        public string Nome { get; set; }
        public string Abreviacao { get; set; }
        public DateTime DataAtualizacao { get; set; }        
    }
}
