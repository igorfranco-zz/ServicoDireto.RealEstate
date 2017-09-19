using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Services.InfraStructure;

namespace SpongeSolutions.ServicoDireto.Services.StructureSystem.Contracts
{
    public interface IAttributeCultureContract : IBaseService<AttributeCulture>
    {
        void DeleteByAttribute(int idAttribute);
        AttributeCulture GetById(int idAttribute, string idCulture);
    }
}
