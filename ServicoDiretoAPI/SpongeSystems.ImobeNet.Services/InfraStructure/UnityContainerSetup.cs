using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto;
using System.Web.Security;
using SpongeSolutions.ServicoDireto.Services.StructureSystem.Contracts;
using SpongeSolutions.ServicoDireto.Services.StructureSystem.Implementation;
using SpongeSolutions.ServicoDireto.Database;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Services.CustomerSystem.Contracts;
using SpongeSolutions.ServicoDireto.Services.CustomerSystem.Implementation;
using SpongeSolutions.ServicoDireto.Services.ElementSystem.Contracts;
using SpongeSolutions.ServicoDireto.Services.ElementSystem.Implementation;
using SpongeSolutions.ServicoDireto.Services.PositioningSystem.Contracts;
using SpongeSolutions.ServicoDireto.Services.PositioningSystem.Implementation;
using SpongeSolutions.ServicoDireto.Services.AdvertisementSystem.Implementation;
using SpongeSolutions.ServicoDireto.Services.AdvertisementSystem.Contracts;
using Microsoft.Practices.Unity;
using SpongeSolutions.ServicoDireto.Services.AccountSystem.Contracts;
using SpongeSolutions.ServicoDireto.Services.AccountSystem.Implementation;

namespace SpongeSolutions.ServicoDireto.Services.InfraStructure
{
    //http://www.lostechies.com/blogs/johnvpetersen/archive/2011/01/23/dependency-injection-with-asp-mvc-3-distilled-and-simplified.aspx
    public class UnityContainerSetup
    {
        public static void SetUp()
        {
            //Using Lifetime Managers
            //https://msdn.microsoft.com/en-us/library/ff647854.aspx

            var container = new UnityContainer();
            container.RegisterType<IPurposeContract, PurposeService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IPurposeCultureContract, PurposeCultureService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IIconContract, IconService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICultureContract, CultureService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICountryContract, CountryService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IStateProvinceContract, StateProvinceService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICityContract, CityService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IAttributeContract, AttributeService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IAttributeCultureContract, AttributeCultureService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IAttributeTypeContract, AttributeTypeService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IAttributeTypeCultureContract, AttributeTypeCultureService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IHierarchyStructureContract, HierarchyStructureService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IHierarchyStructureCultureContract, HierarchyStructureCultureService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICustomerContract, CustomerService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IElementContract, ElementService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IElementCultureContract, ElementCultureService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IPositioningContract, PositioningService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IFilterContract, FilterService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IEmailContract, EmailService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IAdsCategoryContract, AdsCategoryService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IAdsCategoryCultureContract, AdsCategoryCultureService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IAdsCategoryRelationContract, AdsCategoryRelationService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IAlertContract, AlertService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IElementBookmarkedContract, ElementBookmarkedService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMembershipContract, AccountMembershipService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IFormsAuthenticationContract, FormsAuthenticationService>(new ContainerControlledLifetimeManager());
            DependencyResolver.SetResolver(new Unity.Mvc4.UnityDependencyResolver(container));
        }
    }
}

