using System.ComponentModel.DataAnnotations;

namespace SME.SERAp.Prova.Dominio
{
    public enum ModeloProva
    {
        NaoCadastrado = 0,

        [Display(Name = "Modelo Teste publicação")]
        Modelo = 1,

        [Display(Name = "Prova teste Mais Educação - 1 semestre 2016")]
        PTME = 2,

        [Display(Name = "Provinha Brasil 2016 Teste 1 MT")]
        PB2016 = 3,

        [Display(Name = "Prova Diagnóstica SP")]
        PDSP = 4,

        [Display(Name = "Prova Semestral")]
        PS = 5,

        [Display(Name = "Prova Semestral 2017 - Surdos")]
        PS2017 = 6,

        [Display(Name = "EJA")]
        EJA = 7,

        [Display(Name = "Simulado_ENEM")]
        ENEM = 8,

        [Display(Name = "EJA_ENEM")]
        EJAENEM = 10,

        [Display(Name = "Provinha São Paulo")]
        ProvinhaSP = 11,

        [Display(Name = "Prova São Paulo")]
        ProvaSP = 12
    }
}
