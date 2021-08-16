using MediatR;
using SME.SERAp.Prova.Dominio;
using System;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExecucaoControleAtualizarCommand : IRequest<long>
    {
        public ExecucaoControle ExecucaoControle { get; set; }
        public ExecucaoControleAtualizarCommand(ExecucaoControle execucaoControle)
        {
            ExecucaoControle = execucaoControle;
            ExecucaoControle.UltimaExecucao = DateTime.Now;
        }

    }
}
