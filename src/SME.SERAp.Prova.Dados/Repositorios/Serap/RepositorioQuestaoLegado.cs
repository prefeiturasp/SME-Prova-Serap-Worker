﻿using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioQuestaoLegado : RepositorioSerapLegadoBase, IRepositorioQuestaoLegado
    {
        public RepositorioQuestaoLegado(ConnectionStringOptions connectionStrings) : base(connectionStrings) { }

        public async Task<IEnumerable<ItemAmostraTaiDto>> ObterItensAmostraTai(long matrizId, int[] tipoCurriculoGradeIds)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @$"select
								i.Id ItemId,
								i.ItemCode ItemCodigo,
								icg.TypeCurriculumGradeId TipoCurriculoGradeId,
								s.Id HabilidadeId,
								s.[Description] HabilidadeNome,
								s.Code HabilidadeCodigo,
								sub.Id AssuntoId,
								sub.[Description] AssuntoNome,
								ss.Id SubAssuntoId,
								ss.[Description] SubAssuntoNome
							from Item i
								inner join ItemCurriculumGrade icg on i.Id = icg.Item_id
								inner join ItemSkill its on i.Id = its.Item_Id
								inner join Skill s on its.Skill_Id = s.Id
								inner join SubSubject ss on i.SubSubject_Id = ss.Id
								inner join [Subject] sub on ss.Subject_Id = sub.Id
							where i.[State] = 1 and icg.[State] = 1 and s.[State] = 1
								and ss.[State] = 1 and sub.[State] = 1
								and s.Parent_Id is not null
								and i.EvaluationMatrix_Id = @matrizId
								and icg.TypeCurriculumGradeId in({string.Join(",", tipoCurriculoGradeIds)})";

                return await conn.QueryAsync<ItemAmostraTaiDto>(query, new { matrizId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
