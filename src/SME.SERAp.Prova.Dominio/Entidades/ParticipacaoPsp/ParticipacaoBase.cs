using Dapper;
using System.Data;

namespace SME.SERAp.Prova.Dominio
{
    public class ParticipacaoBase
    {
        public string Edicao { get; set; }
        public string AnoEscolar { get; set; }
        public int TotalPrevisto { get; set; }
        public int TotalPresente { get; set; }
        public decimal? PercentualParticipacao { get; set; }

        public DynamicParameters ObterParametrosBase()
        {
            var parametros = new DynamicParameters();
            parametros.Add("@Edicao", this.Edicao, DbType.String, ParameterDirection.Input, 10);
            parametros.Add("@AnoEscolar", this.AnoEscolar, DbType.String, ParameterDirection.Input, 3);
            parametros.Add("@TotalPrevisto", this.TotalPrevisto, DbType.Int32, ParameterDirection.Input, null);
            parametros.Add("@TotalPresente", this.TotalPresente, DbType.Int32, ParameterDirection.Input, null);
            parametros.Add("@PercentualParticipacao", this.PercentualParticipacao, DbType.Decimal, ParameterDirection.Input, null, 6, 2);

            return parametros;
        }
    }
}
