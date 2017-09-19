using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using SpongeSolutions.ServicoDireto.Database;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Services.AdvertisementSystem.Contracts;
using SpongeSolutions.ServicoDireto.Model.Advertisement;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using System.Transactions;

namespace SpongeSolutions.ServicoDireto.Services.AdvertisementSystem.Implementation
{
    public class AdsCategoryRelationService : IAdsCategoryRelationContract
    {
        public void Insert(AdsCategoryRelation entity)
        {
            using (var context = new ImobeNetContext())
            {
                var AdsCategoryRelationRepository = new BaseRepository<AdsCategoryRelation>(context);
                AdsCategoryRelationRepository.Insert(entity);
                AdsCategoryRelationRepository.SaveChanges();
            }
        }

        public void Delete(AdsCategoryRelation entity)
        {
            using (var context = new ImobeNetContext())
            {
                var AdsCategoryRelationRepository = new BaseRepository<AdsCategoryRelation>(context);
                AdsCategoryRelationRepository.Delete(entity);
                AdsCategoryRelationRepository.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var context = new ImobeNetContext())
            {
                var AdsCategoryRelationRepository = new BaseRepository<AdsCategoryRelation>(context);
                AdsCategoryRelationRepository.Delete(AdsCategoryRelationRepository.GetById(id));
                AdsCategoryRelationRepository.SaveChanges();
            }
        }

        public void Update(AdsCategoryRelation entity)
        {
            using (var context = new ImobeNetContext())
            {
                var AdsCategoryRelationRepository = new BaseRepository<AdsCategoryRelation>(context);
                AdsCategoryRelationRepository.Update(entity);
                AdsCategoryRelationRepository.SaveChanges();
            }
        }

        public void Delete(int[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var AdsCategoryRelationRepository = new BaseRepository<AdsCategoryRelation>(context);
                var items = AdsCategoryRelationRepository.Fetch().Where(p => ids.Contains(p.IDAdsCategoryRelation));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                        AdsCategoryRelationRepository.Delete(item);

                    AdsCategoryRelationRepository.SaveChanges();
                }
            }
        }

        public IList<AdsCategoryRelation> GetAll()
        {
            using (var context = new ImobeNetContext())
            {
                return context.AdsCategoryRelation.ToList();
            }
        }

        public AdsCategoryRelation GetById(int id)
        {
            return this.GetAll().Where(p => p.IDAdsCategoryRelation == id).FirstOrDefault();
        }
    }
}