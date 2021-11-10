using System.ComponentModel.DataAnnotations;

namespace SME.SERAp.Prova.Dominio
{
    public enum ModalidadeSerap
    {
        [Display(Name = "Educação Infantil")]
        EducacaoInfantil = 1,
        [Display(Name = "Ensino Fundamental")]
        EnsinoFundamental = 2,
        [Display(Name = "Ensino Médio")]
        EnsinoMedio = 3,
        [Display(Name = "EJA - Ensino Fundamental")]
        EJAEnsinoFundamental = 4,
        [Display(Name = "EJA - CIEJA")]
        EJACIEJA = 5,
        [Display(Name = "EJA ESCOLAS EDUCACAO ESPECIAL")]
        EJAEscolasEducacaoEspecial = 6
    }
}
