using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace SpongeSystems.Core.Service
{
	public static class ServiceLocator
	{
		private static readonly UnityContainer _unityContainer;

		static ServiceLocator()
		{
			_unityContainer = new UnityContainer();
			UnityConfigurationSection section = (UnityConfigurationSection) ConfigurationManager.GetSection( "unity" );
			section.Containers.Default.Configure( _unityContainer );
		}

		public static T Retrieve<T>()
		{
			return _unityContainer.Resolve<T>();
		}

		public static T Retrieve<T>( string key )
		{
			return _unityContainer.Resolve<T>( key );
		}
	}
}