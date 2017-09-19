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
using System.Data.SqlClient;

namespace SpongeSolutions.ServicoDireto.Services.StructureSystem.Implementation
{
    public class AttributeTypeService : IAttributeTypeContract
    {
        public void InsertUpdate(Model.AttributeType entity, ICollection<AttributeTypeCulture> attributeTypeCulture, int[] attributes, Enums.ActionType actionType)
        {
            using (var context = new ImobeNetContext())
            {
                using (TransactionScope scopeOfTransaction = new TransactionScope())
                {
                    try
                    {
                        var attributeTypeRepository = new BaseRepository<Model.AttributeType>(context);
                        var attributeTypeCultureRepository = new BaseRepository<AttributeTypeCulture>(context);
                        var attributeAttributeTypeRepository = new BaseRepository<Model.Attribute_AttributeType>(context);

                        if (actionType == Enums.ActionType.Insert)
                            attributeTypeRepository.Insert(entity);
                        else
                            if (actionType == Enums.ActionType.Update)
                            attributeTypeRepository.Update(entity);

                        context.SaveChanges();

                        var attributeTypes = (from p in context.AttributeTypeCulture
                                              where p.IDAttributeType == entity.IDAttributeType
                                              select p);

                        //Deletando os idiomas
                        if (attributeTypes != null && attributeTypes.Count() > 0)
                        {
                            foreach (var attributeType in attributeTypes)
                                attributeTypeCultureRepository.Delete(attributeType);

                            context.SaveChanges();
                        }

                        // inserindo novos
                        foreach (var attributeType in attributeTypeCulture)
                        {
                            attributeType.IDAttributeType = entity.IDAttributeType.Value;
                            attributeTypeCultureRepository.Insert(attributeType);
                        }

                        context.SaveChanges();

                        var attrAttributeTypes = from aat in context.Attribute_AttributeType
                                                 where aat.IDAttributeType == entity.IDAttributeType
                                                 select aat;

                        //Deletando os atributos vinculados
                        if (attrAttributeTypes != null && attrAttributeTypes.Count() > 0)
                        {
                            foreach (var attrAttributeType in attrAttributeTypes)
                                attributeAttributeTypeRepository.Delete(attrAttributeType);

                            context.SaveChanges();
                        }

                        if (attributes != null && attributes.Count() > 0)
                        {
                            //Inserindo novos atributos
                            foreach (var attribute in attributes)
                                attributeAttributeTypeRepository.Insert(new Attribute_AttributeType() { IDAttributeType = entity.IDAttributeType.Value, IDAttribute = attribute });

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

        public void Insert(Model.AttributeType entity, ICollection<AttributeTypeCulture> AttributeTypeCulture, int[] attributes)
        {
            this.InsertUpdate(entity, AttributeTypeCulture, attributes, Enums.ActionType.Insert);
        }

        public void Insert(Model.AttributeType entity)
        {
            using (var context = new ImobeNetContext())
            {
                var attributeTypeRepository = new BaseRepository<Model.AttributeType>(context);
                attributeTypeRepository.Insert(entity);
                attributeTypeRepository.SaveChanges();
            }
        }

        public void Delete(Model.AttributeType entity)
        {
            using (var context = new ImobeNetContext())
            {
                var attributeTypeRepository = new BaseRepository<Model.AttributeType>(context);
                attributeTypeRepository.Delete(entity);
                attributeTypeRepository.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var context = new ImobeNetContext())
            {
                var attributeTypeRepository = new BaseRepository<Model.AttributeType>(context);
                attributeTypeRepository.Delete(attributeTypeRepository.GetById(id));
                attributeTypeRepository.SaveChanges();
            }
        }

        public void Update(Model.AttributeType entity)
        {
            using (var context = new ImobeNetContext())
            {
                var attributeTypeRepository = new BaseRepository<Model.AttributeType>(context);
                attributeTypeRepository.Update(entity);
                attributeTypeRepository.SaveChanges();
            }
        }

        public void Update(Model.AttributeType entity, ICollection<AttributeTypeCulture> AttributeTypeCulture, int[] attributes)
        {
            this.InsertUpdate(entity, AttributeTypeCulture, attributes, Enums.ActionType.Update);
        }

        public void Delete(int[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var attributeTypeRepository = new BaseRepository<Model.AttributeType>(context);
                var items = attributeTypeRepository.Fetch().Where(p => ids.Contains(p.IDAttributeType.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                        attributeTypeRepository.Delete(item);

                    attributeTypeRepository.SaveChanges();
                }
            }
        }

        public void Inactivate(int[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var attributeTypeRepository = new BaseRepository<Model.AttributeType>(context);
                var items = attributeTypeRepository.Fetch().Where(p => ids.Contains(p.IDAttributeType.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                    {
                        item.Status = (short)Enums.StatusType.Inactive;
                        item.ModifiedBy = ServiceContext.ActiveUserName;
                        item.ModifyDate = DateTime.Now;
                        attributeTypeRepository.Update(item);
                    }
                    attributeTypeRepository.SaveChanges();
                }
            }
        }

        public IList<Model.AttributeType> GetAll()
        {
            using (var context = new ImobeNetContext())
            {
                var attributeTypeRepository = new BaseRepository<Model.AttributeType>(context);
                return attributeTypeRepository.GetAll().ToList();
            }
        }

        public IList<Model.AttributeTypeExtended> GetAll(string idCulture)
        {
            StringBuilder sql = new StringBuilder();
            object[] parameters = new object[] {
                new SqlParameter("IDCulture", idCulture)
            };
            //
            using (var context = new ImobeNetContext())
            {
                sql.AppendLine("exec pAttributeType_List @IDCulture");
                return context.Database.SqlQuery<Model.AttributeTypeExtended>(sql.ToString(), parameters).ToList();
            }
        }

        public Model.AttributeType GetById(int id)
        {
            return this.GetAll().Where(p => p.IDAttributeType == id).FirstOrDefault();
        }

        public Model.AttributeType GetByAcronym(string acronym)
        {
            return this.GetAll().Where(p => p.Acronym == acronym).FirstOrDefault();
        }

        public IList<Model.AttributeType> GetByStatus(short status)
        {
            return this.GetAll().Where(p => p.Status == status).ToList();
        }

        public IList<Model.AttributeType> GetAllActive()
        {
            return this.GetByStatus((short)Enums.StatusType.Active).ToList();
        }

        public IList<AttributeTypeCultureExtended> ListAttributeTypeCulture(int? idAttributeType)
        {
            List<AttributeTypeCultureExtended> result = new List<AttributeTypeCultureExtended>();
            foreach (var item in ServiceContext.CultureService.GetAllActive())
            {
                var attributeTypeCulture = new AttributeTypeCultureExtended() { IDCulture = item.IDCulture, IconPath = item.IconPath, CultureName = item.Name };
                if (idAttributeType.HasValue)
                {
                    var culture = ServiceContext.AttributeTypeCultureService.GetById(idAttributeType.Value, item.IDCulture);
                    if (culture != null)
                        attributeTypeCulture.Description = culture.Description;
                }
                result.Add(attributeTypeCulture);
            }

            return result;
        }
    }
}
