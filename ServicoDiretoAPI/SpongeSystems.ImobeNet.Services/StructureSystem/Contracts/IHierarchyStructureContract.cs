using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using System.Web.Mvc;

namespace SpongeSolutions.ServicoDireto.Services.StructureSystem.Contracts
{
    public interface IHierarchyStructureContract : IBaseService<HierarchyStructure>
    {
        void Insert(HierarchyStructure entity, ICollection<HierarchyStructureCulture> hierarchyStructureCulture, int[] vinculatedStruture);
        void Update(HierarchyStructure entity, ICollection<HierarchyStructureCulture> hierarchyStructureCulture, int[] vinculatedStruture);
        void Inactivate(int[] ids);
        HierarchyStructure GetInsert(string categoryName, int? iDPurpose);
        IList<Model.HierarchyStructureExtended> ListCategory(out int recordCount, string idCulture, string sortType = null, int startRowIndex = -1, int maximumRows = -1);
        IList<Model.HierarchyStructureExtended> ListType(out int recordCount, string idCulture, string sortType = null, int startRowIndex = -1, int maximumRows = -1);
        Model.HierarchyStructureExtended GetByIdExtended(int id);
        IList<Model.HierarchyStructureExtended> GetAll(out int recordCount, string sortType = null, int startRowIndex = -1, int maximumRows = -1, int idHierarchyStructure = -1, short status = -1, string idCulture = null);
        IList<HierarchyStructureCulture> ListCulture(int idHierarchyStructure);
        IList<Model.HierarchyStructureExtended> ListVinculated(string idCulture, int idHierarchyStructureParent);
        IList<Model.HierarchyStructureExtended> ListAvailable(string idCulture, int idHierarchyStructureParent);
        IList<Model.HierarchyStructure> ListVinculatedByPurpose(string idCulture, int idPurpose);
        IList<Model.HierarchyStructure> ListAvailableByPurpose(string idCulture, int idPurpose);
        
    }
}
