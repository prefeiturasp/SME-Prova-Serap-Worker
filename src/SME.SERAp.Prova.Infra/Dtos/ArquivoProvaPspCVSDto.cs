using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Infra.Dtos
{
    public class ArquivoProvaPspCVSDto
    {
        [Name("Edicao")]
        public string Edicao { get; set; }
        [Name("AreaConhecimentoID")]
        public int AreaConhecimentoID { get; set; }
        [Name("uad_sigla")]
        public string uad_sigla { get; set; }
        [Name("esc_codigo")]
        public string esc_codigo { get; set; }
        [Name("AnoEscolar")]
        public string AnoEscolar { get; set; }
        [Name("tur_codigo")]
        public string tur_codigo { get; set; }
        [Name("tur_id")]
        public int tur_id { get; set; }
        [Name("alu_matricula")]
        public string alu_matricula { get; set; }
        [Name("alu_nome")]
        public string alu_nome { get; set; }

        [Name("NivelProficienciaID")]
        public int NivelProficienciaID { get; set; }
        [Name("Valor")]
        public string Valor { get; set; }

    }
}
