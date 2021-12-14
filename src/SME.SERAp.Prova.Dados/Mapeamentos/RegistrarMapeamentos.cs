using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;

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
                config.AddMap(new ParametroSistemaMap());

                config.ForDommel();
            });
        }
    }
}
