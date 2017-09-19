using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using SpongeSolutions.ServicoDireto.Services.StructureSystem.Contracts;
using SpongeSolutions.ServicoDireto.Database;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using System.Web.Mvc;
using System.Data.SqlClient;


namespace SpongeSolutions.ServicoDireto.Services.StructureSystem.Implementation
{
    public class PurposeService : IPurposeContract
    {
        public void InsertUpdate(Purpose entity, ICollection<PurposeCulture> purposeCulture, int[] vinculatedStructure, Enums.ActionType actionType)
        {
            using (var context = new ImobeNetContext())
            {
                using (TransactionScope scopeOfTransaction = new TransactionScope())
                {
                    try
                    {
                        var purposeRepository = new BaseRepository<Purpose>(context);
                        var purposeCultureRepository = new BaseRepository<PurposeCulture>(context);
                        var hierarchyStructurePurposeRepository = new BaseRepository<HierarchyStructurePurpose>(context);

                        if (actionType == Enums.ActionType.Insert)
                            purposeRepository.Insert(entity);
                        else
                            if (actionType == Enums.ActionType.Update)
                            purposeRepository.Update(entity);

                        context.SaveChanges();

                        var purposes = (from p in context.PurposeCulture
                                        where p.IDPurpose == entity.IDPurpose
                                        select p);

                        //Deletando os idiomas
                        if (purposes != null && purposes.Count() > 0)
                        {
                            foreach (var purpose in purposes)
                                purposeCultureRepository.Delete(purpose);

                            context.SaveChanges();
                        }

                        // inserindo novos
                        foreach (var purpose in purposeCulture)
                        {
                            purpose.IDPurpose = entity.IDPurpose;
                            purposeCultureRepository.Insert(purpose);
                        }


                        #region [Children Structure]

                        var childStructures = (from p in context.HierarchyStructurePurpose
                                               where p.IDPurpose == entity.IDPurpose
                                               select p);

                        //Desvinculando os filhos
                        if (childStructures != null && childStructures.Count() > 0)
                        {
                            foreach (var childStructure in childStructures)
                                hierarchyStructurePurposeRepository.Delete(childStructure);

                            context.SaveChanges();
                        }

                        //Vinculando novos filhos
                        if (vinculatedStructure != null && vinculatedStructure.Count() > 0)
                        {
                            foreach (var item in vinculatedStructure)
                            {
                                hierarchyStructurePurposeRepository.Insert(new HierarchyStructurePurpose() { IDPurpose = entity.IDPurpose.Value, IDHierarchyStructure = item });
                                context.SaveChanges();
                            }
                        }

                        #endregion

                        context.SaveChanges();

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

        public void Insert(Purpose entity, ICollection<PurposeCulture> purposeCulture, int[] vinculatedStructure)
        {
            this.InsertUpdate(entity, purposeCulture, vinculatedStructure, Enums.ActionType.Insert);
        }

        public void Insert(Purpose entity)
        {
            using (var context = new ImobeNetContext())
            {
                var purposeRepository = new BaseRepository<Purpose>(context);
                purposeRepository.Insert(entity);
                purposeRepository.SaveChanges();
            }
        }

        public void Delete(Purpose entity)
        {
            using (var context = new ImobeNetContext())
            {
                var purposeRepository = new BaseRepository<Purpose>(context);
                purposeRepository.Delete(entity);
                purposeRepository.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var context = new ImobeNetContext())
            {
                var purposeRepository = new BaseRepository<Purpose>(context);
                purposeRepository.Delete(purposeRepository.GetById(id));
                purposeRepository.SaveChanges();
            }
        }

        public void Update(Purpose entity)
        {
            using (var context = new ImobeNetContext())
            {
                var purposeRepository = new BaseRepository<Purpose>(context);
                purposeRepository.Update(entity);
                purposeRepository.SaveChanges();
            }
        }

        public void Update(Purpose entity, ICollection<PurposeCulture> purposeCulture, int[] vinculatedStructure)
        {
            this.InsertUpdate(entity, purposeCulture, vinculatedStructure, Enums.ActionType.Update);
        }

        public void Delete(int[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var purposeRepository = new BaseRepository<Purpose>(context);
                var items = purposeRepository.Fetch().Where(p => ids.Contains(p.IDPurpose.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                        purposeRepository.Delete(item);

                    purposeRepository.SaveChanges();
                }
            }
        }

        public void Inactivate(int[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var purposeRepository = new BaseRepository<Purpose>(context);
                var items = purposeRepository.Fetch().Where(p => ids.Contains(p.IDPurpose.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                    {
                        item.Status = (short)Enums.StatusType.Inactive;
                        item.ModifiedBy = ServiceContext.ActiveUserName;
                        purposeRepository.Update(item);
                    }

                    purposeRepository.SaveChanges();
                }
            }
        }

        public IList<Purpose> GetAll()
        {
            using (var context = new ImobeNetContext())
            {
                var purposeRepository = new BaseRepository<Purpose>(context);
                return purposeRepository.GetAll().ToList();
            }
        }

        public IList<PurposeCultureExtended> ListPurposeCulture(int? idPurpose)
        {
            List<PurposeCultureExtended> result = new List<PurposeCultureExtended>();
            foreach (var item in ServiceContext.CultureService.GetAllActive())
            {
                var purpose = new PurposeCultureExtended() { IDCulture = item.IDCulture, IconPath = item.IconPath, CultureName = item.Name };
                if (idPurpose.HasValue)
                {
                    var purposeCulture = ServiceContext.PurposeCultureService.GetById(idPurpose.Value, item.IDCulture);
                    if (purposeCulture != null)
                        purpose.Description = purposeCulture.Description;
                }
                result.Add(purpose);
            }

            return result;
        }

        public IList<PurposeExtended> ListAll(string idCulture)
        {
            StringBuilder sql = new StringBuilder();
            List<object> parameters = new List<object>();
            parameters.Add(new SqlParameter("IDCulture", idCulture));
            using (var context = new ImobeNetContext())
            {
                sql.AppendLine("exec [pPurpose_List] @IDCulture");
                return context.Database.SqlQuery<PurposeExtended>(sql.ToString(), parameters.ToArray()).ToList();
            }
        }

        public Purpose GetById(int id)
        {
            return this.GetAll().Where(p => p.IDPurpose == id).FirstOrDefault();
        }

        public IList<PurposeExtended> GetByStatus(short status, string idCulture)
        {
            return this.ListAll(idCulture).Where(p => p.Status == status).ToList();
        }

        public IList<PurposeExtended> GetAllActive(string idCulture)
        {
            return this.GetByStatus((short)Enums.StatusType.Active, idCulture).ToList();
        }

        public IList<Model.HierarchyStructureBasic> ListVinculatedCategory(int[] idPurpose, string idCulture)
        {
            using (var context = new ImobeNetContext())
            {
                StringBuilder sql = new StringBuilder();
                string purposes = "-1";
                if (idPurpose != null && idPurpose.Length > 0)
                    purposes = String.Join(",", idPurpose);

                object[] parameters = new object[] { new SqlParameter("IDPurpose", purposes),
                                                     new SqlParameter("Status", (short)Enums.StatusType.Active),
                                                     new SqlParameter("IDCulture", idCulture)};

                sql.AppendLine("select distinct	");
                sql.AppendLine(" hs.*, ");
                sql.AppendLine(" hsc.Description,");
                sql.AppendLine(" i.*");
                sql.AppendLine(" from HierarchyStructure hs");
                sql.AppendLine(" left join Icon i on (hs.IDIcon = i.IDIcon)");
                sql.AppendLine(" inner join HierarchyStructureCulture hsc on (hs.IDHierarchyStructure = hsc.IDHierarchyStructure )");
                sql.AppendLine(" inner join HierarchyStructurePurpose hsp on (hs.IDHierarchyStructure = hsp.IDHierarchyStructure )");
                sql.AppendLine(" where hsc.IDCulture = @IDCulture");
                sql.AppendLine("    and hs.Status = @Status");
                sql.AppendLine("    and hsp.IDPurpose in (select * from [dbo].[IntegerCommaSplit](@IDPurpose)) ");
                sql.AppendLine(" order by hsc.Description");

                return context.Database.SqlQuery<Model.HierarchyStructureBasic>(sql.ToString(), parameters).ToList();
            }
        }

        public IList<Model.HierarchyStructureBasic> ListAvailableCategory(int[] idPurpose, string idCulture)
        {
            using (var context = new ImobeNetContext())
            {
                StringBuilder sql = new StringBuilder();
                string purposes = "-1";
                if (idPurpose != null && idPurpose.Length > 0)
                    purposes = String.Join(",", idPurpose);

                object[] parameters = new object[] { new SqlParameter("IDPurpose", purposes),
                                                     new SqlParameter("Status", (short)Enums.StatusType.Active),
                                                     new SqlParameter("IDCulture", idCulture)};

                sql.AppendLine("select distinct");
                sql.AppendLine(" hs.*, ");
                sql.AppendLine(" hsc.Description,");
                sql.AppendLine(" i.*");
                sql.AppendLine(" from HierarchyStructure hs");
                sql.AppendLine(" left join Icon i on (hs.IDIcon = i.IDIcon)");
                sql.AppendLine(" inner join HierarchyStructureCulture hsc on (hs.IDHierarchyStructure = hsc.IDHierarchyStructure )");
                sql.AppendLine(" where hsc.IDCulture = @IDCulture");
                sql.AppendLine("    and hs.Status = @Status");
                sql.AppendLine("    and hs.IDHierarchyStructureParent is null");
                sql.AppendLine("    and hs.IDHierarchyStructure not in (select IDHierarchyStructure from HierarchyStructurePurpose where IDPurpose in ( (select * from [dbo].[IntegerCommaSplit](@IDPurpose)) ) )");
                sql.AppendLine(" order by hsc.Description");

                return context.Database.SqlQuery<Model.HierarchyStructureBasic>(sql.ToString(), parameters).ToList();
            }
        }

        public IList<HierarchyStructureExtended> ListCategory(string idCulture, int[] idPurpose )
        {
            StringBuilder sql = new StringBuilder();
            string purposes = "-1";
            if (idPurpose != null && idPurpose.Length > 0)
                purposes = String.Join(",", idPurpose);

            object[] parameters = new object[] { new SqlParameter("IDPurpose", purposes),
                                                 new SqlParameter("Status", (short)Enums.StatusType.Active),
                                                 new SqlParameter("IDCulture", idCulture)};
            using (var context = new ImobeNetContext())
            {
                sql.AppendLine("exec pPurposeCategory_List @IDPurpose, @Status, @IDCulture");
                return context.Database.SqlQuery<Model.HierarchyStructureExtended>(sql.ToString(), parameters).ToList();
            }
        }

        public IList<HierarchyStructureBasic> ListType(int idHierarhyStructureParent, string idCulture)
        {
            using (var context = new ImobeNetContext())
            {
                StringBuilder sql = new StringBuilder();
                object[] parameters = new object[] { new SqlParameter("IDHierarhyStructureParent", idHierarhyStructureParent),
                                                     new SqlParameter("Status", (short)Enums.StatusType.Active),
                                                     new SqlParameter("IDCulture", idCulture)};

                sql.AppendLine("select 	");
                sql.AppendLine(" hs.*, ");
                sql.AppendLine(" hsc.Description,");
                sql.AppendLine(" i.*");
                sql.AppendLine(" from HierarchyStructure hs");
                sql.AppendLine(" left join Icon i on (hs.IDIcon = i.IDIcon)");
                sql.AppendLine(" inner join HierarchyStructureCulture hsc on (hs.IDHierarchyStructure = hsc.IDHierarchyStructure )");
                sql.AppendLine(" where hsc.IDCulture = @IDCulture");
                sql.AppendLine("    and hs.Status = @Status");
                sql.AppendLine("    and hs.IDHierarchyStructureParent = @IDHierarhyStructureParent");
                sql.AppendLine(" order by hsc.Description");

                return context.Database.SqlQuery<Model.HierarchyStructureBasic>(sql.ToString(), parameters).ToList();
            }
        }

        //public SelectList ListCategoryAsSelectList(int[] idPurpose)
        //{
        //    return new SelectList(this.ListCategory(idPurpose), "IDHierarchyStructure", "Description");
        //}

        //public SelectList ListTypeAsSelectList(int idHierarhyStructureParent)
        //{
        //    return new SelectList(this.ListType(idHierarhyStructureParent), "IDHierarchyStructure", "Description");
        //}

        public IList<Model.PurposeExtended> ListAvailablePurpose(string idCulture, int idFilter)
        {
            StringBuilder sql = new StringBuilder();
            object[] parameters = new object[] { new SqlParameter("IDFilter", idFilter),
                                                     new SqlParameter("Status", (short)Enums.StatusType.Active),
                                                     new SqlParameter("IDCulture", idCulture)};

            sql.AppendLine("select p.*, pc.Description from purpose p");
            sql.AppendLine("	inner join purposeculture pc on (p.idpurpose = pc.idpurpose and pc.idculture = @IDCulture)");
            sql.AppendLine("where p.idpurpose not in(select idpurpose from filterpurpose fp where fp.idfilter = @IDFilter)");
            sql.AppendLine("and p.status = @Status");

            using (var context = new ImobeNetContext())
            {
                return context.Database.SqlQuery<Model.PurposeExtended>(sql.ToString(), parameters).ToList();
            }
        }

        public IList<Model.PurposeExtended> ListVinculatedPurpose(string idCulture, int idFilter)
        {
            StringBuilder sql = new StringBuilder();
            object[] parameters = new object[] { new SqlParameter("IDFilter", idFilter),
                                                     new SqlParameter("Status", (short)Enums.StatusType.Active),
                                                     new SqlParameter("IDCulture", idCulture)};

            sql.AppendLine("select p.*, pc.Description from purpose p");
            sql.AppendLine("	inner join purposeculture pc on (p.idpurpose = pc.idpurpose and pc.idculture = @IDCulture)");
            sql.AppendLine("where p.idpurpose in(select idpurpose from filterpurpose fp where fp.idfilter = @IDFilter)");
            sql.AppendLine("and p.status = @Status");

            using (var context = new ImobeNetContext())
            {
                return context.Database.SqlQuery<Model.PurposeExtended>(sql.ToString(), parameters).ToList();
            }

        }

        //public SelectList ListAvailablePurposeAsSelectList(int idFilter)
        //{
        //    return new SelectList(this.ListAvailablePurpose(idFilter), "IDPurpose", "Description");
        //}

        //public SelectList ListVinculatedPurposeAsSelectList(int idFilter)
        //{
        //    return new SelectList(this.ListVinculatedPurpose(idFilter), "IDPurpose", "Description");
        //}

        public Purpose GetInsert(string purposeName)
        {
            using (var context = new ImobeNetContext())
            {
                var purpose = (from pc in context.PurposeCulture
                               join p in context.Purpose on pc.IDPurpose equals p.IDPurpose
                               where pc.Description == purposeName
                               select p).FirstOrDefault();

                if (purpose == null)
                {
                    purpose = new Purpose()
                    {
                        IDIcon = 6,
                        Status = (short)Enums.StatusType.Pending,
                        CreateDate = DateTime.Now,
                        ModifyDate = DateTime.Now,
                        CreatedBy = "AUTO",
                        ModifiedBy = "AUTO"
                    };
                    this.Insert(purpose);

                    foreach (var culture in ServiceContext.CultureService.GetAll())
                    {
                        ServiceContext.PurposeCultureService.Insert(new PurposeCulture()
                        {
                            Description = purposeName,
                            IDCulture = culture.IDCulture,
                            IDPurpose = purpose.IDPurpose

                        });
                    }
                }

                return purpose;
            }
        }
    }
}
