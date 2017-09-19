

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Mvc;
using SpongeSolutions.Core.Cache;
using SpongeSolutions.Core.Helpers;
using SpongeSolutions.ServicoDireto.Model.AccountSystem;

namespace SpongeSolutions.ServicoDireto.Admin
{
    public class SiteContext : BaseController
    {
        public class Roles
        {
            public const string ADMINISTRATOR = "Administrator";
            public const string USER = "User";
        }
        
        private static AssemblyInfoHelper _assemblyInfoHelper = null;

        //public static int TimeoutSession { get { returnSpongeSolutions.ServicoDireto.Admin.Properties.Settings.Default.TimeoutSession; } }

        public static string ActiveUserName
        {
            get
            {
                string userName = "UNKNOW";
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var claimsIdentity = ((System.Security.Claims.ClaimsIdentity)System.Web.HttpContext.Current.User.Identity);
                    var customerClaim = claimsIdentity.Claims.Where(p => p.Type == "sub").FirstOrDefault();
                    if (customerClaim != null)
                        userName = customerClaim.Value;
                }
                return userName;

                //if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.User != null && !string.IsNullOrEmpty(System.Web.HttpContext.Current.User.Identity.Name))
                //    return System.Web.HttpContext.Current.User.Identity.Name;
                //else
                //    return "UNKNOW";
            }
        }

        public static CustomProfile GetActiveProfile
        {
            get { return CustomProfile.GetProfile(); }
        }

        public static CustomProfile GetProfile(string userName)
        {
            return CustomProfile.GetProfile(userName);
        }

        public static int MaximumRows { get { return SpongeSolutions.ServicoDireto.Admin.Properties.Settings.Default.MaximumRows; } }

        public static int MaximumImages { get { return SpongeSolutions.ServicoDireto.Admin.Properties.Settings.Default.MaximumImages; } }

        public static string LayoutPath { get { return SpongeSolutions.ServicoDireto.Admin.Properties.Settings.Default.LayoutPath; } }

        public static string SitePath { get { return SpongeSolutions.ServicoDireto.Admin.Properties.Settings.Default.SitePath; } }

        public static string SiteName { get { return SpongeSolutions.ServicoDireto.Admin.Properties.Settings.Default.SiteName; } }

        public static string UploadPath { get { return SpongeSolutions.ServicoDireto.Admin.Properties.Settings.Default.UploadPath; } }

        public static string DefaultCountry { get { return SpongeSolutions.ServicoDireto.Admin.Properties.Settings.Default.DefaultCountry; } }

        public static string GetFieldMask(int precision = 2)
        {
            return String.Format("decimal-{0}-{1}", System.Threading.Thread.CurrentThread.CurrentUICulture.Name, precision);
        }

        public class Assembly
        {
            public static string Version
            {
                get
                {
                    if (_assemblyInfoHelper == null)
                        _assemblyInfoHelper = new AssemblyInfoHelper(typeof(SiteContext));

                    return _assemblyInfoHelper.AssemblyVersion;
                }
            }
        }
    }
}