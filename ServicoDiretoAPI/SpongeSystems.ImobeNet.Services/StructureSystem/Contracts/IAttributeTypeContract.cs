using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Services.InfraStructure;

namespace SpongeSolutions.ServicoDireto.Services.StructureSystem.Contracts
{
    public interface IAttributeTypeContract : IBaseService<AttributeType>
    {
        void Insert(AttributeType entity, ICollection<AttributeTypeCulture> AttributeTypeCulture, int[] attribute);
        void Update(AttributeType entity, ICollection<AttributeTypeCulture> AttributeTypeCulture, int[] attribute);
        void Inactivate(int[] ids);
        IList<Model.AttributeTypeExtended> GetAll(string idCulture);
        IList<Model.AttributeType> GetByStatus(short status);
        IList<Model.AttributeType> GetAllActive();        
        Model.AttributeType GetByAcronym(string acronym);
        IList<AttributeTypeCultureExtended> ListAttributeTypeCulture(int? idAttributeType);
    }
}
