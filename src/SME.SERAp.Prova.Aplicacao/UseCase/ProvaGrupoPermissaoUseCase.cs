using MediatR;
using SME.SERAp.Prova.Aplicacao.Queries;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.UseCase
{
    public class ProvaGrupoPermissaoUseCase : AbstractUseCase, IProvaGrupoPermissaoUseCase
    {
        private readonly IServicoLog servicoLog;

        public ProvaGrupoPermissaoUseCase(IMediator mediator, IServicoLog servicoLog) : base(mediator)
        {
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var idsProvaDto = mensagemRabbit.ObterObjetoMensagem<ProvaIdsDto>();
            
            if (idsProvaDto.ProvaId <= 0 || idsProvaDto.ProvaLegadoId <= 0)
            {
                servicoLog.Registrar(Dominio.Enums.LogNivel.Negocio, $"O id das provas estão incorretos, ProvaId:{idsProvaDto.ProvaId} e ProvaLegado: {idsProvaDto.ProvaLegadoId }");
                return false;
            }

            var permissoesGrupoProvaLegado = await mediator.Send(new ObterPermissoesProvaLegadoQuery(idsProvaDto.ProvaLegadoId));
            var gruposSerapEstudantesCoreSso = await mediator.Send(new ObterGruposSerapCoreSsoQuery());

            if (permissoesGrupoProvaLegado == null)
            {
                servicoLog.Registrar(Dominio.Enums.LogNivel.Informacao, ($"Permissoes da prova:  {idsProvaDto.ProvaLegadoId} não localizada no serap!"));
                return false;
            }

            var listaPermissoes = new List<ProvaGrupoPermissao>();
            var listaProvasGrupoPermissao = (await mediator.Send(new ObterProvaGruposPermissaoQuery(idsProvaDto.ProvaId))).ToList();

            if (!listaProvasGrupoPermissao.Any())
                await IncluiListaPermissoes(idsProvaDto, permissoesGrupoProvaLegado, gruposSerapEstudantesCoreSso, listaPermissoes);
            else
                await AlteraListaPermissoes(permissoesGrupoProvaLegado, gruposSerapEstudantesCoreSso, listaPermissoes, listaProvasGrupoPermissao);

            return true;
        }

        private async Task IncluiListaPermissoes(ProvaIdsDto idsProvaDto, IEnumerable<ProvaGrupoPermissaoDto> permissoesGrupoProvaLegado, IEnumerable<GrupoSerapCoreSso> gruposSerapEstudantesCoreSso, List<ProvaGrupoPermissao> listaPermissoes)
        {
            foreach (var permissaoGrupo in permissoesGrupoProvaLegado)
            {
                var grupoPermissao = gruposSerapEstudantesCoreSso.FirstOrDefault(g => g.IdCoreSso == permissaoGrupo.GrupoCoressoId);

                if (grupoPermissao == null)
                {
                    servicoLog.Registrar(Dominio.Enums.LogNivel.Critico, ($"O GrupoId {permissaoGrupo.GrupoCoressoId}  não encontrado na base do serap estudantes!"));
                    continue;
                }
                
                var entidadePermissao = new ProvaGrupoPermissao(idsProvaDto.ProvaId, permissaoGrupo.ProvaLegadoId, grupoPermissao.Id, permissaoGrupo.OcultarProva);
                listaPermissoes.Add(entidadePermissao);
            }

            if (listaPermissoes.Count > 0)
                await mediator.Send(new IncluirProvaGrupoPermissaoCommand(listaPermissoes));
        }

        private async Task AlteraListaPermissoes(IEnumerable<ProvaGrupoPermissaoDto> permissoesGrupoProvaLegado, IEnumerable<GrupoSerapCoreSso> gruposSerapEstudantesCoreSso, List<ProvaGrupoPermissao> listaPermissoes, IEnumerable<ProvaGrupoPermissao> listaProvasGrupoPermissao)
        {
            foreach (var provaGrupoPermissao in listaProvasGrupoPermissao)
            {
                var grupoPermissaoCoressoSerapEstudante = gruposSerapEstudantesCoreSso.FirstOrDefault(x => x.Id == provaGrupoPermissao.GrupoId);
                
                if (grupoPermissaoCoressoSerapEstudante == null)
                    continue;
                
                var permissaoLegado = permissoesGrupoProvaLegado.FirstOrDefault(p => p.GrupoCoressoId == grupoPermissaoCoressoSerapEstudante.IdCoreSso);
                
                if (permissaoLegado == null)
                    continue;

                provaGrupoPermissao.OcultarProva = permissaoLegado.OcultarProva;
                provaGrupoPermissao.AlteradoEm = DateTime.Now;

                listaPermissoes.Add(provaGrupoPermissao);
            }

            await mediator.Send(new AlterarProvaGrupoPermissaoCommand(listaPermissoes));
        }
    }
}