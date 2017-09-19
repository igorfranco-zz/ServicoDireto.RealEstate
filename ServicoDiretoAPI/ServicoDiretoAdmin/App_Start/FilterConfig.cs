using SpongeSolutions.Core.Attributes;
using System.Web;
using System.Web.Mvc;

namespace SpongeSolutions.ServicoDireto.Admin
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LogonAuthorize());
            filters.Add(new HandleErrorAttribute());
        }
    }
}