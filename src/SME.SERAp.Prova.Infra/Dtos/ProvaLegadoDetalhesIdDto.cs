using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SERAp.Prova.Infra
{
    public class ProvaLegadoDetalhesIdDto : DtoBase
    {
        public ProvaLegadoDetalhesIdDto()
        {
            Anos = new List<string>();
        }
        public long Id { get; set; }
        public string Descricao { get; set; }
        public string Disciplina { get; set; }
        public DateTime? InicioDownload { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
        public int TotalItens { get; set; }
        public int TempoExecucao { get; set; }
        public string Senha { get; set; }
        public List<string> Anos { get; set; }
        public bool PossuiBIB { get; set; }
        public int TotalCadernos { get; set; }
        public ModalidadeSerap Modalidade { get; set; }
        public ModeloProva ModeloProva { get; set; }
        public bool OcultarProva { get; set; }
        public bool AderirTodos { get; set; }
        public bool Multidisciplinar { get; set; }
        public int TipoProva { get; set; }
        public bool FormatoTai { get; set; }

        public int? QtdItensSincronizacaoRespostas { get; set; }

        public void AddAno(string ano)
        {
            if (!string.IsNullOrEmpty(ano))
                if (!Anos.Any(a => a == ano))
                    Anos.Add(ano);
        }
    }
}
