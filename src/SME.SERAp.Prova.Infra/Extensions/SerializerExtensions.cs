using System.Text.Json;

namespace SME.SERAp.Prova.Infra
{
    public static class JsonSerializerExtensions
    {
        private static JsonSerializerOptions ObterConfigSerializer()
        {
            return new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                IgnoreNullValues = true
            };            
        }
        
        public static T ConverterObjectStringPraObjeto<T>(this string objectString)
        {
            return JsonSerializer.Deserialize<T>(objectString, ObterConfigSerializer());
        }

        public static string ConverterObjectParaJson(this object obj)
        {
            return JsonSerializer.Serialize(obj, ObterConfigSerializer());
        }
    }
}