﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterExtracaoProvaRespostaQueryHandler :
        IRequestHandler<ObterExtracaoProvaRespostaQuery, IEnumerable<ConsolidadoProvaRespostaDto>>
    {
        private readonly IRepositorioResultadoProvaConsolidado repositorioResultadoProvaConsolidado;

        public ObterExtracaoProvaRespostaQueryHandler(IRepositorioResultadoProvaConsolidado repositorioResultadoProvaConsolidado)
        {
            this.repositorioResultadoProvaConsolidado = repositorioResultadoProvaConsolidado ??
                                          throw new ArgumentNullException(nameof(repositorioResultadoProvaConsolidado));
        }

        public async Task<IEnumerable<ConsolidadoProvaRespostaDto>> Handle(ObterExtracaoProvaRespostaQuery request, CancellationToken cancellationToken)
        {
            var resultado = await repositorioResultadoProvaConsolidado.ObterExtracaoProvaRespostaQuery(request.ProvaSerapId, request.DreCodigoEol, request.UeCodigoEol, request.TurmasCodigosEol);
            var alunos = resultado.Select(a => a.AlunoCodigoEol).Distinct();

            var resultadoRetorno = new List<ConsolidadoProvaRespostaDto>();
            foreach (var aluno in alunos)
            {                
                var turmas = await repositorioResultadoProvaConsolidado.ObterTurmasResultadoProvaAluno(request.ProvaSerapId, aluno);
                var turma = turmas.FirstOrDefault();

                var cadernoComResposta = resultado.Where(x => x.AlunoCodigoEol == aluno && x.TurmaCodigo == turma && !String.IsNullOrEmpty(x.Resposta))
                                                  .Select(x => x.ProvaCaderno).FirstOrDefault();
                var caderno = string.Empty;
                if (!String.IsNullOrEmpty(cadernoComResposta))
                    caderno = cadernoComResposta;
                else
                    caderno = resultado.Where(x => x.AlunoCodigoEol == aluno && x.TurmaCodigo == turma).Select(x => x.ProvaCaderno).FirstOrDefault();

                var questoesIds = resultado.Where(x => x.AlunoCodigoEol == aluno && x.TurmaCodigo == turma && x.ProvaCaderno == caderno).Select(x => x.QuestaoId).Distinct();
                foreach (var questoesId in questoesIds)
                {
                    var rq = resultado.Where(x => x.AlunoCodigoEol == aluno && x.TurmaCodigo == turma && x.QuestaoId == questoesId && x.ProvaCaderno == caderno).FirstOrDefault();
                    if (rq != null)
                        resultadoRetorno.Add(rq);
                }
            }

            return resultadoRetorno.AsEnumerable();
        }
    }
}
