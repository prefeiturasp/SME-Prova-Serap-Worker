using MessagePack;

namespace SME.SERAp.Prova.Infra
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class TurmaAtribuicaoDto
    {
        public int DreId { get; set; }
        public int UeId { get; set; }
        public int TurmaId { get; set; }
    }
}
