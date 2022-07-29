namespace SME.SERAp.Prova.Infra
{
    public static class RotasRabbit
    {
        public const string ProvaSync = "serap.estudante.prova.legado.sync";       
        public const string ProvaTratar = "serap.estudante.prova.legado.tratar";
        public const string ProvaAnoTratar = "serap.estudante.prova.ano.legado.tratar";
        public const string QuestaoSync = "serap.estudante.questao.legado.sync";       
        public const string QuestaoTratar = "serap.estudante.questao.legado.tratar";
        public const string QuestaoImagemIncorretaTratar = "serap.estudante.questoes.imagens.tratar";
        public const string AlternativaImagemIncorretaTratar = "serap.estudante.alternativas.imagens.tratar";
        public const string AlternativaSync = "serap.estudante.alternativa.legado.sync";       
        public const string AlternativaTratar = "serap.estudante.alternativa.legado.tratar";
        public const string ProvaBIBSync = "serap.estudante.provabib.sync";
        public const string ProvaBIBTratar = "serap.estudante.provabib.tratar";
        public const string ProvaWebPushTeste = "serap.estudante.prova.webpush.teste";
        public const string IncluirRespostaAluno = "serap.estudante.resposta.aluno.incluir";
        public const string IncluirPreferenciasAluno = "serap.estudante.preferencias.aluno.incluir";
        public const string AtualizarFrequenciaAlunoProvaSync = "serap.estudante.atualizar.frequencia.sync";
        public const string AtualizarFrequenciaAlunoProvaTratar = "serap.estudante.atualizar.frequencia.tratar";
        public const string ExtrairResultadosProva = "serap.estudante.prova.resultados.aluno.extrair";
        public const string ExtrairResultadosProvaFiltro = "serap.estudante.prova.resultados.aluno.extrair.filtro";
        public const string ConsolidarProvaResultado = "serap.estudante.prova.resultados.consolidar";
        public const string ConsolidarProvaResultadoFiltro = "serap.estudante.prova.resultados.consolidar.filtro";
        public const string ConsolidarProvaResultadoFiltroTurma = "serap.estudante.prova.resultados.consolidar.filtro.turma";
        public const string TratarAdesaoProva = "serap.estudante.prova.adesao.tratar";
        public const string TratarTipoProvaDeficiencia = "serap.estudante.prova.tipo.prova.deficiencia.tratar";
        public const string TratarAlunoDeficiencia = "serap.estudante.aluno.deficiencia.tratar";
        public const string DownloadProvaAlunoTratar = "serap.estudante.download.prova.aluno.tratar";
        public const string AlunoProvaProficienciaAsync = "serap.estudante.aluno.prova.proficiencia.sync";
        public const string AlunoProvaProficienciaTratar = "serap.estudante.aluno.prova.proficiencia.tratar";

        public const string IniciarProcessoFinalizarProvasAutomaticamente = "serap.estudante.prova.finalizar.automaticamente.iniciar";
        public const string FinalizarProvaAutomaticamente = "serap.estudante.prova.finalizar.automaticamente";

        public const string SincronizaEstruturaInstitucionalDreSync = "serap.sincronizacao.institucional.dre.sync";
        public const string SincronizaEstruturaInstitucionalDreTratar = "serap.sincronizacao.institucional.dre.tratar";
        public const string SincronizaEstruturaInstitucionalUesSync = "serap.sincronizacao.institucional.ue.sync";
        public const string SincronizaEstruturaInstitucionalUeTratar = "serap.sincronizacao.institucional.ue.tratar";
        public const string SincronizaEstruturaInstitucionalTurmasSync = "serap.sincronizacao.institucional.turma.sync";
        public const string SincronizaEstruturaInstitucionalTurmaTratar = "serap.sincronizacao.institucional.turma.tratar";
        public const string SincronizaEstruturaInstitucionalAlunoSync = "serap.sincronizacao.institucional.aluno.sync";
        public const string SincronizaEstruturaInstitucionalAlunoTratar = "serap.sincronizacao.institucional.aluno.tratar";
        public const string SincronizaEstruturaInstitucionalTurmaAlunoHistoricoSync = "serap.sincronizacao.institucional.turma.aluno.historico.sync";
        public const string SincronizaEstruturaInstitucionalTurmaAlunoHistoricoTratar = "serap.sincronizacao.institucional.turma.aluno.historico.tratar";
        public const string SincronizaEstruturaInstitucionalAtualizarUeTurma = "serap.sincronizacao.institucional.turma.atualizar.ue";

        public const string UsuarioCoreSsoSync = "serap.estudante.usuario.coresso.sync";
        public const string UsuarioPorGrupoCoreSsoTratar = "serap.estudante.usuario.grupo.coresso.tratar";
        public const string UsuarioCoreSsoTratar = "serap.estudante.usuario.coresso.tratar";
        public const string UsuarioGrupoCoreSsoExcluirTratar = "serap.estudante.usuario.grupo.coresso.excluir.tratar";

        public const string UsuarioGrupoAbrangenciaTratar = "serap.estudante.usuario.grupo.abrangencia.tratar";
        public const string GrupoAbrangenciaExcluir = "serap.estudante.grupo.abrangencia.excluir";
        public const string UsuarioGrupoAbrangenciaExcluirTratar = "serap.estudante.usuario.grupo.abrangencia.excluir.tratar";

        public const string FilaDeadletterTratar = "serap.deadletter.tratar";        
        public const string FilaDeadletterSync = "serap.deadletter.sync";


        public const string QuestaoCompletaSync = "serap.estudante.questao.completa.legado.sync";
        public const string QuestaoCompletaTratar = "serap.estudante.questao.completa.legado.tratar";

        public const string TratarCadernosProvaTai = "serap.estudante.prova.legado.tratar.cadernos.amostra.tai";
        public const string TratarCadernoAlunoProvaTai = "serap.estudante.prova.legado.tratar.caderno.aluno.tai";


        public const string IncluirUsuario = "serap.estudantes.usuario.incluir";
        public const string AlterarUsuario = "serap.estudantes.usuario.alterar";
        public const string IncluirVersaoDispositivoApp = "serap.estudantes.versaoAppDispositivo.incluir";
        public const string IncluirProvaAluno = "serap.estudantes.provaAluno.incluir";
        public const string AlterarProvaAluno = "serap.estudantes.provaAluno.alterar";

        public static string RotaLogs => "ApplicationLog";
    }
}
