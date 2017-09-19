using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Model.Advertisement;
using SpongeSolutions.ServicoDireto.Database;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using System.Transactions;
namespace SpongeSolutions.ServicoDireto.Services.AdvertisementSystem.Contracts
{
    public interface IAdsCategoryCultureContract : IBaseService<AdsCategoryCulture>
    {
        AdsCategoryCulture GetById(int idAdsCategory, string idCulture);
    }
}
