using System.ComponentModel.DataAnnotations;

namespace SME.SERAp.Prova.Dominio
{
    public enum TipoTurnoSerapLegado
    {
        [Display(Name = "Integral")]
        Integral = 1,
        [Display(Name = "Vespertino")]
        Vespertino = 2,
        [Display(Name = "Noite")]
        Noite = 3,
        [Display(Name = "Intermediário")]
        Intermediario = 4,
        [Display(Name = "Tarde")]
        Tarde = 5,
        [Display(Name = "Manhã")]
        Manha = 6,
    }
}
