using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TrataSincronizacaoInstitucionalDreCommandHandler : IRequestHandler<TrataSincronizacaoInstitucionalDreCommand, long>
    {
        private readonly IRepositorioDre repositorioDre;

        public TrataSincronizacaoInstitucionalDreCommandHandler(IRepositorioDre repositorioDre)
        {
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
        }

        public async Task<long> Handle(TrataSincronizacaoInstitucionalDreCommand request, CancellationToken cancellationToken)
        {
            long novoDreId = 0;
            if (request.DreSerap == null)
            {
                novoDreId = await repositorioDre.IncluirAsync(request.DreSgp);
                if (novoDreId <= 0)
                    throw new NegocioException($"Erro ao incluir nova Dre {request.DreSgp.CodigoDre}");
            }
            else
            {
                if (VerificaSeTemAlteracao(request.DreSgp, request.DreSerap))
                {
                    var dreParaAtualizar = new Dre()
                    {
                        Id = request.DreSerap.Id,
                        CodigoDre = request.DreSgp.CodigoDre,
                        Abreviacao = request.DreSgp.Abreviacao,
                        Nome = request.DreSgp.Nome,
                        DataAtualizacao = request.DreSgp.DataAtualizacao
                    };

                    novoDreId = await repositorioDre.UpdateAsync(dreParaAtualizar);
                }
                else
                {
                    return request.DreSerap.Id;
                }
            }

            return novoDreId;
        }
        private bool VerificaSeTemAlteracao(Dre dreSgp, Dre dreSerap)
        {
            if (dreSgp.Nome != dreSerap.Nome.Trim() ||
                dreSgp.Abreviacao != dreSerap.Abreviacao)
                return true;

            return false;
        }
    }
}
