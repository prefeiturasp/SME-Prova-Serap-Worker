namespace SME.SERAp.Prova.Infra
{
    public class PreferenciaUsuarioDto
    {
        public PreferenciaUsuarioDto()
        {

        }
        public PreferenciaUsuarioDto(long alunoRA, int tamanhoFonte, int familiaFonte)
        {
            AlunoRA = alunoRA;
            TamanhoFonte = tamanhoFonte;
            FamiliaFonte = familiaFonte;
        }

        public long AlunoRA { get; set; }
        public int TamanhoFonte { get; set; }
        public int FamiliaFonte { get; set; }
    }
}