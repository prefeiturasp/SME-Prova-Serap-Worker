using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Dominio.Entidades
{
    public class ResultadoAluno
    {
      
        public string Edicao { get; set; }
        public int AreaConhecimentoID { get; set; }
        public string uad_sigla { get; set; }
        public string esc_codigo { get; set; }
        public string AnoEscolar { get; set; }
        public string tur_codigo { get; set; }
        public int tur_id { get; set; }
        public string alu_matricula { get; set; }
        public string alu_nome { get; set; }
      //  public int? ResultadoLegadoId { get; set; }
        public int NivelProficienciaID { get; set; }
        public decimal Valor { get; set; }
    }
}
