using System;

namespace SME.SERAp.Prova.Infra
{
    public class ProvaLegadoDetalhesIdDto
    {
        public long Id { get; set; }
        public string Descricao { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
        public int TotalItens { get; set; }
    }
}
