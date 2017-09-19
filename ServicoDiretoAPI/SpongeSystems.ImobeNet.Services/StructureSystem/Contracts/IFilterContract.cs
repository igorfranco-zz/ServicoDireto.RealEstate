using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;

namespace SpongeSolutions.ServicoDireto.Services.StructureSystem.Contracts
{
    public interface IFilterContract : IBaseService<Model.Filter>
    {
        Model.FilterExtended GetById(string idCulture, int id);
        IList<Model.FilterExtended> GetAll(string idCulture);
        void Inactivate(int[] ids);
        IList<Model.FilterExtended> GetByStatus(string idCulture, short status);
        IList<Model.FilterExtended> GetAllActive(string idCulture);
        void Insert(Model.Filter entity, int[] vinculatedPurpose);
        void Update(Model.Filter entity, int[] vinculatedPurpose);
    }
}