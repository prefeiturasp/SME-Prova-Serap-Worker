using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAbrangenciaPorUsuarioGrupoDreUeTurmaQuery : IRequest<Abrangencia>
    {
        public ObterAbrangenciaPorUsuarioGrupoDreUeTurmaQuery(long usuarioId, long grupoId, long? dreId, long? ueId, long? turmaId)
        {
            UsuarioId = usuarioId;
            GrupoId = grupoId;
            DreId = dreId.GetValueOrDefault();
            UeId = ueId.GetValueOrDefault();
            TurmaId = turmaId.GetValueOrDefault();
        }

        public long UsuarioId { get; set; }
        public long GrupoId { get; set; }
        public long DreId { get; set; }
        public long UeId { get; set; }
        public long TurmaId { get; set; }
    }
}
