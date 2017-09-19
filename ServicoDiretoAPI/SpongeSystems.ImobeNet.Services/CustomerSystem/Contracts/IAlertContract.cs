using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
namespace SpongeSolutions.ServicoDireto.Services.CustomerSystem.Contracts
{
    public interface IAlertContract : IBaseService<Alert, long>
    {
        IList<AlertExtended> List(string idCulture, int idCustomer = 0, long idAlert = 0);
        IList<AlertAttribute> ListAttribute(long idAlert);
        void InsertUpdate(Alert entity, List<AlertAttribute> attributes, Enums.ActionType actionType);
        void Insert(Alert entity, List<AlertAttribute> attributes);
        void Update(Alert entity, List<AlertAttribute> attributes);
        void Delete(long[] ids);
        void Deactivate(long[] ids);
        AlertExtended GetByIdExt(string idCulture, int idAlert);
    }
}
