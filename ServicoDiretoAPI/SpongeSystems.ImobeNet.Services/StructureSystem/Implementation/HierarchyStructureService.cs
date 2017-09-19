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
using System.Data.Entity.Validation;

namespace SpongeSolutions.ServicoDireto.Services.StructureSystem.Implementation
{
    public class HierarchyStructureService : IHierarchyStructureContract
    {
        public void InsertUpdate(Model.HierarchyStructure entity, ICollection<HierarchyStructureCulture> hierarchyStructureCulture, int[] vinculatedStructure, Enums.ActionType actionType)
        {
            using (var context = new ImobeNetContext())
            {
                using (TransactionScope scopeOfTransaction = new TransactionScope())
                {
                    try
                    {
                        var hierarchyStructureRepository = new BaseRepository<Model.HierarchyStructure>(context);
                        var hierarchyStructureCultureRepository = new BaseRepository<HierarchyStructureCulture>(context);

                        if (actionType == Enums.ActionType.Insert)
                            hierarchyStructureRepository.Insert(entity);
                        else
                            if (actionType == Enums.ActionType.Update)
                            hierarchyStructureRepository.Update(entity);

                        context.SaveChanges();

                        #region [Culture]

                        var structureCultures = (from p in context.HierarchyStructureCulture
                                                 where p.IDHierarchyStructure == entity.IDHierarchyStructure
                                                 select p);


                        //Deletando variacoes de idioma
                        if (structureCultures != null && structureCultures.Count() > 0)
                        {
                            foreach (var structureCulture in structureCultures)
                                hierarchyStructureCultureRepository.Delete(structureCulture);

                            context.SaveChanges();
                        }


                        //Inserindo variacoes de idioma
                        if (hierarchyStructureCulture != null && hierarchyStructureCulture.Count() > 0)
                        {
                            foreach (var item in hierarchyStructureCulture)
                            {
                                item.IDHierarchyStructure = entity.IDHierarchyStructure.Value;
                                hierarchyStructureCultureRepository.Insert(item);
                            }
                            context.SaveChanges();
                        }
                        #endregion

                        #region [Children Structure]

                        var childStructures = (from p in context.HierarchyStructure
                                               where p.IDHierarchyStructureParent == entity.IDHierarchyStructure
                                               select p);

                        //Desvinculando os filhos
                        if (childStructures != null && childStructures.Count() > 0)
                        {
                            foreach (var childStructure in childStructures)
                            {
                                childStructure.IDHierarchyStructureParent = null;
                                hierarchyStructureRepository.Update(childStructure);
                            }

                            context.SaveChanges();
                        }


                        //Vinculando novos filhos
                        if (vinculatedStructure != null && vinculatedStructure.Count() > 0)
                        {
                            foreach (var item in vinculatedStructure)
                            {
                                foreach (var updateItem in (from hs in context.HierarchyStructure
                                                            where vinculatedStructure.Contains(hs.IDHierarchyStructure.Value)
                                                            select hs))
                                {
                                    updateItem.IDHierarchyStructureParent = entity.IDHierarchyStructure;
                                    updateItem.ModifiedBy = ServiceContext.ActiveUserName;
                                    updateItem.ModifyDate = DateTime.Now;

                                    hierarchyStructureRepository.Update(updateItem);
                                }
                                context.SaveChanges();
                            }
                        }

                        #endregion

                        scopeOfTransaction.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
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

        public void Insert(Model.HierarchyStructure entity, ICollection<HierarchyStructureCulture> hierarchyStructureCulture, int[] vinculatedStruture)
        {
            this.InsertUpdate(entity, hierarchyStructureCulture, vinculatedStruture, Enums.ActionType.Insert);
        }

        public void Insert(Model.HierarchyStructure entity)
        {
            using (var context = new ImobeNetContext())
            {
                var hierarchyStructureRepository = new BaseRepository<Model.HierarchyStructure>(context);
                hierarchyStructureRepository.Insert(entity);
                hierarchyStructureRepository.SaveChanges();
            }
        }

        public void Delete(Model.HierarchyStructure entity)
        {
            using (var context = new ImobeNetContext())
            {
                var hierarchyStructureRepository = new BaseRepository<Model.HierarchyStructure>(context);
                hierarchyStructureRepository.Delete(entity);
                hierarchyStructureRepository.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var context = new ImobeNetContext())
            {
                var hierarchyStructureRepository = new BaseRepository<Model.HierarchyStructure>(context);
                hierarchyStructureRepository.Delete(hierarchyStructureRepository.GetById(id));
                hierarchyStructureRepository.SaveChanges();
            }
        }

        public void Update(Model.HierarchyStructure entity)
        {
            using (var context = new ImobeNetContext())
            {
                var hierarchyStructureRepository = new BaseRepository<Model.HierarchyStructure>(context);
                hierarchyStructureRepository.Update(entity);
                hierarchyStructureRepository.SaveChanges();
            }
        }

        public void Update(Model.HierarchyStructure entity, ICollection<HierarchyStructureCulture> hierarchyStructureCulture, int[] vinculatedStruture)
        {
            this.InsertUpdate(entity, hierarchyStructureCulture, vinculatedStruture, Enums.ActionType.Update);
        }

        public void Delete(int[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var hierarchyStructureRepository = new BaseRepository<Model.HierarchyStructure>(context);
                var items = hierarchyStructureRepository.Fetch().Where(p => ids.Contains(p.IDHierarchyStructure.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                        hierarchyStructureRepository.Delete(item);

                    hierarchyStructureRepository.SaveChanges();
                }
            }
        }

        public void Inactivate(int[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var hierarchyStructureRepository = new BaseRepository<Model.HierarchyStructure>(context);
                var items = hierarchyStructureRepository.Fetch().Where(p => ids.Contains(p.IDHierarchyStructure.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                    {
                        item.Status = (short)Enums.StatusType.Inactive;
                        item.ModifiedBy = ServiceContext.ActiveUserName;
                        hierarchyStructureRepository.Update(item);
                    }

                    hierarchyStructureRepository.SaveChanges();
                }
            }
        }

        public IList<Model.HierarchyStructureExtended> GetAll(out int recordCount, string sortType = null, int startRowIndex = -1, int maximumRows = -1, int idHierarchyStructure = -1, short status = -1, string idCulture = null)
        {
            using (var context = new ImobeNetContext())
            {
                recordCount = 0;
                SqlParameter recordCountParameter = new SqlParameter("RecordCount", 0);
                recordCountParameter.Direction = System.Data.ParameterDirection.Output;

                StringBuilder sql = new StringBuilder();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("IDHierarchyStructure", idHierarchyStructure));
                parameters.Add(new SqlParameter("Status", status));
                parameters.Add(new SqlParameter("IDCulture", idCulture));
                parameters.Add(new SqlParameter("StartRowIndex", startRowIndex));
                parameters.Add(new SqlParameter("MaximumRows", maximumRows));
                parameters.Add(recordCountParameter);

                //sql.AppendLine("exec pHierarchyStructure_List " + string.Join(",", parameters.Select(p => "@" + p.ParameterName)));
                sql.AppendLine("exec pHierarchyStructure_List @IDHierarchyStructure, @Status, @IDCulture, @StartRowIndex, @MaximumRows, @RecordCount out");
                var result = context.Database.SqlQuery<Model.HierarchyStructureExtended>(sql.ToString(), parameters.ToArray()).ToList();
                recordCount = Convert.ToInt32(recordCountParameter.Value);
                return result;
            }
        }

        public Model.HierarchyStructureExtended GetByIdExtended(int id)
        {
            int count;
            return this.GetAll(out count, idHierarchyStructure: id).FirstOrDefault();
        }

        public Model.HierarchyStructure GetById(int id)
        {
            using (var context = new ImobeNetContext())
            {
                var hierarchyStructureRepository = new BaseRepository<Model.HierarchyStructure>(context);
                return hierarchyStructureRepository.GetById(id);
            }
        }

        public IList<Model.HierarchyStructureExtended> ListVinculated(string idCulture, int idHierarchyStructureParent)
        {
            using (var context = new ImobeNetContext())
            {
                var result = (from hs in context.HierarchyStructure.ToList()
                              join hsc in context.HierarchyStructureCulture on new { IDHierarchyStructure = hs.IDHierarchyStructure.Value, IDCulture = idCulture} equals new { IDHierarchyStructure = hsc.IDHierarchyStructure, IDCulture = hsc.IDCulture }
                              join i in context.Icon on hs.IDIcon equals i.IDIcon
                              where hs.IDHierarchyStructureParent == idHierarchyStructureParent
                              orderby hsc.Description
                              select new Model.HierarchyStructureExtended
                              {
                                  Description = hsc.Description,
                                  IDHierarchyStructure = hs.IDHierarchyStructure,
                                  IDHierarchyStructureParent = hs.IDHierarchyStructureParent,
                                  IconPath = i.Path,
                                  IconName = i.Name,
                                  IDIcon = hs.IDIcon,
                                  CreateDate = hs.CreateDate,
                                  CreatedBy = hs.CreatedBy,
                                  ModifiedBy = hs.ModifiedBy,
                                  ModifyDate = hs.ModifyDate,
                                  Status = hs.Status,

                              });
                return result.ToList();
            }
        }

        public IList<Model.HierarchyStructureExtended> ListAvailable(string idCulture, int idHierarchyStructureParent)
        {
            using (var context = new ImobeNetContext())
            {
                var result = (from hs in context.HierarchyStructure.ToList()
                              join hsc in context.HierarchyStructureCulture on new { IDHierarchyStructure = hs.IDHierarchyStructure.Value, IDCulture = idCulture } equals new { IDHierarchyStructure = hsc.IDHierarchyStructure, IDCulture = hsc.IDCulture }
                              join i in context.Icon on hs.IDIcon equals i.IDIcon
                              where hs.IDHierarchyStructureParent != idHierarchyStructureParent
                              orderby hsc.Description
                              select new Model.HierarchyStructureExtended
                              {
                                  Description = hsc.Description,
                                  IDHierarchyStructure = hs.IDHierarchyStructure,
                                  IDHierarchyStructureParent = hs.IDHierarchyStructureParent,
                                  IconPath = i.Path,
                                  IDIcon = hs.IDIcon,
                                  IconName = i.Name,
                                  CreateDate = hs.CreateDate,
                                  CreatedBy = hs.CreatedBy,
                                  ModifiedBy = hs.ModifiedBy,
                                  ModifyDate = hs.ModifyDate,
                                  Status = hs.Status,
                              });
                return result.ToList();
            }
        }

        //public SelectList ListVinculatedAsSelectList(int idHierarchyStructureParent)
        //{
        //    return new SelectList(this.ListVinculated(idHierarchyStructureParent), "IDHierarchyStructure", "Description");
        //}

        //public SelectList ListAvailableAsSelectList(int idHierarchyStructureParent)
        //{
        //    return new SelectList(this.ListAvailable(idHierarchyStructureParent), "IDHierarchyStructure", "Description");
        //}

        public IList<Model.HierarchyStructure> ListVinculatedByPurpose(string idCulture, int idPurpose)
        {
            using (var context = new ImobeNetContext())
            {

                StringBuilder sql = new StringBuilder();
                object[] parameters = new object[] { new SqlParameter("IDPurpose", idPurpose),
                                                     new SqlParameter("Status", (short)Enums.StatusType.Active),
                                                     new SqlParameter("IDCulture", idCulture)};
                sql.AppendLine("select 	");
                sql.AppendLine(" hs.*, ");
                sql.AppendLine(" hsc.description,");
                sql.AppendLine(" i.*");
                sql.AppendLine(" from HierarchyStructure hs");
                sql.AppendLine(" left join Icon i on (hs.IDIcon = i.IDIcon)");
                sql.AppendLine(" inner join HierarchyStructureCulture hsc on (hs.IDHierarchyStructure = hsc.IDHierarchyStructure )");
                sql.AppendLine(" inner join HierarchyStructurePurpose hsp on (hs.IDHierarchyStructure = hsp.IDHierarchyStructure )");
                sql.AppendLine(" where hsc.IDCulture = @IDCulture");
                sql.AppendLine("    and hs.Status = @Status");
                sql.AppendLine("    and hsp.IDPurpose = @IDPurpose ");
                sql.AppendLine(" order by hsc.description");
                return context.Database.SqlQuery<Model.HierarchyStructure>(sql.ToString(), parameters).ToList();
            }
        }

        public IList<Model.HierarchyStructure> ListAvailableByPurpose(string idCulture, int idPurpose)
        {
            using (var context = new ImobeNetContext())
            {

                StringBuilder sql = new StringBuilder();
                object[] parameters = new object[] { new SqlParameter("IDPurpose", idPurpose),
                                                     new SqlParameter("Status", (short)Enums.StatusType.Active),
                                                     new SqlParameter("IDCulture", idCulture)};
                sql.AppendLine("select 	");
                sql.AppendLine(" hs.*, ");
                sql.AppendLine(" hsc.description,");
                sql.AppendLine(" i.*");
                sql.AppendLine(" from HierarchyStructure hs");
                sql.AppendLine(" left join Icon i on (hs.IDIcon = i.IDIcon)");
                sql.AppendLine(" inner join HierarchyStructureCulture hsc on (hs.IDHierarchyStructure = hsc.IDHierarchyStructure )");
                //sql.AppendLine(" inner join HierarchyStructurePurpose hsp on (hs.IDHierarchyStructure = hsp.IDHierarchyStructure )");
                sql.AppendLine(" where hsc.IDCulture = @IDCulture");
                sql.AppendLine("    and hs.Status = @Status");
                if (idPurpose == 0)
                    sql.AppendLine("    and hs.IDHierarchyStructure not in (select IDHierarchyStructure from HierarchyStructurePurpose where IDPurpose = @IDPurpose )");
                sql.AppendLine(" order by hsc.description");
                return context.Database.SqlQuery<Model.HierarchyStructure>(sql.ToString(), parameters).ToList();
            }
        }

        //public IList<HierarchyStructureExtended> GetAll(out int recordCount, string idCulture, string sortType = null, int startRowIndex = -1, int maximumRows = -1)
        //{
        //    throw new NotImplementedException();
        //}

        public HierarchyStructure GetInsert(string categoryName, int? iDPurpose)
        {
            using (var context = new ImobeNetContext())
            {
                var hierarchy = (from hc in context.HierarchyStructureCulture
                                 join h in context.HierarchyStructure on hc.IDHierarchyStructure equals h.IDHierarchyStructure
                                 where hc.Description == categoryName
                                 select h).FirstOrDefault();

                if (hierarchy == null)
                {
                    hierarchy = new HierarchyStructure()
                    {
                        IDHierarchyStructureParent = 1, //residencial
                        IDIcon = 6,
                        Status = (short)Enums.StatusType.Pending,
                        CreateDate = DateTime.Now,
                        CreatedBy = "AUTO",
                        ModifyDate = DateTime.Now,
                        ModifiedBy = "AUTO"
                    };

                    this.Insert(hierarchy);

                    foreach (var culture in ServiceContext.CultureService.GetAll())
                    {
                        ServiceContext.HierarchyStructureCultureService.Insert(new HierarchyStructureCulture()
                        {
                            IDHierarchyStructure = hierarchy.IDHierarchyStructure.Value,
                            Description = categoryName,
                            IDCulture = culture.IDCulture
                        });
                    }
                }

                return hierarchy;
            }
        }

        public IList<HierarchyStructure> GetAll()
        {
            using (var context = new ImobeNetContext())
            {
                var hierarchyStructureRepository = new BaseRepository<Model.HierarchyStructure>(context);
                return hierarchyStructureRepository.GetAll().ToList();
            }
        }

        public IList<Model.HierarchyStructureExtended> ListCategory(out int recordCount, string idCulture, string sortType = null, int startRowIndex = -1, int maximumRows = -1)
        {
            return this.GetAll(out recordCount, null, startRowIndex, maximumRows, idCulture: idCulture).Where(p => p.IDHierarchyStructureParent == null).ToList();
        }

        public IList<Model.HierarchyStructureExtended> ListType(out int recordCount, string idCulture, string sortType = null, int startRowIndex = -1, int maximumRows = -1)
        {
            return this.GetAll(out recordCount, null, startRowIndex, maximumRows, idCulture: idCulture).Where(p => p.IDHierarchyStructureParent != null).ToList();
        }

        public IList<HierarchyStructureCulture> ListCulture(int idHierarchyStructure)
        {
            List<HierarchyStructureCulture> result = new List<HierarchyStructureCulture>();
            foreach (var item in ServiceContext.CultureService.GetAllActive())
            {
                var hierarchy = new HierarchyStructureCulture() { IDCulture = item.IDCulture, IconPath = item.IconPath, CultureName = item.Name };
                if (idHierarchyStructure != 0)
                {
                    var culture = ServiceContext.HierarchyStructureCultureService.GetById(idHierarchyStructure, item.IDCulture);
                    if (culture != null)
                        hierarchy.Description = culture.Description;
                }
                result.Add(hierarchy);
            }

            return result;
        }
    }
}

