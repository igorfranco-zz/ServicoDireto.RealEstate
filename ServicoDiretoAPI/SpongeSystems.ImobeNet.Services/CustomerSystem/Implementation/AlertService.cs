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
using SpongeSolutions.ServicoDireto.Services.CustomerSystem.Contracts;
using System.Data.SqlClient;
using System.Transactions;

namespace SpongeSolutions.ServicoDireto.Services.CustomerSystem.Implementation
{
    public class AlertService : IAlertContract
    {
        public void InsertUpdate(Alert entity, List<AlertAttribute> attributes, Enums.ActionType actionType)
        {
            using (var context = new ImobeNetContext())
            {
                using (TransactionScope scopeOfTransaction = new TransactionScope())
                {
                    try
                    {
                        var alertRepository = new BaseRepository<Alert>(context);
                        var alertAttributeRepository = new BaseRepository<AlertAttribute>(context);
                        if (!string.IsNullOrEmpty(entity.Address))
                            this.SetAlertPosition(entity);

                        if (actionType == Enums.ActionType.Insert)
                        {
                            alertRepository.Insert(entity);
                            context.SaveChanges();
                        }
                        else if (actionType == Enums.ActionType.Update)
                        {
                            alertRepository.Update(entity);
                            context.SaveChanges();

                            //Atributos
                            var vinculatedAttributes = (from oc in context.AlertAttribute
                                                        where oc.IDAlert == entity.IDAlert.Value
                                                        select oc);

                            if (vinculatedAttributes != null && vinculatedAttributes.Count() > 0)
                            {
                                foreach (var item in vinculatedAttributes)
                                    alertAttributeRepository.Delete(item);

                                context.SaveChanges();
                            }
                        }

                        // Inserindo Atributos
                        foreach (var item in attributes)
                        {
                            item.IDAlert = entity.IDAlert.Value;
                            alertAttributeRepository.Insert(item);
                        }
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

        public void Insert(Alert entity, List<AlertAttribute> attributes)
        {
            this.InsertUpdate(entity, attributes, Enums.ActionType.Insert);
        }

        public void Update(Alert entity, List<AlertAttribute> attributes)
        {
            this.InsertUpdate(entity, attributes, Enums.ActionType.Update);
        }

        public void Insert(Alert entity)
        {
            using (var context = new ImobeNetContext())
            {
                var AlertRepository = new BaseRepository<Alert>(context);
                AlertRepository.Insert(entity);
                AlertRepository.SaveChanges();
            }
        }

        public void Delete(Alert entity)
        {
            using (var context = new ImobeNetContext())
            {
                var AlertRepository = new BaseRepository<Alert>(context);
                AlertRepository.Delete(entity);
                AlertRepository.SaveChanges();
            }
        }

        public void Delete(long[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var AlertRepository = new BaseRepository<Alert>(context);
                var items = AlertRepository.Fetch().Where(p => ids.Contains(p.IDAlert.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                        AlertRepository.Delete(item);

                    AlertRepository.SaveChanges();
                }
            }
        }

        public void Deactivate(long[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var AlertRepository = new BaseRepository<Alert>(context);
                var items = AlertRepository.Fetch().Where(p => ids.Contains(p.IDAlert.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                    {
                        item.Status = (short)Enums.StatusType.Inactive;
                        item.ModifiedBy = ServiceContext.ActiveUserName;
                        item.ModifyDate = DateTime.Now;
                        this.Update(item);
                    }
                }
            }
        }

        public void Update(Alert entity)
        {
            using (var context = new ImobeNetContext())
            {
                var AlertRepository = new BaseRepository<Alert>(context);
                AlertRepository.Update(entity);
                AlertRepository.SaveChanges();
            }
        }

        public IList<Alert> GetAll()
        {
            using (var context = new ImobeNetContext())
            {
                return (from Alert in context.Alert.ToList()
                        orderby Alert.CreateDate
                        select Alert).ToList();
            }
        }

        public IList<Alert> GetByStatus(short status)
        {
            return this.GetAll().Where(p => p.Status == status).ToList();
        }

        public IList<Alert> GetAllActive()
        {
            return this.GetByStatus((short)Enums.StatusType.Active).ToList();
        }

        public void Delete(long id)
        {
            using (var context = new ImobeNetContext())
            {
                var AlertRepository = new BaseRepository<Alert>(context);
                AlertRepository.Delete(this.GetById(id));
                AlertRepository.SaveChanges();
            }
        }

        public Alert GetById(long id)
        {
            return this.GetAll().Where(p => p.IDAlert == id).FirstOrDefault();
        }        

        public IList<AlertExtended> List(string idCulture, int idCustomer = 0, long idAlert = 0)
        {
            using (var context = new ImobeNetContext())
            {
                StringBuilder sql = new StringBuilder();
                object[] parameters = new object[] {
                                                     new SqlParameter("IDCustomer", idCustomer) ,   
                                                     new SqlParameter("IDAlert", idAlert) ,
                                                     new SqlParameter("IDCulture", idCulture)};

                sql.AppendLine("exec pAlert_List @IDCulture, @IDCustomer, @IDAlert");
                return context.Database.SqlQuery<Model.AlertExtended>(sql.ToString(), parameters).ToList();
            }
        }

        public IList<AlertAttribute> ListAttribute(long idAlert) 
        {
            using (var context = new ImobeNetContext())
            {
                return (from alert in context.AlertAttribute.ToList()
                        where alert.IDAlert == idAlert
                        select alert).ToList();
            }
        }

        private void SetAlertPosition(Alert entity)
        {
            //var city = ServiceContext.CityService.GetById(entity.IDCity.Value);

            ////Buscando informações de Geolocalização;
            //string address = String.Format("{0},{1},{2},{3}",
            //        city.CountryName,
            //        city.Name,
            //        city.StateName,
            //        entity.Address);
            //try
            //{
            //    var position = ServiceContext.PositioningService.GetGeographicPosition(address);
            //    if (position != null)
            //    {
            //        entity.Latitude = position.Latitude;
            //        entity.Longitude = position.Longitude;
            //    }
            //}
            //catch
            //{
            //    entity.Latitude = null;
            //    entity.Longitude = null;
            //}
        }

        public AlertExtended GetByIdExt(string idCulture, int idAlert)
        {
            return this.List(idCulture, idAlert: idAlert).FirstOrDefault();
        }
    }
}