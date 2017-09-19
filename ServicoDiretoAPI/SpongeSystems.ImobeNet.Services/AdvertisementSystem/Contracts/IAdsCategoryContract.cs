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
    public interface IAdsCategoryContract : IBaseService<AdsCategory>
    {
        IList<AdsCategoryExtended> GetAllExtended(string idCulture);
        void InsertUpdate(AdsCategory entity, ICollection<AdsCategoryCulture> adsCategoryCulture, int[] purposeVinculated, int[] categoryVinculated, int[] typeVinculated, Enums.ActionType actionType);
        void Update(AdsCategory entity, ICollection<AdsCategoryCulture> adsCategoryCulture, int[] purposeVinculated, int[] categoryVinculated, int[] typeVinculated);
        void Insert(AdsCategory entity, ICollection<AdsCategoryCulture> adsCategoryCulture, int[] purposeVinculated, int[] categoryVinculated, int[] typeVinculated);
        
        IList<HierarchyStructureBasic> ListVinculatedCategory(string idCulture, int idAdsCategory);
        IList<HierarchyStructureBasic> ListAvailableCategory(string idCulture, int idAdsCategory);
        IList<HierarchyStructureBasic> ListVinculatedType(string idCulture, int idAdsCategory);
        IList<HierarchyStructureBasic> ListAvailableType(string idCulture, int idAdsCategory);
        IList<AdsCategoryRelation> ListRelation( int idAdsCategory);
        void Inactivate(int[] ids);
    }
}
