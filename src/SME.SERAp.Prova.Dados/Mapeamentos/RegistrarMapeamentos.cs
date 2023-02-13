using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using SME.SERAp.Prova.Dados.Mapeamentos;

namespace SME.SERAp.Prova.Dados
{
    public static class RegistrarMapeamentos
    {
        public static void Registrar()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new ProvaAnoMap());
                config.AddMap(new ProvaMap());
                config.AddMap(new ExecucaoControleMap());
                config.AddMap(new QuestaoMap());
                config.AddMap(new AlternativasMap());
                config.AddMap(new ArquivoMap());
                config.AddMap(new QuestaoArquivoMap());
                config.AddMap(new DreMap());
                config.AddMap(new UeMap());
                config.AddMap(new TurmaMap());
                config.AddMap(new AlunoMap());
                config.AddMap(new CadernoAlunoMap());
                config.AddMap(new QuestaoAlunoRespostaMap());
                config.AddMap(new ContextoProvaMap());
                config.AddMap(new PreferenciasUsuarioMap());
                config.AddMap(new AlternativaArquivoMap());
                config.AddMap(new ProvaAlunoMap());
                config.AddMap(new ExportacaoResultadoMap());
                config.AddMap(new ResultadoProvaConsolidadoMap());
                config.AddMap(new ExportacaoResultadoItemMap());
                config.AddMap(new ParametroSistemaMap());
                config.AddMap(new ProvaAdesaoMap());
                config.AddMap(new QuestaoAudioMap());
                config.AddMap(new TipoProvaMap());
                config.AddMap(new TipoDeficienciaMap());
                config.AddMap(new TipoProvaDeficienciaMap());
                config.AddMap(new AlunoDeficienciaMap());
                config.AddMap(new QuestaoVideoMap());
                config.AddMap(new TurmaAlunoHistoricoMap());
                config.AddMap(new UsuarioSerapCoreSsoMap());
                config.AddMap(new GrupoSerapCoreSsoMap());
                config.AddMap(new UsuarioGrupoSerapCoreSsoMap());
                config.AddMap(new AbrangenciaMap());
                config.AddMap(new DownloadProvaAlunoMap());
                config.AddMap(new AlunoProvaProficienciaMap());
                config.AddMap(new VersaoAppDispositivoMap());
                config.AddMap(new UsuarioMap());
                config.AddMap(new UsuarioDispositivoMap());
                config.AddMap(new ProvaAlunoReaberturaMap());
                config.AddMap(new ProvaGrupoPermissaoMap());
                config.AddMap(new QuestaoTriMap());
                config.AddMap(new ResultadoAlunoMap());

                config.ForDommel();
            });
        }
    }
}
