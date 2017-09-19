using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpongeSolutions.Core
{
	public static class ExpressionValidators
	{
		public static string GetEmailValidation()
		{
			return @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
		}

		public static string GetNumberValidation()
		{
			return @"^\d+$";
		}

		public static string GetMaxLengthValidation( int maxlength )
		{
			return @"^[\s\S]{0," + maxlength + "}$";
		}

        public static string GetOnlyNumbers 
        {
            get
            {
                return @"(\+|-)?[0-9][0-9]*(\.[0-9]*)?";
            } 
        }

        public static string GetOnlyCharacters 
        {
            get { return @"[A-Za-z][A-Za-z]*(\.)?"; }
        }
	}
}