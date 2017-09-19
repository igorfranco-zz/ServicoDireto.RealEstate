using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpongeSolutions.Core.Translation
{
	/// <summary>
	/// Item do enum <typeparamref name="TEnum"/> traduzido
	/// </summary>
	/// <typeparam name="TEnum"></typeparam>
	public class Translation<TEnum>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="displayName"></param>
		/// <param name="value"></param>
		/// <param name="culture"></param>
		public Translation( string displayName, TEnum value, CultureInfo culture )
		{
			this.DisplayName = displayName;
			this.Value = value;
		}

		/// <summary>
		/// Obtém a descrição do item do enumerador
		/// </summary>
		public string DisplayName
		{
			get;
			private set;
		}

		/// <summary>
		/// Obtém o valor do enumerador
		/// </summary>
		public TEnum Value
		{
			get;
			private set;
		}

        /// <summary>
        /// Obtém o valor do enumerador (int)
        /// </summary>
        public int RawValue
        {
            get { return Convert.ToInt32(this.Value); }
        }

        /// <summary>
        /// Obtém o valor do enumerador (string)
        /// </summary>
        public string StringValue
        {
            get { return Convert.ToChar(Convert.ToInt32(this.Value)).ToString();}
        }

		/// <summary>
		/// Obtém o idioma traduzido
		/// </summary>
		public CultureInfo Culture
		{
			get;
			private set;
		}

	}
}
