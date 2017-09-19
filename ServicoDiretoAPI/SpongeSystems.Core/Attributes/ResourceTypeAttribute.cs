using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpongeSolutions.Core.Attributes
{
	/// <summary>
	/// Define de onde será obtido o respositório com as traduções do enumerador
	/// </summary>
	[AttributeUsage( AttributeTargets.Enum, AllowMultiple = false )]
	public class ResourceTypeAttribute : Attribute
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="resourceType"></param>
		public ResourceTypeAttribute( Type resourceType )
		{
			if ( resourceType == null ) throw new ArgumentNullException( "resourceType" );
			this.ResourceType = resourceType;
		}

		/// <summary>
		/// 
		/// </summary>
		public Type ResourceType
		{
			get;
			private set;
		}
	}
}
