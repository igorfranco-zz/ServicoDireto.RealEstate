using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpongeSystems.Core.Helpers
{
    public class EnumResourceHelper
	{
		private EnumResourceHelper()
		{
		}

		public static List<Resource<TEnum>> Translate<TEnum>()
		{
            return Translate<TEnum>(System.Threading.Thread.CurrentThread.CurrentUICulture.Name);
		}

		public static List<Resource<TEnum>> Translate<TEnum>( string language )
		{
			// TODO: Implementar, buscar as traduções dos enumerados do arquivo .resx, de acordo com a chave definida pelo atributo ResourceAttribute
			return new List<Resource<TEnum>>();
		}
	}

	public class Resource<TEnum>
	{
		public Resource()
		{
		}

		public string DisplayName
		{
			get;
			set;
		}

		public TEnum EnumValue
		{
			get;
			set;
		}

    }
}

