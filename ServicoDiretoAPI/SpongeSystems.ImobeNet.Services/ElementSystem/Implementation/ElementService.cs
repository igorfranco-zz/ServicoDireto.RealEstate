using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using SpongeSolutions.ServicoDireto.Database;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Services.StructureSystem.Contracts;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using SpongeSolutions.ServicoDireto.Model.AccountSystem;
using SpongeSolutions.ServicoDireto.Services.ElementSystem.Contracts;
using System.Transactions;
using SpongeSolutions.Core.LINQ;
using System.Data.SqlClient;
using SpongeSolutions.Core.Helpers;
using System.IO;
using SpongeSolutions.Core.Helpers.Serialization;
using SpongeSolutions.ServicoDireto.Internationalization;
using System.Data.Entity.Validation;

namespace SpongeSolutions.ServicoDireto.Services.ElementSystem.Implementation
{
    public class ElementService : IElementContract
    {
        private void SetElementPosition(Element entity)
        {
            var city = ServiceContext.CityService.Get(entity.IDCity);
            //Buscando informações de Geolocalização;
            string address = String.Format("{0},{1},{2},{3}",
                    city.CountryName,
                    city.Name,
                    city.StateName,
                    entity.Address);

            try
            {
                var position = ServiceContext.PositioningService.GetGeographicPosition(address);
                if (position != null)
                {
                    entity.Latitude = position.Latitude;
                    entity.Longitude = position.Longitude;
                }
            }
            catch
            {
                entity.Latitude = 0;
                entity.Longitude = 0;
            }
        }

        public void InsertUpdate(Element entity, ICollection<ElementCulture> elementCulture, ICollection<ElementAttribute> elementAttribute, Enums.ActionType actionType)
        {
            if (entity.Latitude == 0 && entity.Longitude == 0)
                this.SetElementPosition(entity);

            using (var context = new ImobeNetContext())
            {
                //var options = new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted };
                //using (TransactionScope scopeOfTransaction = new TransactionScope(TransactionScopeOption.Required, options))
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var objectRepository = new BaseRepository<Element>(context);
                        var elementCultureRepository = new BaseRepository<ElementCulture>(context);
                        var elementAttributeRepository = new BaseRepository<ElementAttribute>(context);

                        if (actionType == Enums.ActionType.Insert)
                            objectRepository.Insert(entity);
                        else
                            if (actionType == Enums.ActionType.Update)
                            objectRepository.Update(entity);

                        context.SaveChanges();

                        //Buscando os valores de culturas
                        var cultureValues = (from oc in context.ElementCulture
                                             where oc.IDElement == entity.IDElement.Value
                                             select oc);

                        //Deletando as culturas
                        if (cultureValues != null && cultureValues.Count() > 0)
                        {
                            foreach (var item in cultureValues)
                                elementCultureRepository.Delete(item);

                            context.SaveChanges();
                        }

                        // Inserindo os novos valores da cultura
                        foreach (var item in elementCulture)
                        {
                            item.IDElement = entity.IDElement.Value;
                            elementCultureRepository.Insert(item);
                        }
                        context.SaveChanges();

                        if (elementAttribute != null)
                        {
                            var attr = ServiceContext.AttributeService.GetByGroup("IMG");
                            long? ignoreAttribute = (attr == null) ? 0 : attr.IDAttribute.Value;

                            //Buscando os atributos vinculados
                            var attributes = (from oc in context.ElementAttribute
                                              where oc.IDElement == entity.IDElement.Value && oc.IDAttribute != ignoreAttribute //deixar as imagens
                                              select oc);

                            //Deletando os attributos vinculados
                            if (attributes != null && attributes.Count() > 0)
                            {
                                foreach (var item in attributes)
                                    elementAttributeRepository.Delete(item);

                                context.SaveChanges();
                            }

                            // Inserindo os novos atributos
                            foreach (var item in elementAttribute)
                            {
                                item.IDElement = entity.IDElement.Value;
                                elementAttributeRepository.Insert(item);
                            }

                            context.SaveChanges();
                        }

                        //scopeOfTransaction.Complete();
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
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        //scopeOfTransaction.Dispose();
                        transaction.Dispose();
                    }
                }
            }
        }

        public void Insert(Element entity, ICollection<ElementCulture> elementCulture, ICollection<ElementAttribute> elementAttribute)
        {
            this.InsertUpdate(entity, elementCulture, elementAttribute, Enums.ActionType.Insert);
        }

        public void Insert(Element entity)
        {
            using (var context = new ImobeNetContext())
            {
                var objectRepository = new BaseRepository<Element>(context);
                objectRepository.Insert(entity);
                objectRepository.SaveChanges();
            }
        }

        public void Delete(Element entity)
        {
            using (var context = new ImobeNetContext())
            {
                var objectRepository = new BaseRepository<Element>(context);
                objectRepository.Delete(entity);
                objectRepository.SaveChanges();
            }
        }

        public void Delete(long id)
        {
            using (var context = new ImobeNetContext())
            {
                var objectRepository = new BaseRepository<Element>(context);
                objectRepository.Delete(this.GetById(id));
                objectRepository.SaveChanges();
            }
        }

        public void Update(Element entity)
        {
            using (var context = new ImobeNetContext())
            {
                var objectRepository = new BaseRepository<Element>(context);
                objectRepository.Update(entity);
                objectRepository.SaveChanges();
            }
        }

        public void Update(Element entity, ICollection<ElementCulture> elementCulture, ICollection<ElementAttribute> elementAttribute)
        {
            this.InsertUpdate(entity, elementCulture, elementAttribute, Enums.ActionType.Update);
        }

        public void Delete(long[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var objectRepository = new BaseRepository<Element>(context);
                var items = objectRepository.Fetch().Where(p => ids.Contains(p.IDElement.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                        objectRepository.Delete(item);

                    objectRepository.SaveChanges();
                }
            }
        }

        public IList<ElementExtended> GetAll(string idCulture, out int recordCount, int idCustomer = -1, long idElement = -1, short status = -1, string sortType = null, int startRowIndex = -1, int maximumRows = -1, bool igoreAttributes = false)
        {
            using (var context = new ImobeNetContext())
            {
                string sortExp = "NAME";
                string sortDirection = "ASC";

                recordCount = 0;
                SqlParameter recordCountParameter = new SqlParameter("RecordCount", 0);
                recordCountParameter.Direction = System.Data.ParameterDirection.Output;

                StringBuilder sql = new StringBuilder();
                List<object> parameters = new List<object>();
                parameters.Add(new SqlParameter("IDCustomer", idCustomer));
                parameters.Add(new SqlParameter("IDElement", idElement));
                parameters.Add(new SqlParameter("Status", status));
                parameters.Add(new SqlParameter("StartRowIndex", startRowIndex));
                parameters.Add(new SqlParameter("MaximumRows", maximumRows));
                parameters.Add(recordCountParameter);
                parameters.Add(new SqlParameter("IDCulture", idCulture));

                if (sortType != null)
                {
                    var sort = sortType.Split(' ');
                    if (sort.Length == 1)
                        sortExp = sort[0];
                    if (sort.Length == 2)
                    {
                        sortExp = sort[0];
                        sortDirection = sort[1];
                    }
                }

                sortExp = sortExp.Trim().ToUpper();
                sortDirection = sortDirection.Trim().ToUpper();

                parameters.Add(new SqlParameter("SortExpr", sortExp));
                parameters.Add(new SqlParameter("SortDir", sortDirection));
                sql.AppendLine("exec pElement_GetAll @IDElement, @IDCustomer, @Status, @IDCulture, @SortExpr, @SortDir, @StartRowIndex, @MaximumRows, @RecordCount out");
                var result = context.Database.SqlQuery<Model.ElementExtended>(sql.ToString(), parameters.ToArray()).ToList();
                if (!igoreAttributes)
                {
                    foreach (var item in result)
                    {
                        item.BasicAttributes = ServiceContext.AttributeService.List(idCulture, "BASIC", item.IDElement.Value);
                        item.StructureAttributes = SpongeSolutions.ServicoDireto.Services.ServiceContext.AttributeService.List(idCulture, "IE", item.IDElement.Value);
                    }
                }

                recordCount = Convert.ToInt32(recordCountParameter.Value);
                return result;
            }
        }

        public IList<Model.Element> GetAll()
        {
            using (var context = new ImobeNetContext())
            {
                return context.Database.SqlQuery<Model.Element>("select * from [element] with (nolock)").ToList();
            }
        }

        public Element GetById(long id)
        {
            return this.GetAll().Where(p => p.IDElement.Value == id).FirstOrDefault();
        }

        public ElementExtended GetByIdExtended(string idCulture, long id, bool igoreAttributes = false)
        {
            int recordCount = 0;
            return this.GetAll(idCulture, out recordCount, idElement: id, startRowIndex: 0, maximumRows: 1, igoreAttributes: igoreAttributes).FirstOrDefault();
        }

        public IList<Element> GetByStatus(short status)
        {
            return this.GetAll().Where(p => p.Status == status).ToList();
        }

        public IList<Element> GetAllActive()
        {
            return this.GetByStatus((short)Enums.StatusType.Active).ToList();
        }

        public void Inactivate(int[] ids)
        {
            //TODO:REVER
            using (var context = new ImobeNetContext())
            {
                StringBuilder sql = new StringBuilder();
                string elements = "-1";
                if (ids != null && ids.Length > 0)
                    elements = String.Join(",", ids);

                object[] parameters = new object[] {
                        new SqlParameter("IDElement", elements),
                        new SqlParameter("ModifiedBy", ServiceContext.ActiveUserName),
                        new SqlParameter("ModifyDate", DateTime.Now)
                };

                sql.AppendLine(" update [dbo].[element] ");
                sql.AppendLine(" set Status = 0, ");
                sql.AppendLine("     ModifiedBy = @ModifiedBy, ");
                sql.AppendLine("     ModifyDate = @ModifyDate ");
                sql.AppendLine(" where IDElement in (select * from [dbo].[IntegerCommaSplit](@IDElement)) ");
                context.Database.ExecuteSqlCommand(sql.ToString(), parameters);
            }
        }

        public IList<Element> GetByCustomer(int idCustomer, short? status)
        {
            return this.GetAll().Where(p => p.IDCustomer == idCustomer && (p.Status == status || !status.HasValue)).ToList();
        }

        public IList<ElementExtended> List(out int recordCount, ElementFilter filter, string sortType = null, int startRowIndex = -1, int maximumRows = -1, bool loadAttributes = false)
        {
            List<object> parameters = new List<object>();
            string sortExp = "NAME";
            string sortDirection = "ASC";
            SqlParameter recordCountParameter = new SqlParameter("RecordCount", 0);
            recordCountParameter.Direction = System.Data.ParameterDirection.Output;
            StringBuilder sql = new StringBuilder();
            parameters.Add(new SqlParameter("Filter", SerializationHelper.SerializeAsDocument<ElementFilter>(filter).OuterXml));
            parameters.Add(new SqlParameter("IDCulture", filter.IDCulture));
            parameters.Add(new SqlParameter("StartRowIndex", startRowIndex));
            parameters.Add(new SqlParameter("MaximumRows", maximumRows));
            parameters.Add(recordCountParameter);

            if (sortType != null)
            {
                var sort = sortType.Split(' ');
                if (sort.Length == 1)
                    sortExp = sort[0];
                if (sort.Length == 2)
                {
                    sortExp = sort[0];
                    sortDirection = sort[1];
                }
            }

            sortExp = sortExp.Trim().ToUpper();
            sortDirection = sortDirection.Trim().ToUpper();
            parameters.Add(new SqlParameter("SortExpr", sortExp));
            parameters.Add(new SqlParameter("SortDir", sortDirection));
            using (var context = new ImobeNetContext())
            {
                sql.AppendLine("exec pElement_GetByDistance @Filter, @IDCulture, @StartRowIndex, @MaximumRows, @SortExpr, @SortDir, @RecordCount out");
                var result = context.Database.SqlQuery<Model.ElementExtended>(sql.ToString(), parameters.ToArray()).ToList();
                if (loadAttributes)
                {
                    foreach (var item in result)
                    {
                        item.BasicAttributes = ServiceContext.AttributeService.List("BASIC", item.IDElement.Value);
                        item.StructureAttributes = ServiceContext.AttributeService.List("IE", item.IDElement.Value);
                    }
                }
                recordCount = Convert.ToInt32(recordCountParameter.Value);
                return result;
            }
        }

        public IList<ElementExtended> ListByAdsCategory(string idCulture, int idAdsCategory, int idCustomer)
        {
            StringBuilder sql = new StringBuilder();
            object[] parameters = new object[] { new SqlParameter("IDCulture", idCulture),
                                                 new SqlParameter("IDAdsCategory", idAdsCategory),
                                                 new SqlParameter("IDCustomer", idCustomer)
            };
            using (var context = new ImobeNetContext())
            {
                sql.AppendLine("exec pElement_ListByAdsCategory @IDCulture, @IDAdsCategory, @IDCustomer");
                var result = context.Database.SqlQuery<Model.ElementExtended>(sql.ToString(), parameters).ToList();
                foreach (var item in result)
                    item.BasicAttributes = ServiceContext.AttributeService.List("BASIC", item.IDElement.Value);

                return result;
            }
        }

        public SelectList ListByCustomer(string idCulture, int idCustomer)
        {
            StringBuilder sql = new StringBuilder();
            object[] parameters = new object[] { new SqlParameter("IDCulture", idCulture),
                                                 new SqlParameter("IDCustomer", idCustomer)};

            using (var context = new ImobeNetContext())
            {
                sql.AppendLine("exec pElement_List @IDCulture, @IDCustomer");
                var result = context.Database.SqlQuery<Model.ElementExtended>(sql.ToString(), parameters).ToList();
                return new SelectList(result, "IDElement", "Name");
            }
        }

        public IList<ElementExtended> ListAlert(string idCulture, int idCustomer, long idAlert)
        {
            StringBuilder sql = new StringBuilder();
            object[] parameters = new object[] { new SqlParameter("IDCulture", idCulture),
                                                 new SqlParameter("IDCustomer", idCustomer),
                                                 new SqlParameter("IDAlert", idAlert)
            };

            using (var context = new ImobeNetContext())
            {
                sql.AppendLine("exec pElement_ListByAlert @IDCustomer, @IDCulture, @IDAlert");
                var result = context.Database.SqlQuery<Model.ElementExtended>(sql.ToString(), parameters).ToList();
                return result;
            }
        }

        public IList<ElementExtended> ListFavorite(string idCulture, List<long> items)
        {
            StringBuilder sql = new StringBuilder();
            var elements = String.Join(",", items);
            object[] parameters = new object[] { new SqlParameter("IDCulture", idCulture ),
                                                 new SqlParameter("IDElements", elements)};

            using (var context = new ImobeNetContext())
            {
                sql.AppendLine("exec pElement_ListFavorite @IDCulture, @IDElements");
                var result = context.Database.SqlQuery<Model.ElementExtended>(sql.ToString(), parameters).ToList();
                return result;
            }
        }

        public object[][] ListPageViewXDetailView(long idCustomer)
        {
            List<object[]> listResult = new List<object[]>();
            using (var context = new ImobeNetContext())
            {
                object[] parameters = new object[] { new SqlParameter("IDCustomer", idCustomer),
                                                     new SqlParameter("Status", (short)Enums.StatusType.Active) };

                foreach (var item in context.Database.SqlQuery<GenericResult<long>>("select ISNULL(SUM(PageView),0) as Value1, ISNULL(SUM(DetailView),0) as Value2 from [element] with (nolock) where status = @Status and IDCustomer = @IDCustomer", parameters).ToList())
                {
                    listResult.Add(new object[] { Label.Activities, Label.Values });
                    listResult.Add(new object[] { Label.Visualizations, item.Value1 });
                    listResult.Add(new object[] { Label.Clicks, item.Value2 });
                }
            }
            return listResult.ToArray();
        }

        public void ChangeDefaultImage(long idElement, string path)
        {
            var element = this.GetById(idElement);
            element.DefaultPicturePath = path;
            Update(element);
        }

        public IEnumerable<ElementExtended> ListFeatured(string idCulture, int idCustomer)
        {
            StringBuilder sql = new StringBuilder();
            object[] parameters = new object[] { new SqlParameter("IDCulture", idCulture ),
                                                 new SqlParameter("IDCustomer", idCustomer)
            };

            using (var context = new ImobeNetContext())
            {
                sql.AppendLine("exec pElement_ListFeatured @IDCulture, @IDCustomer");
                return context.Database.SqlQuery<Model.ElementExtended>(sql.ToString(), parameters).ToList();
            }
        }

        public IEnumerable<ElementExtended> ListSimilar(string idCulture, long idElement, int idCustomer = 0)
        {
            StringBuilder sql = new StringBuilder();
            object[] parameters = new object[] { new SqlParameter("IDCulture", idCulture ),
                                                 new SqlParameter("IDCustomer", idCustomer),
                                                 new SqlParameter("IDElement", idElement)
            };

            using (var context = new ImobeNetContext())
            {
                sql.AppendLine("exec pElement_ListSimilar @IDCulture, @IDCustomer, @IDElement");
                return context.Database.SqlQuery<Model.ElementExtended>(sql.ToString(), parameters).ToList();
            }
        }

        public IEnumerable<ElementExtended> ListTopViewed(string idCulture, int idCustomer)
        {
            StringBuilder sql = new StringBuilder();
            object[] parameters = new object[] { new SqlParameter("IDCulture", idCulture),
                                                 new SqlParameter("IDCustomer", idCustomer)
            };

            using (var context = new ImobeNetContext())
            {
                sql.AppendLine("exec pElement_ListTopViewed @IDCulture, @IDCustomer");
                return context.Database.SqlQuery<Model.ElementExtended>(sql.ToString(), parameters).ToList();
            }
        }

        public Element GetByIDItemSite(string idItemSite)
        {
            using (var context = new ImobeNetContext())
            {
                var objectRepository = new BaseRepository<Element>(context);
                return objectRepository.FirstOrDefault(p => p.IDItemSite == idItemSite);
            }
        }

        public void InsertImage(long idElement, string path)
        {
            using (var context = new ImobeNetContext())
            {
                var repository = new BaseRepository<ElementAttribute>(context);
                var elementRepository = new BaseRepository<Element>(context);
                var element = elementRepository.First(p => p.IDElement == idElement);

                //caso nao haja imagem default entao setar uma
                if (string.IsNullOrEmpty(element.DefaultPicturePath))
                {
                    element.DefaultPicturePath = path;
                    elementRepository.Update(element);
                    elementRepository.SaveChanges();
                }

                repository.Insert(new ElementAttribute()
                {
                    IDElement = idElement,
                    IDAttribute = ServiceContext.AttributeService.GetByGroup("IMG").IDAttribute.Value,
                    Value = path
                });
                repository.SaveChanges();
            }
        }

        public string[] DeleteImage(long idElement, long? idElementAttribute = null)
        {
            string[] images = null;
            using (var context = new ImobeNetContext())
            {
                var attributeRepository = new BaseRepository<Model.Attribute>(context);
                var repositoryElementAttribute = new BaseRepository<ElementAttribute>(context);
                var elementRepository = new BaseRepository<Element>(context);
                var element = elementRepository.First(p => p.IDElement == idElement);

                var attr = attributeRepository.FirstOrDefault(p => p.Group == "IMG");
                if (idElementAttribute.HasValue)
                {
                    images = repositoryElementAttribute.Find(p => p.IDElementAttribute == idElementAttribute).Select(p => p.Value).ToArray();
                    repositoryElementAttribute.Delete(p => p.IDElementAttribute == idElementAttribute);
                }
                else
                {
                    images = repositoryElementAttribute.Find(p => p.IDElement == idElement && p.IDAttribute == attr.IDAttribute).Select(p => p.Value).ToArray();
                    repositoryElementAttribute.Delete(p => p.IDElement == idElement && p.IDAttribute == attr.IDAttribute);
                }

                repositoryElementAttribute.SaveChanges();
                //caso uma das imagens seja a default entao colocar uma outra qq
                if (images != null)
                {
                    if (images.Contains(element.DefaultPicturePath))
                    {
                        var allImages = this.ListImages(idElement);
                        if (allImages != null && allImages.Count > 0)
                            element.DefaultPicturePath = allImages.First().Value;
                        else
                            element.DefaultPicturePath = "/empty-property.jpg";

                        elementRepository.Update(element);
                        elementRepository.SaveChanges();
                    }
                }
            }

            return images;
        }

        public string SetDefaultImage(long idElement, long idElementAttribute)
        {
            using (var context = new ImobeNetContext())
            {
                var repositoryElement = new BaseRepository<Element>(context);
                var repositoryImage = new BaseRepository<ElementAttribute>(context);
                var image = repositoryImage.FirstOrDefault(p => p.IDElementAttribute == idElementAttribute);
                var element = repositoryElement.First(p => p.IDElement == idElement);
                element.DefaultPicturePath = image.Value;
                this.Update(element);

                return image.Value;
            }
        }

        public IList<Model.ElementAttribute> ListImages(long idElement)
        {
            IList<Model.ElementAttribute> result = null;
            using (var context = new ImobeNetContext())
            {
                var attributeRepository = new BaseRepository<Model.Attribute>(context);
                var attributeElementRepository = new BaseRepository<Model.ElementAttribute>(context);
                var attr = attributeRepository.FirstOrDefault(p => p.Group == "IMG");
                if (attr != null)
                    result = attributeElementRepository
                        .Find(p => p.IDAttribute == attr.IDAttribute && p.IDElement == idElement)
                        .ToList();

                return result;
            }
        }

        public long GetTotalImages(long idElement)
        {
            long count = 0;
            using (var context = new ImobeNetContext())
            {
                var attributeRepository = new BaseRepository<Model.Attribute>(context);
                var attributeElementRepository = new BaseRepository<Model.ElementAttribute>(context);
                var attr = attributeRepository.FirstOrDefault(p => p.Group == "IMG");
                if (attr != null)
                    count = attributeElementRepository.Count(p => p.IDAttribute == attr.IDAttribute && p.IDElement == idElement);

                return count;
            }
        }

        public void Update(Element entity, ICollection<ElementCulture> elementCulture, ICollection<ElementAttribute> elementAttribute, IList<string> elementImages)
        {
            this.Update(entity, elementCulture, elementAttribute);
            if (elementImages != null)
            {
                this.DeleteImage(entity.IDElement.Value);
                foreach (var item in elementImages)
                    this.InsertImage(entity.IDElement.Value, item);
            }
        }

        public void Insert(Element entity, ICollection<ElementCulture> elementCulture, ICollection<ElementAttribute> elementAttribute, IList<string> elementImages)
        {
            this.Insert(entity, elementCulture, elementAttribute);
            foreach (var item in elementImages)
                this.InsertImage(entity.IDElement.Value, item);
        }
    }
}