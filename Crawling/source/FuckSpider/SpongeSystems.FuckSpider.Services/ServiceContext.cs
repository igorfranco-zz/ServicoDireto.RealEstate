using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SpongeSystems.Spider.Services.Contracts;

namespace SpongeSystems.Spider.Services
{
    public class ServiceContext
    {
        public static IAgencyContract AgencyService
        {
            get { return DependencyResolver.Current.GetService<IAgencyContract>(); }
        }
    }
}
