using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlternativaArquivoPersistirCommand : IRequest<long>
    {
        public AlternativaArquivoPersistirCommand(long alternativaId, long arquivoId)
        {
            AlternativaId = alternativaId;
            ArquivoId = arquivoId;
        }

        public long AlternativaId { get; set; }
        public long ArquivoId { get; set; }
    }
}
