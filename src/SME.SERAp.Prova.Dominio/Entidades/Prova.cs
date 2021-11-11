using System;

namespace SME.SERAp.Prova.Dominio
{
    public class Prova : EntidadeBase
    {
        public Prova()
        {
            Inclusao = DateTime.Now;
        }
        public Prova(long id, string descricao, DateTime? inicioDownload, DateTime inicio, DateTime fim, int totalItens, long legadoId, int tempoExecucao, string senha, bool possuiBIB, int totalCadernos)
        {
            Id = id;
            Descricao = descricao;
            InicioDownload = inicioDownload;
            Inicio = inicio;
            Fim = fim;
            TotalItens = totalItens;
            LegadoId = legadoId;
            Inclusao = DateTime.Now;
            TempoExecucao = tempoExecucao;
            Senha = senha;
            PossuiBIB = possuiBIB;
            TotalCadernos = totalCadernos;
        }

        public string Descricao { get; set; }
        public DateTime? InicioDownload { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public DateTime Inclusao { get; set; }
        public int TotalItens { get; set; }
        public int TempoExecucao { get; set; }
        public long LegadoId { get; set; }
        public string Senha { get; set; }
        public bool PossuiBIB { get; set; }
        public int TotalCadernos { get; set; }
    }
}
