using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using SpongeSolutions.ServicoDireto.Database;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Services.StructureSystem.Contracts;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Transactions;
namespace SpongeSolutions.ServicoDireto.Services.StructureSystem.Implementation
{
    public class FilterService : IFilterContract
    {
        private void InsertUpdate(Model.Filter entity, int[] vinculatedPurpose, Enums.ActionType actionType)
        {
            using (var context = new ImobeNetContext())
            {
                using (TransactionScope scopeOfTransaction = new TransactionScope())
                {
                    try
                    {
                        var filterRepository = new BaseRepository<Model.Filter>(context);
                        var filterPurposeRepository = new BaseRepository<Model.FilterPurpose>(context);

                        if (actionType == Enums.ActionType.Insert)
                            filterRepository.Insert(entity);
                        else
                            if (actionType == Enums.ActionType.Update)
                                filterRepository.Update(entity);

                        context.SaveChanges();

                        var vincultadePurposes = (from p in context.FilterPurpose
                                                  where p.IDFilter == entity.IDFilter
                                                  select p);

                        //Deletando os propositos
                        if (vincultadePurposes != null && vincultadePurposes.Count() > 0)
                        {
                            foreach (var purpose in vincultadePurposes)
                                filterPurposeRepository.Delete(purpose);

                            context.SaveChanges();
                        }

                        // inserindo novos
                        foreach (var purpose in vinculatedPurpose)
                            filterPurposeRepository.Insert(new FilterPurpose() { IDFilter = entity.IDFilter.Value, IDPurpose = purpose });

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

        public void Insert(Model.Filter entity, int[] vinculatedPurpose)
        {
            this.InsertUpdate(entity, vinculatedPurpose, Enums.ActionType.Insert);
        }

        public void Update(Model.Filter entity, int[] vinculatedPurpose)
        {
            this.InsertUpdate(entity, vinculatedPurpose, Enums.ActionType.Update);
        }

        public void Insert(Model.Filter entity)
        {
            using (var context = new ImobeNetContext())
            {
                var filterRepository = new BaseRepository<Model.Filter>(context);
                filterRepository.Insert(entity);
                filterRepository.SaveChanges();
            }
        }

        public void Delete(Model.Filter entity)
        {
            using (var context = new ImobeNetContext())
            {
                var filterRepository = new BaseRepository<Model.Filter>(context);
                filterRepository.Delete(entity);
                filterRepository.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var context = new ImobeNetContext())
            {
                var filterRepository = new BaseRepository<Model.Filter>(context);
                filterRepository.Delete(filterRepository.GetById(id));
                filterRepository.SaveChanges();
            }
        }

        public void Update(Model.Filter entity)
        {
            using (var context = new ImobeNetContext())
            {
                var filterRepository = new BaseRepository<Model.Filter>(context);
                filterRepository.Update(entity);
                filterRepository.SaveChanges();
            }
        }

        public void Delete(int[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var filterRepository = new BaseRepository<Model.Filter>(context);
                var items = filterRepository.Fetch().Where(p => ids.Contains(p.IDFilter.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                        filterRepository.Delete(item);

                    filterRepository.SaveChanges();
                }
            }
        }

        public void Inactivate(int[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var filterRepository = new BaseRepository<Model.Filter>(context);
                var items = filterRepository.Fetch().Where(p => ids.Contains(p.IDFilter.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                    {
                        item.Status = (short)Enums.StatusType.Inactive;
                        item.ModifiedBy = ServiceContext.ActiveUserName;
                        filterRepository.Update(item);
                    }

                    filterRepository.SaveChanges();
                }
            }
        }

        public IList<Model.FilterExtended> GetAll(string idCulture)
        {
            StringBuilder sql = new StringBuilder();
            object[] parameters = new object[] { new SqlParameter("IDCulture", idCulture) };

            using (var context = new ImobeNetContext())
            {
                sql.AppendLine("exec pFilter_GetAll @IDCulture");
                return context.Database.SqlQuery<Model.FilterExtended>(sql.ToString(), parameters).ToList();
            }
        }

        public Model.FilterExtended GetById(string idCulture, int id)
        {
            return this.GetAll(idCulture).Where(p => p.IDFilter == id).FirstOrDefault();
        }

        public IList<Model.FilterExtended> GetByStatus(string idCulture, short status)
        {
            return this.GetAll(idCulture).Where(p => p.Status == status).ToList();
        }

        public IList<Model.FilterExtended> GetAllActive(string idCulture)
        {
            return this.GetByStatus(idCulture, (short)Enums.StatusType.Active).ToList();
        }

        Model.Filter IBaseService<Model.Filter, int>.GetById(int id)
        {
            throw new NotImplementedException();
        }

        IList<Model.Filter> IBaseService<Model.Filter, int>.GetAll()
        {
            throw new NotImplementedException();
        }
    }    
}
