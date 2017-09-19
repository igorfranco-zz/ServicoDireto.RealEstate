using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;

using SpongeSolutions.ServicoDireto.Services;
using SpongeSolutions.ServicoDireto.Services.StructureSystem.Contracts;
using SpongeSolutions.ServicoDireto.Services.CustomerSystem.Contracts;
using SpongeSolutions.ServicoDireto.Services.ElementSystem.Contracts;
using SpongeSolutions.ServicoDireto.Services.PositioningSystem.Contracts;
using SpongeSolutions.ServicoDireto.Services.AdvertisementSystem.Contracts;
using SpongeSolutions.ServicoDireto.Services.AccountSystem.Contracts;

namespace SpongeSolutions.ServicoDireto.Services
{
    public class ServiceContext
    {
        public static ICustomerContract CustomerService
        {
            get { return DependencyResolver.Current.GetService<ICustomerContract>(); }
        }
        public static IPurposeContract PurposeService
        {
            get { return DependencyResolver.Current.GetService<IPurposeContract>(); }
        }
        public static IPurposeCultureContract PurposeCultureService
        {
            get { return DependencyResolver.Current.GetService<IPurposeCultureContract>(); }
        }
        public static IIconContract IconService
        {
            get { return DependencyResolver.Current.GetService<IIconContract>(); }
        }
        public static ICultureContract CultureService
        {
            get { return DependencyResolver.Current.GetService<ICultureContract>(); }
        }
        public static ICountryContract CountryService
        {
            get { return DependencyResolver.Current.GetService<ICountryContract>(); }
        }
        public static IStateProvinceContract StateProvinceService
        {
            get { return DependencyResolver.Current.GetService<IStateProvinceContract>(); }
        }
        public static ICityContract CityService
        {
            get { return DependencyResolver.Current.GetService<ICityContract>(); }
        }
        public static IAttributeContract AttributeService
        {
            get { return DependencyResolver.Current.GetService<IAttributeContract>(); }
        }
        public static IAttributeCultureContract AttributeCultureService
        {
            get { return DependencyResolver.Current.GetService<IAttributeCultureContract>(); }
        }
        public static IAttributeTypeContract AttributeTypeService
        {
            get { return DependencyResolver.Current.GetService<IAttributeTypeContract>(); }
        }
        public static IAttributeTypeCultureContract AttributeTypeCultureService
        {
            get { return DependencyResolver.Current.GetService<IAttributeTypeCultureContract>(); }
        }
        public static IHierarchyStructureContract HierarchyStructureService
        {
            get { return DependencyResolver.Current.GetService<IHierarchyStructureContract>(); }
        }
        public static IHierarchyStructureCultureContract HierarchyStructureCultureService
        {
            get { return DependencyResolver.Current.GetService<IHierarchyStructureCultureContract>(); }
        }
        public static IElementContract ElementService
        {
            get { return DependencyResolver.Current.GetService<IElementContract>(); }
        }
        public static IElementCultureContract ElementCultureService
        {
            get { return DependencyResolver.Current.GetService<IElementCultureContract>(); }
        }
        public static IElementBookmarkedContract ElementBookmarkedService
        {
            get { return DependencyResolver.Current.GetService<IElementBookmarkedContract>(); }
        }
        public static IPositioningContract PositioningService
        {
            get { return DependencyResolver.Current.GetService<IPositioningContract>(); }
        }
        public static IFilterContract FilterService
        {
            get { return DependencyResolver.Current.GetService<IFilterContract>(); }
        }
        public static IEmailContract EmailService
        {
            get { return DependencyResolver.Current.GetService<IEmailContract>(); }
        }
        public static IAdsCategoryContract AdsCategoryService
        {
            get { return DependencyResolver.Current.GetService<IAdsCategoryContract>(); }
        }
        public static IAdsCategoryRelationContract AdsCategoryRelationService
        {
            get { return DependencyResolver.Current.GetService<IAdsCategoryRelationContract>(); }
        }
        public static IAdsCategoryCultureContract AdsCategoryCultureService
        {
            get { return DependencyResolver.Current.GetService<IAdsCategoryCultureContract>(); }
        }
        public static IAlertContract AlertService
        {
            get { return DependencyResolver.Current.GetService<IAlertContract>(); }
        }

        public static IMembershipContract AccountMembershipService
        {
            get { return DependencyResolver.Current.GetService<IMembershipContract>(); }
        }

        public static IFormsAuthenticationContract FormsAuthenticationService
        {
            get { return DependencyResolver.Current.GetService<IFormsAuthenticationContract>(); }
        }

        public static string ActiveUserName
        {
            get
            {
                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.User != null && !string.IsNullOrEmpty(System.Web.HttpContext.Current.User.Identity.Name))
                    return System.Web.HttpContext.Current.User.Identity.Name;
                else
                    return "UNKNOW";
            }
        }
        //public static string ActiveLanguage
        //{
        //    get
        //    {
        //        HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["imobenet"];
        //        string culture = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
        //        if (cookie != null)
        //            if (cookie.Values["language"] != null)
        //                culture = cookie.Values["language"];

        //        if (culture == "en-US")
        //            culture = "en";

        //        return culture;
        //    }
        //    set
        //    {
        //        //if (!System.Threading.Thread.CurrentThread.CurrentUICulture.Name.Equals(value, StringComparison.CurrentCultureIgnoreCase))
        //        //{
        //        var cultureInfo = new System.Globalization.CultureInfo(value);
        //        System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
        //        //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        //        HttpCookie cookie = new HttpCookie("imobenet");
        //        cookie.Values.Add("language", value);
        //        cookie.Expires = DateTime.Now.AddDays(50000);
        //        System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
        //        //}
        //    }
        //}

        public class CacheKey
        {
            public const string ObjectKey = "ObjectKey_{0}";
            public const string ObjectCountKey = "ObjectCountKey_{0}";
        }
    }

}

