namespace SME.SERAp.Prova.Infra
{
    public class RegistroProficienciaPspCsvDto
    {

        public RegistroProficienciaPspCsvDto()
        {

        }

        public RegistroProficienciaPspCsvDto(long processoId, object registro)
        {
            ProcessoId = processoId;
            Registro = registro;
        }

        public long ProcessoId { get; set; }
        public object Registro { get; set; }

        public T ObterObjetoRegistro<T>() where T : class
        {
            return Registro?.ToString().ConverterObjectStringPraObjeto<T>();
        }

    }
}
