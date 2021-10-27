using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SERAp.Prova.Infra
{
    public class ProvaLegadoDetalhesIdDto
    {
        public ProvaLegadoDetalhesIdDto()
        {
            Anos = new List<int>();
        }
        public long Id { get; set; }
        public string Descricao { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
        public int TotalItens { get; set; }
        public int TempoExecucao { get; set; }
        public string Senha { get; set; }
        public List<int> Anos { get; set; }
        public bool PossuiBIB { get; set; }
        public int TotalCadernos { get; set; }

        public void AddAno(int ano)
        {
            if (ano > 0)
                if (!Anos.Any(a => a == ano))
                    Anos.Add(ano);
        }
    }
}
