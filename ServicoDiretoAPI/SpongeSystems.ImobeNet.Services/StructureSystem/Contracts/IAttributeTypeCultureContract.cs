using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Services.InfraStructure;

namespace SpongeSolutions.ServicoDireto.Services.StructureSystem.Contracts
{
    public interface IAttributeTypeCultureContract : IBaseService<AttributeTypeCulture>
    {
        void DeleteByAttributeType(int idAttributeType);
        AttributeTypeCulture GetById(int idAttributeType, string idCulture);
    }
}
