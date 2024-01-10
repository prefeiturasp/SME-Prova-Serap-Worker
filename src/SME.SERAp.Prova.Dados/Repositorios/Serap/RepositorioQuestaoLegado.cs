using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioQuestaoLegado : RepositorioSerapLegadoBase, IRepositorioQuestaoLegado
    {
        public RepositorioQuestaoLegado(ConnectionStringOptions connectionStrings) : base(connectionStrings) { }

        public async Task<IEnumerable<ItemAmostraTaiDto>> ObterItensAmostraTai(long matrizId, int tipoCurriculoGradeId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select
								i.Id ItemId,
								i.ItemCode ItemCodigo,
                                i.Statement as Enunciado, 
                                i.EvaluationMatrix_Id as MatrizId,
								icg.TypeCurriculumGradeId TipoCurriculoGradeId,
								s.Id HabilidadeId,
								s.[Description] HabilidadeNome,
								s.Code HabilidadeCodigo,
								sub.Id AssuntoId,
								sub.[Description] AssuntoNome,
								ss.Id SubAssuntoId,
								ss.[Description] SubAssuntoNome,
								i.TRIDiscrimination Discriminacao,
								i.TRIDifficulty ProporcaoAcertos,
								i.TRICasualSetting AcertoCasual,
								IT.QuantityAlternative QuantidadeAlternativas,
								case when IT.QuantityAlternative > 0 then 
								    1 
								else 
								    2 
								end TipoItem,
								bt.Description as TextoBase
							from Item i
							    inner join ITemType it on i.ItemType_Id = it.id and it.State = 1
								inner join ItemCurriculumGrade icg on i.Id = icg.Item_id and icg.State = 1
								inner join ItemSkill its on i.Id = its.Item_Id and its.State = 1
								inner join Skill s on its.Skill_Id = s.Id and s.State = 1
								inner join SubSubject ss on i.SubSubject_Id = ss.Id and ss.State = 1
								inner join [Subject] sub on ss.Subject_Id = sub.Id and sub.State = 1
								inner join BaseText bt on bt.Id = I.BaseText_Id and bt.State = 1
							where i.[State] = 1
								and s.Parent_Id is not null
								and i.EvaluationMatrix_Id = @matrizId
								and i.TRIDiscrimination is not null
								and i.TRIDifficulty is not null
								and i.TRICasualSetting is not null
								and icg.TypeCurriculumGradeId = @tipoCurriculoGradeId
								and i.ItemVersion = (select max(i2.ItemVersion) from Item i2 where i2.Id = i.Id)";

                return await conn.QueryAsync<ItemAmostraTaiDto>(query, new { matrizId, tipoCurriculoGradeId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
