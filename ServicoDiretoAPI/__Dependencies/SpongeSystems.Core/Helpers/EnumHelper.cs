using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpongeSystems.Core.Helpers
{
    public class EnumHelper
    {
        public static TEnum TryParse<TEnum>(object value)   where TEnum : struct 
        {
            TEnum result;
            Enum.TryParse<TEnum>(value.ToString(), out result);
            return result;
        }
    }
}
