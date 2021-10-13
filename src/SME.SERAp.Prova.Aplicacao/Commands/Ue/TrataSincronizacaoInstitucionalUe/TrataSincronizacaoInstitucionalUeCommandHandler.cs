using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TrataSincronizacaoInstitucionalUeCommandHandler : IRequestHandler<TrataSincronizacaoInstitucionalUeCommand, long>
    {
        private readonly IRepositorioUe repositorioUe;

        public TrataSincronizacaoInstitucionalUeCommandHandler(IRepositorioUe repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<long> Handle(TrataSincronizacaoInstitucionalUeCommand request, CancellationToken cancellationToken)
        {
            long idNovaUe = 0;
            if (request.UeSerap == null)
            {
                idNovaUe = await repositorioUe.IncluirAsync(request.UeSgp);
                if (idNovaUe <= 0)
                    throw new NegocioException($"Erro ao incluir nova Ue {request.UeSgp.CodigoUe}");
            }
            else
            {
                if (VerificaSeTemAlteracao(request.UeSgp, request.UeSerap))
                {
                    var ueParaAtualizar = new Ue()
                    {
                        CodigoUe = request.UeSgp.CodigoUe,
                        Nome = request.UeSgp.Nome.Trim(),
                        TipoEscola = request.UeSgp.TipoEscola,
                        Id = request.UeSerap.Id,
                        DreId = request.UeSerap.DreId
                    };
                    idNovaUe = await repositorioUe.UpdateAsync(ueParaAtualizar);
                }
                else
                {
                    return request.UeSerap.Id;
                }
            }
            return idNovaUe;
        }
        private bool VerificaSeTemAlteracao(Ue ueSgp, Ue ueSerap)
        {
            if (ueSgp.Nome.Trim() != ueSerap.Nome.Trim() ||
                ueSgp.TipoEscola != ueSerap.TipoEscola)
                return true;

            return false;
        }
    }
}
