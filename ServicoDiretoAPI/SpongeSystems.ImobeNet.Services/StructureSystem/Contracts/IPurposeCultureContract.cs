using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using SpongeSolutions.ServicoDireto.Services.InfraStructure;

namespace SpongeSolutions.ServicoDireto.Services.StructureSystem.Contracts
{
    [System.ServiceModel.ServiceContract]
    public interface IPurposeCultureContract : IBaseService<PurposeCulture>
    {
        void DeleteByPurpose(int idPurpose);
        PurposeCulture GetById(int idPurpose, string idCulture);
    }
}
