using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Infra
{
    public class ObjResultadoPspDto
    {
        public ObjResultadoPspDto()
        {

        }

        public ObjResultadoPspDto(TipoResultadoPsp tipoResultado, object resultado)
        {
            TipoResultado = tipoResultado;
            Resultado = resultado;
        }

        public TipoResultadoPsp TipoResultado { get; set; }
        public object Resultado { get; set; }

        public T ObterObjetoResultado<T>() where T : class
        {
            return Resultado?.ToString().ConverterObjectStringPraObjeto<T>();
        }

    }
}
