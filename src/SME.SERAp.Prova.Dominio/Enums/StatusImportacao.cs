using System.ComponentModel.DataAnnotations;

namespace SME.SERAp.Prova.Dominio.Enums
{
    public enum StatusImportacao
    {
        [Display(Name = "Pendente")]
        Pendente = 1,

        [Display(Name = "Em andamento")]
        EmAndamento = 2,

        [Display(Name = "Processado")]
        Processado = 3,

        [Display(Name = "Erro")]
        Erro = 4
      }
}
