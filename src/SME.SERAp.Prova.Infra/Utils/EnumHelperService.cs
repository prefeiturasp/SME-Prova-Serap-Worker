using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SME.SERAp.Prova.Infra
{
    public static class EnumHelperService
    {
        public static Dictionary<int, string> MapEnumToDictionary<T>()
        {            
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T precisa ser do tipo Enum.");
            }
            return Enum.GetValues(typeof(T)).Cast<T>().ToDictionary(i => (int)Convert.ChangeType(i, i.GetType()), t => t.ToString());
        }
    }
}
