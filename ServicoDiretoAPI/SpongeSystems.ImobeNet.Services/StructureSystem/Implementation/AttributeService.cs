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
using SpongeSolutions.Core.LINQ;
using System.Data.Entity.Validation;

namespace SpongeSolutions.ServicoDireto.Services.StructureSystem.Implementation
{
    public class AttributeService : IAttributeContract
    {
        public void InsertUpdate(Model.Attribute entity, ICollection<AttributeCulture> attributeCulture, Enums.ActionType actionType)
        {
            using (var context = new ImobeNetContext())
            {
                //using (TransactionScope scopeOfTransaction = new TransactionScope())
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var attributeRepository = new BaseRepository<Model.Attribute>(context);
                        var AttributeCultureRepository = new BaseRepository<AttributeCulture>(context);

                        if (actionType == Enums.ActionType.Insert)
                            attributeRepository.Insert(entity);
                        else
                            if (actionType == Enums.ActionType.Update)
                            attributeRepository.Update(entity);

                        context.SaveChanges();

                        var Attributes = (from a in context.AttributeCulture
                                          where a.IDAttribute == entity.IDAttribute
                                          select a);

                        //Deletando os idiomas
                        if (Attributes != null && Attributes.Count() > 0)
                        {
                            foreach (var Attribute in Attributes)
                                AttributeCultureRepository.Delete(Attribute);

                            context.SaveChanges();
                        }

                        // inserindo novos
                        foreach (var Attribute in attributeCulture)
                        {
                            Attribute.IDAttribute = entity.IDAttribute.Value;
                            AttributeCultureRepository.Insert(Attribute);
                        }

                        context.SaveChanges();

                        transaction.Commit();
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
                    finally
                    {
                        transaction.Dispose();
                    }
                }
            }
        }

        public void Insert(Model.Attribute entity, ICollection<AttributeCulture> attributeCulture)
        {
            this.InsertUpdate(entity, attributeCulture, Enums.ActionType.Insert);
        }

        public void Insert(Model.Attribute entity)
        {
            using (var context = new ImobeNetContext())
            {
                var attributeRepository = new BaseRepository<Model.Attribute>(context);
                attributeRepository.Insert(entity);
                attributeRepository.SaveChanges();
            }
        }

        public void Delete(Model.Attribute entity)
        {
            using (var context = new ImobeNetContext())
            {
                var attributeRepository = new BaseRepository<Model.Attribute>(context);
                attributeRepository.Delete(entity);
                attributeRepository.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var context = new ImobeNetContext())
            {
                var attributeRepository = new BaseRepository<Model.Attribute>(context);
                attributeRepository.Delete(attributeRepository.GetById(id));
                attributeRepository.SaveChanges();
            }
        }

        public void Update(Model.Attribute entity)
        {
            using (var context = new ImobeNetContext())
            {
                var attributeRepository = new BaseRepository<Model.Attribute>(context);
                attributeRepository.Update(entity);
                attributeRepository.SaveChanges();
            }
        }

        public void Update(Model.Attribute entity, ICollection<AttributeCulture> attributeCulture)
        {
            this.InsertUpdate(entity, attributeCulture, Enums.ActionType.Update);
        }

        public void Delete(int[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var attributeRepository = new BaseRepository<Model.Attribute>(context);
                var items = attributeRepository.Fetch().Where(a => ids.Contains(a.IDAttribute.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                        attributeRepository.Delete(item);

                    attributeRepository.SaveChanges();
                }
            }
        }

        public void Inactivate(int[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var attributeRepository = new BaseRepository<Model.Attribute>(context);
                var items = attributeRepository.Fetch().Where(a => ids.Contains(a.IDAttribute.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                    {
                        item.Status = (short)Enums.StatusType.Inactive;
                        item.ModifiedBy = ServiceContext.ActiveUserName;
                        item.ModifyDate = DateTime.Now;
                        attributeRepository.Update(item);
                    }

                    attributeRepository.SaveChanges();
                }
            }
        }

        public IList<Model.AttributeExtended> GetAll(out int recordCount, string idCulture, string sortType = null, int startRowIndex = -1, int maximumRows = -1, int idAttribute = -1)
        {
            using (var context = new ImobeNetContext())
            {
                recordCount = 0;
                SqlParameter recordCountParameter = new SqlParameter("RecordCount", 0);
                recordCountParameter.Direction = System.Data.ParameterDirection.Output;

                StringBuilder sql = new StringBuilder();
                object[] parameters = new object[] { new SqlParameter("Status", -1) ,
                                                     new SqlParameter("StartRowIndex", startRowIndex) ,
                                                     new SqlParameter("IDAttribute", idAttribute) ,
                                                     new SqlParameter("MaximumRows", maximumRows) ,
                                                     recordCountParameter,
                                                     new SqlParameter("IDCulture", idCulture)};

                sql.AppendLine("exec pAttribute_GetAll @Status, @IDAttribute, @IDCulture, @StartRowIndex, @MaximumRows, @RecordCount out");
                var result = context.Database.SqlQuery<Model.AttributeExtended>(sql.ToString(), parameters).ToList();
                recordCount = Convert.ToInt32(recordCountParameter.Value);
                return result;
            }
        }

        public IList<Model.AttributeExtended> GetByStatus(short status, out int recordCount, string idCulture, string sortType = null, int startRowIndex = -1, int maximumRows = -1)
        {
            return this.GetAll(out recordCount, idCulture, sortType, startRowIndex, maximumRows).Where(a => a.Status == status).ToList();
        }

        public IList<Model.AttributeExtended> GetAllActive(out int recordCount, string idCulture, string sortType = null, int startRowIndex = -1, int maximumRows = -1)
        {
            return this.GetByStatus((short)Enums.StatusType.Active, out recordCount, idCulture, sortType, startRowIndex, maximumRows).ToList();
        }

        public IList<Model.AttributeExtended> ListVinculatedAttribute(string idCulture, int? idAttributeType = null)
        {
            StringBuilder sql = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();
            int[] vinculatedAttr = new int[] { };
            using (var context = new ImobeNetContext())
            {
                if (idAttributeType.HasValue)
                {
                    vinculatedAttr = (from aat in context.Attribute_AttributeType.ToList()
                                      where (aat.IDAttributeType == idAttributeType || !idAttributeType.HasValue)
                                      select aat.IDAttribute).ToArray();
                }

                parameters.Add(new SqlParameter("IDCulture", idCulture));
                parameters.Add(new SqlParameter("IDAttributeType", idAttributeType));
                parameters.Add(new SqlParameter("IDAttributes", string.Join(",", vinculatedAttr)));

                sql.AppendLine("exec pAttribute_ListVinculated @IDAttributes, @IDCulture, @IDAttributeType");
                return context.Database.SqlQuery<AttributeExtended>(sql.ToString(), parameters.ToArray()).ToList();
            }
        }

        public IList<Model.AttributeExtended> ListAvailableAttribute(string idCulture, int? idAttributeType = null)
        {
            StringBuilder sql = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();
            int[] vinculatedAttr = new int[] { };
            using (var context = new ImobeNetContext())
            {
                if (idAttributeType.HasValue)
                {
                    vinculatedAttr = (from aat in context.Attribute_AttributeType.ToList()
                                      where (aat.IDAttributeType == idAttributeType || !idAttributeType.HasValue)
                                      select aat.IDAttribute).ToArray();
                }

                parameters.Add(new SqlParameter("IDCulture", idCulture));
                parameters.Add(new SqlParameter("IDAttributeType", idAttributeType));
                parameters.Add(new SqlParameter("IDAttributes", string.Join(",", vinculatedAttr)));

                sql.AppendLine("exec pAttribute_ListAvailable @IDAttributes, @IDCulture, @IDAttributeType");
                return context.Database.SqlQuery<AttributeExtended>(sql.ToString(), parameters.ToArray()).ToList();

                //var result = (from a in context.Attribute.ToList()
                //              join ac in context.AttributeCulture on new { IDAttribute = a.IDAttribute.Value, IDCulture = ServiceContext.ActiveLanguage } equals new { IDAttribute = ac.IDAttribute, IDCulture = ac.IDCulture }
                //              where !vinculatedAttr.Contains(a.IDAttribute.Value)
                //              && a.Status == (short)Enums.StatusType.Active
                //              orderby ac.Name
                //              select new Model.AttributeExtended
                //              {
                //                  Name = ac.Name,
                //                  IDAttribute = a.IDAttribute,
                //                  IDIcon = a.IDIcon,
                //                  DisplayMask = a.DisplayMask,
                //                  EditMask = a.EditMask,
                //                  CreateDate = a.CreateDate,
                //                  CreatedBy = a.CreatedBy,
                //                  ModifiedBy = a.ModifiedBy,
                //                  ModifyDate = a.ModifyDate,
                //                  Status = a.Status,
                //              });
                //return result.ToList();
            }
        }

        public IList<Model.AttributeExtended> List(string idCulture, int idAttributeType, long idElement)
        {
            StringBuilder sql = new StringBuilder();
            object[] parameters = new object[] { new SqlParameter("IDAttributeType", idAttributeType),
                                                 new SqlParameter("Status", (short)Enums.StatusType.Active),
                                                 new SqlParameter("IDElement", idElement),
                                                 new SqlParameter("IDCulture", idCulture)};

            using (var context = new ImobeNetContext())
            {
                sql.AppendLine("	exec pAttribute_ListByAttributeType @IDAttributeType, @IDCulture, @Status, @IDElement");
                var result = context.Database.SqlQuery<Model.AttributeExtended>(sql.ToString(), parameters).ToList();
                for (int i = 0; i < result.Count; i++)
                {
                    if (!string.IsNullOrEmpty(result[i].DisplayMask))
                    {
                        decimal value = 0;
                        if (decimal.TryParse(result[i].Value, out value))
                            result[i].Value = string.Format(result[i].DisplayMask, value);
                    }
                }

                return result;
            }
        }

        public IList<Model.AttributeExtended> List(string idCulture, string acronym, long idElement)
        {
            StringBuilder sql = new StringBuilder();
            object[] parameters = new object[] { new SqlParameter("IDCulture", idCulture),
                                                 new SqlParameter("Status", (short)Enums.StatusType.Active),
                                                 new SqlParameter("IDElement", idElement),
                                                 new SqlParameter("Acronym", acronym)

            };

            using (var context = new ImobeNetContext())
            {
                sql.AppendLine("	exec pAttribute_List @IDCulture, @Status, @IDElement, null, @Acronym");
                return context.Database.SqlQuery<Model.AttributeExtended>(sql.ToString(), parameters).ToList();
            }
        }

        public IList<Model.AttributeExtended> List(string idCulture, long idElement)
        {
            StringBuilder sql = new StringBuilder();
            object[] parameters = new object[] { new SqlParameter("IDCulture", idCulture),
                                                 new SqlParameter("Status", (short)Enums.StatusType.Active),
                                                 new SqlParameter("IDElement", idElement)};

            using (var context = new ImobeNetContext())
            {
                sql.AppendLine("	exec pAttribute_List @IDCulture, @Status, @IDElement, null");
                return context.Database.SqlQuery<Model.AttributeExtended>(sql.ToString(), parameters).ToList();
            }
        }

        public IEnumerable<dynamic> AutoComplete(string idCulture, string text)
        {
            using (var context = new ImobeNetContext())
            {
                return (from a in context.Attribute.ToList()
                        join ac in context.AttributeCulture on new { IDAttribute = a.IDAttribute.Value, IDCulture = idCulture } equals new { IDAttribute = ac.IDAttribute, IDCulture = ac.IDCulture }
                        where ac.Name.Contains(text)
                        && a.Status == (short)Enums.StatusType.Active
                        orderby ac.Name
                        select new
                        {
                            id = a.IDAttribute,
                            value = ac.Name,
                            label = ac.Name
                        }).ToList();
            }
        }

        public IList<AttributeCultureExtended> ListAttributeCulture(int? idAttribute)
        {
            List<AttributeCultureExtended> result = new List<AttributeCultureExtended>();
            foreach (var item in ServiceContext.CultureService.GetAllActive())
            {
                var attributeCultureExtended = new AttributeCultureExtended() { IDCulture = item.IDCulture, IconPath = item.IconPath, CultureName = item.Name };
                if (idAttribute.HasValue)
                {
                    attributeCultureExtended.IDAttribute = idAttribute.Value;
                    var culture = ServiceContext.AttributeCultureService.GetById(idAttribute.Value, item.IDCulture);
                    if (culture != null)
                    {
                        attributeCultureExtended.Value = culture.Value;
                        attributeCultureExtended.Name = culture.Name;
                    }
                }
                result.Add(attributeCultureExtended);
            }

            return result;
        }

        public Model.Attribute GetById(int id)
        {
            using (var context = new ImobeNetContext())
            {
                var attributeRepository = new BaseRepository<Model.Attribute>(context);
                return attributeRepository.GetById(id);
            }
        }

        public IList<Model.Attribute> GetAll()
        {
            using (var context = new ImobeNetContext())
            {
                var attributeRepository = new BaseRepository<Model.Attribute>(context);
                return attributeRepository.GetAll().ToList();
            }

        }

        public int GetInsert(string attributeName)
        {
            string acronym = attributeName.Replace(' ', '_').ToUpper().Trim();
            int idAttribute = 0;
            using (var context = new ImobeNetContext())
            {
                var attribute = (from ac in context.Attribute
                                 where ac.Acronym == acronym
                                 select ac).FirstOrDefault();

                if (attribute == null)
                {
                    var attr = new Model.Attribute()
                    {
                        Acronym = acronym,
                        ModifyDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        CreatedBy = "AUTO",
                        ModifiedBy = "AUTO",
                        Status = (short)Enums.StatusType.Pending,
                        IDIcon = 6, //Blank
                        Required = false
                    };
                    this.Insert(attr);
                    idAttribute = attr.IDAttribute.Value;

                    foreach (var culture in ServiceContext.CultureService.GetAll())
                    {
                        ServiceContext.AttributeCultureService.Insert(new AttributeCulture()
                        {
                            Name = attributeName,
                            IDCulture = culture.IDCulture,
                            IDAttribute = attr.IDAttribute.Value
                        });
                    }
                }
                else
                {
                    idAttribute = attribute.IDAttribute.Value;
                }
            }

            return idAttribute;
        }

        public Model.Attribute GetByAcronym(string value)
        {
            using (var context = new ImobeNetContext())
            {
                var attributeRepository = new BaseRepository<Model.Attribute>(context);
                return attributeRepository.FirstOrDefault(p => p.Acronym == value);
            }
        }

        public Model.Attribute GetByGroup(string value)
        {
            using (var context = new ImobeNetContext())
            {
                var attributeRepository = new BaseRepository<Model.Attribute>(context);
                return attributeRepository.FirstOrDefault(p => p.Group == value);
            }

        }
    }
}
