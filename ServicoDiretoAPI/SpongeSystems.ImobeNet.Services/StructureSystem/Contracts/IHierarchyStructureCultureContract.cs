using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Services.InfraStructure;

namespace SpongeSolutions.ServicoDireto.Services.StructureSystem.Contracts
{
    public interface IHierarchyStructureCultureContract : IBaseService<HierarchyStructureCulture>
    {
        void DeleteByHierarchyStructure(int idHierarchyStructure);
        HierarchyStructureCulture GetById(int idHierarchyStructure, string idCulture);
    }
}
