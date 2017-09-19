using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpongeSolutions.Core.Translation
{
	/// <summary>
	/// Coleção com a tradução dos itens do enumerador <typeparamref name="TEnum"/>
	/// </summary>
	/// <typeparam name="TEnum"></typeparam>
	public class TanslationCollection<TEnum> : List<Translation<TEnum>>
	{ }
}
