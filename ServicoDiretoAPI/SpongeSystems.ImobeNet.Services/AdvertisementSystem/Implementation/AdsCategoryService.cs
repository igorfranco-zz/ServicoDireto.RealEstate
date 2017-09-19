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
using System.Data.SqlClient;

namespace SpongeSolutions.ServicoDireto.Services.AdvertisementSystem.Implementation
{
    public class AdsCategoryService : IAdsCategoryContract
    {
        public void InsertUpdate(AdsCategory entity, ICollection<AdsCategoryCulture> adsCategoryCulture, int[] purposeVinculated, int[] categoryVinculated, int[] typeVinculated, Enums.ActionType actionType)
        {
            using (var context = new ImobeNetContext())
            {
                using (TransactionScope scopeOfTransaction = new TransactionScope())
                {
                    try
                    {
                        var adsCategoryRelationRepository = new BaseRepository<AdsCategoryRelation>(context);
                        var adsCategoryRepository = new BaseRepository<AdsCategory>(context);
                        var adsCategoryCultureRepository = new BaseRepository<AdsCategoryCulture>(context);

                        if (actionType == Enums.ActionType.Insert)
                            adsCategoryRepository.Insert(entity);
                        else
                            if (actionType == Enums.ActionType.Update)
                            adsCategoryRepository.Update(entity);

                        context.SaveChanges();

                        //Buscando os valores de culturas
                        var cultureValues = (from acc in context.AdsCategoryCulture
                                             where acc.IDAdsCategory == entity.IDAdsCategory.Value
                                             select acc);

                        //Deletando as culturas
                        if (cultureValues != null && cultureValues.Count() > 0)
                        {
                            foreach (var item in cultureValues)
                                adsCategoryCultureRepository.Delete(item);

                            context.SaveChanges();
                        }

                        // Inserindo os novos valores da cultura
                        foreach (var item in adsCategoryCulture)
                        {
                            item.IDAdsCategory = entity.IDAdsCategory;
                            adsCategoryCultureRepository.Insert(item);
                        }

                        context.SaveChanges();

                        //Excluindo todos os vínculos
                        var vinculatedValues = (from ac in context.AdsCategoryRelation
                                                where ac.IDAdsCategory == entity.IDAdsCategory.Value
                                                select ac);

                        if (vinculatedValues != null && vinculatedValues.Count() > 0)
                        {
                            foreach (var item in vinculatedValues)
                                adsCategoryRelationRepository.Delete(item);

                            context.SaveChanges();
                        }

                        //Inserindo vínculos com propositos
                        if (purposeVinculated != null && purposeVinculated.Count() > 0)
                        {
                            foreach (var item in purposeVinculated)
                                adsCategoryRelationRepository.Insert(new AdsCategoryRelation() { IDAdsCategory = entity.IDAdsCategory.Value, IDPurpose = item });

                            context.SaveChanges();
                        }

                        //Inserindo vínculos com categorias
                        if (categoryVinculated != null && categoryVinculated.Count() > 0)
                        {
                            foreach (var item in categoryVinculated)
                                adsCategoryRelationRepository.Insert(new AdsCategoryRelation() { IDAdsCategory = entity.IDAdsCategory.Value, IDHierarchyStructure = item });

                            context.SaveChanges();
                        }

                        //Inserindo vínculos com tipo
                        if (typeVinculated != null && typeVinculated.Count() > 0)
                        {
                            foreach (var item in typeVinculated)
                                adsCategoryRelationRepository.Insert(new AdsCategoryRelation() { IDAdsCategory = entity.IDAdsCategory.Value, IDHierarchyStructure = item });

                            context.SaveChanges();
                        }

                        if (entity.IDCustomer.HasValue)
                        {
                            adsCategoryRelationRepository.Insert(new AdsCategoryRelation() { IDAdsCategory = entity.IDAdsCategory.Value, IDCustomer = entity.IDCustomer.Value });
                            context.SaveChanges();
                        }


                        scopeOfTransaction.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        scopeOfTransaction.Dispose();
                    }
                }
            }
        }

        public void Insert(AdsCategory entity, ICollection<AdsCategoryCulture> adsCategoryCulture, int[] purposeVinculated, int[] categoryVinculated, int[] typeVinculated)
        {
            this.InsertUpdate(entity, adsCategoryCulture, purposeVinculated, categoryVinculated, typeVinculated, Enums.ActionType.Insert);
        }

        public void Update(AdsCategory entity, ICollection<AdsCategoryCulture> adsCategoryCulture, int[] purposeVinculated, int[] categoryVinculated, int[] typeVinculated)
        {
            this.InsertUpdate(entity, adsCategoryCulture, purposeVinculated, categoryVinculated, typeVinculated, Enums.ActionType.Update);
        }

        public void Insert(AdsCategory entity)
        {
            using (var context = new ImobeNetContext())
            {
                var adsCategoryRepository = new BaseRepository<AdsCategory>(context);
                adsCategoryRepository.Insert(entity);
                adsCategoryRepository.SaveChanges();
            }
        }

        public void Delete(AdsCategory entity)
        {
            using (var context = new ImobeNetContext())
            {
                var adsCategoryRepository = new BaseRepository<AdsCategory>(context);
                adsCategoryRepository.Delete(entity);
                adsCategoryRepository.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var context = new ImobeNetContext())
            {
                var adsCategoryRepository = new BaseRepository<AdsCategory>(context);
                adsCategoryRepository.Delete(adsCategoryRepository.GetById(id));
                adsCategoryRepository.SaveChanges();
            }
        }

        public void Update(AdsCategory entity)
        {
            using (var context = new ImobeNetContext())
            {
                var adsCategoryRepository = new BaseRepository<AdsCategory>(context);
                adsCategoryRepository.Update(entity);
                adsCategoryRepository.SaveChanges();
            }
        }

        public void Delete(int[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var adsCategoryRepository = new BaseRepository<AdsCategory>(context);
                var items = adsCategoryRepository.Fetch().Where(p => ids.Contains(p.IDAdsCategory.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                        adsCategoryRepository.Delete(item);

                    adsCategoryRepository.SaveChanges();
                }
            }
        }

        public void Inactivate(int[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var adsCategoryRepository = new BaseRepository<AdsCategory>(context);
                var items = adsCategoryRepository.Fetch().Where(p => ids.Contains(p.IDAdsCategory.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                    {
                        item.Status = (short)Enums.StatusType.Inactive;
                        item.ModifiedBy = ServiceContext.ActiveUserName;
                        adsCategoryRepository.Update(item);
                    }

                    adsCategoryRepository.SaveChanges();
                }
            }
        }

        public IList<AdsCategoryExtended> GetAllExtended(string idCulture)
        {
            using (var context = new ImobeNetContext())
            {
                var result = (from ac in context.AdsCategory.ToList()
                              join acc in context.AdsCategoryCulture on new { IDAdsCategory = ac.IDAdsCategory, IDCulture = idCulture } equals new { IDAdsCategory = acc.IDAdsCategory, IDCulture = acc.IDCulture }
                              orderby acc.Name
                              select new AdsCategoryExtended
                              {
                                  IDAdsCategory = ac.IDAdsCategory,
                                  Name = acc.Name,
                                  CreateDate = ac.CreateDate,
                                  CreatedBy = ac.CreatedBy,
                                  ModifiedBy = ac.ModifiedBy,
                                  ModifyDate = ac.ModifyDate,
                                  Status = ac.Status,
                              });
                return result.ToList();
            }

        }

        public IList<AdsCategory> GetAll()
        {
            using (var context = new ImobeNetContext())
            {
                var result = (from ac in context.AdsCategory.ToList()
                              join acc in context.AdsCategoryCulture on new { IDAdsCategory = ac.IDAdsCategory } equals new { IDAdsCategory = acc.IDAdsCategory }
                              orderby acc.Name
                              select new AdsCategory
                              {
                                  IDAdsCategory = ac.IDAdsCategory,
                                  CreateDate = ac.CreateDate,
                                  CreatedBy = ac.CreatedBy,
                                  ModifiedBy = ac.ModifiedBy,
                                  ModifyDate = ac.ModifyDate,
                                  Status = ac.Status,
                              });
                return result.ToList();
            }
        }

        public AdsCategory GetById(int id)
        {
            return this.GetAll().Where(p => p.IDAdsCategory == id).FirstOrDefault();
        }

        public IList<AdsCategory> GetByStatus(short status)
        {
            return this.GetAll().Where(p => p.Status == status).ToList();
        }

        public IList<AdsCategory> GetAllActive()
        {
            return this.GetByStatus((short)Enums.StatusType.Active).ToList();
        }

        //public IList<PurposeBasic> ListAvailablePurpose(int idAdsCategory)
        //{
        //    using (var context = new ImobeNetContext())
        //    {
        //        StringBuilder sql = new StringBuilder();
        //        object[] parameters = new object[] { new SqlParameter("IDAdsCategory", idAdsCategory),
        //                                             new SqlParameter("Status", (short)Enums.StatusType.Active),
        //                                             new SqlParameter("IDCulture", ServiceContext.ActiveLanguage)};

        //        sql.AppendLine("select p.*, pc.[description] from purpose p");
        //        sql.AppendLine("	inner join PurposeCulture pc on (p.IDPurpose  = pc.IDPurpose and pc.IDCulture = @IDCulture)");
        //        sql.AppendLine("where p.idpurpose not in (select ISNULL(idpurpose,0) from dbo.AdsCategoryRelation acr where IDAdsCategory = @IDAdsCategory)");
        //        sql.AppendLine("	and p.status = @Status");
        //        sql.AppendLine("order by pc.Description");
        //        return context.Database.SqlQuery<Model.PurposeBasic>(sql.ToString(), parameters).ToList();
        //    }
        //}

        //public IList<PurposeBasic> ListVinculatedPurpose(int idAdsCategory)
        //{
        //    using (var context = new ImobeNetContext())
        //    {
        //        StringBuilder sql = new StringBuilder();
        //        object[] parameters = new object[] { new SqlParameter("IDAdsCategory", idAdsCategory),
        //                                             new SqlParameter("Status", (short)Enums.StatusType.Active),
        //                                             new SqlParameter("IDCulture", ServiceContext.ActiveLanguage)};

        //        sql.AppendLine("select p.*, pc.[description] from dbo.AdsCategoryRelation acr ");
        //        sql.AppendLine("	inner join Purpose p on (acr.IDPurpose  = p.IDPurpose)");
        //        sql.AppendLine("	inner join PurposeCulture pc on (p.IDPurpose  = pc.IDPurpose and pc.IDCulture = @IDCulture)");
        //        sql.AppendLine("where p.status = @Status and IDAdsCategory = @IDAdsCategory");
        //        sql.AppendLine("order by pc.Description");
        //        return context.Database.SqlQuery<Model.PurposeBasic>(sql.ToString(), parameters).ToList();
        //    }
        //}

        public IList<HierarchyStructureBasic> ListAvailableCategory(string idCulture, int idAdsCategory)
        {
            using (var context = new ImobeNetContext())
            {
                StringBuilder sql = new StringBuilder();
                object[] parameters = new object[] { new SqlParameter("IDAdsCategory", idAdsCategory),
                                                     new SqlParameter("Status", (short)Enums.StatusType.Active),
                                                     new SqlParameter("IDCulture", idCulture)};

                sql.AppendLine(" select hs.IDHierarchyStructure, hsc.[Description] from HierarchyStructure hs ");
                sql.AppendLine("	inner join HierarchyStructureCulture hsc on (hs.IDHierarchyStructure = hsc.IDHierarchyStructure and hsc.IDCulture = @IDCulture)	 ");
                sql.AppendLine(" where IDHierarchyStructureParent is null and hs.IDHierarchyStructure not in (select ISNULL(IDHierarchyStructure,0) from AdsCategoryRelation where IDAdsCategory = @IDAdsCategory) ");
                sql.AppendLine(" and hs.status = @Status");
                return context.Database.SqlQuery<Model.HierarchyStructureBasic>(sql.ToString(), parameters).ToList();
            }
        }

        public IList<HierarchyStructureBasic> ListVinculatedCategory(string idCulture, int idAdsCategory)
        {
            using (var context = new ImobeNetContext())
            {
                StringBuilder sql = new StringBuilder();
                object[] parameters = new object[] { new SqlParameter("IDAdsCategory", idAdsCategory),
                                                     new SqlParameter("Status", (short)Enums.StatusType.Active),
                                                     new SqlParameter("IDCulture", idCulture )};

                sql.AppendLine("select hs.IDHierarchyStructure, hsc.[Description] from dbo.AdsCategoryRelation acr ");
                sql.AppendLine("	inner join HierarchyStructure hs on (acr.IDHierarchyStructure = hs.IDHierarchyStructure) ");
                sql.AppendLine("	inner join HierarchyStructureCulture hsc on (acr.IDHierarchyStructure = hsc.IDHierarchyStructure and hsc.IDCulture = @IDCulture)	 ");
                sql.AppendLine("where IDHierarchyStructureParent is null and IDAdsCategory = @IDAdsCategory");
                sql.AppendLine("and hs.status = @Status ");
                return context.Database.SqlQuery<Model.HierarchyStructureBasic>(sql.ToString(), parameters).ToList();
            }
        }

        public IList<HierarchyStructureBasic> ListAvailableType(string idCulture, int idAdsCategory)
        {
            using (var context = new ImobeNetContext())
            {
                StringBuilder sql = new StringBuilder();
                object[] parameters = new object[] { new SqlParameter("IDAdsCategory", idAdsCategory),
                                                     new SqlParameter("Status", (short)Enums.StatusType.Active),
                                                     new SqlParameter("IDCulture", idCulture)};

                sql.AppendLine(" select hs.IDHierarchyStructure, hsc.[Description] from HierarchyStructure hs ");
                sql.AppendLine("	inner join HierarchyStructureCulture hsc on (hs.IDHierarchyStructure = hsc.IDHierarchyStructure and hsc.IDCulture = @IDCulture)	 ");
                sql.AppendLine(" where IDHierarchyStructureParent is not null and hs.IDHierarchyStructure not in (select ISNULL(IDHierarchyStructure,0) from dbo.AdsCategoryRelation acr where acr.IDAdsCategory = @IDAdsCategory) ");
                sql.AppendLine(" and hs.status = @Status");
                sql.AppendLine(" order by hsc.description");
                return context.Database.SqlQuery<Model.HierarchyStructureBasic>(sql.ToString(), parameters).ToList();
            }
        }

        public IList<HierarchyStructureBasic> ListVinculatedType(string idCulture, int idAdsCategory)
        {
            using (var context = new ImobeNetContext())
            {
                StringBuilder sql = new StringBuilder();
                object[] parameters = new object[] { new SqlParameter("IDAdsCategory", idAdsCategory),
                                                     new SqlParameter("Status", (short)Enums.StatusType.Active),
                                                     new SqlParameter("IDCulture", idCulture)};

                sql.AppendLine(" select hs.IDHierarchyStructure, hsc.[Description] from dbo.AdsCategoryRelation acr ");
                sql.AppendLine("	inner join HierarchyStructure hs on (acr.IDHierarchyStructure = hs.IDHierarchyStructure) ");
                sql.AppendLine("	inner join HierarchyStructureCulture hsc on (acr.IDHierarchyStructure = hsc.IDHierarchyStructure and hsc.IDCulture = @IDCulture)	");
                sql.AppendLine(" where IDHierarchyStructureParent is not null and IDAdsCategory = @IDAdsCategory");
                sql.AppendLine(" and hs.status = @Status ");
                sql.AppendLine(" order by hsc.description");
                return context.Database.SqlQuery<Model.HierarchyStructureBasic>(sql.ToString(), parameters).ToList();
            }
        }

        public IList<AdsCategoryRelation> ListRelation(int idAdsCategory)
        {
            using (var context = new ImobeNetContext())
            {
                var result = (from acr in context.AdsCategoryRelation
                              where acr.IDAdsCategory == idAdsCategory
                              select acr).ToList();
                return result.ToList();
            }
        }

    }
}