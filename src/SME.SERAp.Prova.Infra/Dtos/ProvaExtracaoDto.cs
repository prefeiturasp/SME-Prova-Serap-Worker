using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Infra
{
    public class ProvaExtracaoDto : DtoBase
    {
        public long ProvaSerapId { get; set; }
        public long ExtracaoResultadoId { get; set; }
        public ExportacaoResultadoStatus Status { get; set; }
        public bool AderirTodos { get; set; }
        public bool ParaEstudanteComDeficiencia { get; set; }
        public string CaminhoArquivo { get; set; } = null;

        public int Take { get; set; }
        public int Skip { get; set; }


        public ProvaExtracaoDto()
        {

        }
        
    }
}