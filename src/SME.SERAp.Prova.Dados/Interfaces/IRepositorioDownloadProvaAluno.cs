using SME.SERAp.Prova.Dominio;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioDownloadProvaAluno : IRepositorioBase<DownloadProvaAluno>
    {
        Task<bool> ExcluirDownloadProvaAluno(Guid[] codigos, DateTime? dataAlteracao);
    }
}
