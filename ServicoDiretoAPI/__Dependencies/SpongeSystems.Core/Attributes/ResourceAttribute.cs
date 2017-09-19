using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpongeSystems.Core.Attributes
{
	[AttributeUsage( AttributeTargets.All, AllowMultiple = false )]
	public class ResourceAttribute : Attribute
	{
		private string keyName;

		public ResourceAttribute( string keyName )
		{
			this.keyName = keyName;
		}

		public string KeyName
		{
			get
			{
				return this.keyName;
			}
		}
	}
}