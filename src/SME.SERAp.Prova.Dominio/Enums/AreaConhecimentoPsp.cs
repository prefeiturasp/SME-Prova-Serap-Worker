using System.ComponentModel.DataAnnotations;

namespace SME.SERAp.Prova.Dominio.Enums
{
    public enum AreaConhecimentoPsp
    {
        [Display(Name = "Ciências da Natureza")]
        CienciasNatureza = 1,

        [Display(Name = "Língua Portuguesa")]
        LinguaPortuguesa = 2,

        [Display(Name = "Matemática")]
        Matematica = 3,

        [Display(Name = "Redação")]
        Redacao = 4
    }
}
