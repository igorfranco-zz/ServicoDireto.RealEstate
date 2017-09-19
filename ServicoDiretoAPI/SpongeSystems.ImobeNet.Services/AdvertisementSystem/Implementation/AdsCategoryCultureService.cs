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
    public class AdsCategoryCultureService : IAdsCategoryCultureContract
    {
        public void Insert(AdsCategoryCulture entity)
        {
            using (var context = new ImobeNetContext())
            {
                var adsCategoryCultureRepository = new BaseRepository<AdsCategoryCulture>(context);
                adsCategoryCultureRepository.Insert(entity);
                adsCategoryCultureRepository.SaveChanges();
            }
        }

        public void Delete(AdsCategoryCulture entity)
        {
            using (var context = new ImobeNetContext())
            {
                var adsCategoryCultureRepository = new BaseRepository<AdsCategoryCulture>(context);
                adsCategoryCultureRepository.Delete(entity);
                adsCategoryCultureRepository.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var context = new ImobeNetContext())
            {
                var adsCategoryCultureRepository = new BaseRepository<AdsCategoryCulture>(context);
                adsCategoryCultureRepository.Delete(adsCategoryCultureRepository.GetById(id));
                adsCategoryCultureRepository.SaveChanges();
            }
        }

        public void Update(AdsCategoryCulture entity)
        {
            using (var context = new ImobeNetContext())
            {
                var adsCategoryCultureRepository = new BaseRepository<AdsCategoryCulture>(context);
                adsCategoryCultureRepository.Update(entity);
                adsCategoryCultureRepository.SaveChanges();
            }
        }

        public void Delete(int[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var adsCategoryCultureRepository = new BaseRepository<AdsCategoryCulture>(context);
                var items = adsCategoryCultureRepository.Fetch().Where(p => ids.Contains(p.IDAdsCategory.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                        adsCategoryCultureRepository.Delete(item);

                    adsCategoryCultureRepository.SaveChanges();
                }
            }
        }

        public IList<AdsCategoryCulture> GetAll()
        {
            using (var context = new ImobeNetContext())
            {
                return (from acc in context.AdsCategoryCulture.ToList()
                        join c in context.Culture on acc.IDCulture equals c.IDCulture
                        join i in context.Icon on c.IDIcon equals i.IDIcon
                        select new AdsCategoryCulture
                        {
                            IDAdsCategory = acc.IDAdsCategory,
                            IDCulture = acc.IDCulture,
                            Name = acc.Name,
                            CultureName = c.Name,
                            IconPath = i.Path
                        }).ToList();
            }
        }

        public AdsCategoryCulture GetById(int id)
        {
            return this.GetAll().Where(p => p.IDAdsCategory == id).FirstOrDefault();
        }

        public AdsCategoryCulture GetById(int idAdsCategory, string idCulture)
        {
            return this.GetAll().Where(p => p.IDAdsCategory == idAdsCategory && p.IDCulture == idCulture).FirstOrDefault();
        }
    }
}