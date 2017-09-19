using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using System.Security.Cryptography;

namespace SpongeSolutions.Core
{
	public class Cryptography
	{
		/// <summary>
		/// Criptografa um valor no algoritmo Simétrico de Rijndael do Enterprise Library 4. Com método definido no webconfig
		/// </summary>
		/// <param name="value">Valor em texto pleno a ser criptografado</param>
		/// <returns>Retorna um valor criptografado</returns>
		public static string EncryptSymmetric( string value )
		{
			return Cryptographer.EncryptSymmetric( "SpongeBuilderKey", value );
		}

		/// <summary>
		/// Descriptografa um valor no algoritmo Simétrico de Rijndael do Enterprise Library 4. Com método definido no webconfig
		/// </summary>
		/// <param name="value">Valor criptografado a ser descriptografado</param>
		/// <returns>Retorna o valor em texto pleno do valor passado por parâmentro</returns>
		public static string DecryptSymmetric( string value )
		{
            return Cryptographer.DecryptSymmetric("SpongeBuilderKey", value);
		}

		/// <summary>
		/// Criptografa um valor no algoritmo SHA1
		/// </summary>
		/// <param name="value">valor a ser criptografado</param>
		/// <returns>Retorna valor criptografado</returns>
		public static string EncryptSHA1( string value )
		{
			SHA1 sha = new SHA1Managed();
			ASCIIEncoding encoding = new ASCIIEncoding();
			byte[] data = encoding.GetBytes( value );
			byte[] digest = sha.ComputeHash( data );
			return Convert.ToBase64String( digest ).Replace( " ", "0" );
		}
	}
}