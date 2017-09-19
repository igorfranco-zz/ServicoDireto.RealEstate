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

namespace SpongeSolutions.ServicoDireto.Services.StructureSystem.Implementation
{
    public class StateProvinceService : IStateProvinceContract
    {
        public void Insert(StateProvince entity)
        {
            using (var context = new ImobeNetContext())
            {
                var stateProvinceRepository = new BaseRepository<StateProvince>(context);
                stateProvinceRepository.Insert(entity);
                stateProvinceRepository.SaveChanges();
            }
        }

        public void Delete(StateProvince entity)
        {
            using (var context = new ImobeNetContext())
            {
                var stateProvinceRepository = new BaseRepository<StateProvince>(context);
                stateProvinceRepository.Delete(entity);
                stateProvinceRepository.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var context = new ImobeNetContext())
            {
                var stateProvinceRepository = new BaseRepository<StateProvince>(context);
                stateProvinceRepository.Delete(stateProvinceRepository.GetById(id));
                stateProvinceRepository.SaveChanges();
            }
        }

        public void Update(StateProvince entity)
        {
            using (var context = new ImobeNetContext())
            {
                var stateProvinceRepository = new BaseRepository<StateProvince>(context);
                stateProvinceRepository.Update(entity);
                stateProvinceRepository.SaveChanges();
            }
        }

        public void Delete(int[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var stateProvinceRepository = new BaseRepository<StateProvince>(context);
                var items = stateProvinceRepository.Fetch().Where(p => ids.Contains(p.IDStateProvince.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                        stateProvinceRepository.Delete(item);

                    stateProvinceRepository.SaveChanges();
                }
            }
        }

        public void Inactivate(int[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var stateProvinceRepository = new BaseRepository<StateProvince>(context);
                var items = stateProvinceRepository.Fetch().Where(p => ids.Contains(p.IDStateProvince.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                    {
                        item.Status = (short)Enums.StatusType.Inactive;
                        item.ModifiedBy = ServiceContext.ActiveUserName;
                        stateProvinceRepository.Update(item);
                    }

                    stateProvinceRepository.SaveChanges();
                }
            }
        }

        public StateProvince GetById(int id)
        {
            return this.GetAll().Where(p => p.IDStateProvince == id).FirstOrDefault();
        }

        public IList<StateProvince> GetByStatus(short status)
        {
            return this.GetAll().Where(p => p.Status == status).ToList();
        }

        public IList<StateProvince> GetAllActive()
        {
            return this.GetByStatus((short)Enums.StatusType.Active).ToList();
        }

        public IList<StateProvince> GetAllActive(string idCountry)
        {
            return this.GetAllActive().Where(p => p.IDCountry == idCountry).ToList();
        }

        public SelectList GetAllActiveAsSelectList(string idCountry)
        {
            return new SelectList(this.GetAllActive(idCountry), "IDStateProvince", "Name");
        }

        public IList<StateProvince> GetAll()
        {
            using (var context = new ImobeNetContext())
            {
                return (from state in context.StateProvince.ToList()
                        join country in context.Country on state.IDCountry equals country.IDCountry
                        orderby state.Name
                        select new StateProvince
                        {
                            Name = state.Name,
                            IDCountry = state.IDCountry,
                            IDStateProvince = state.IDStateProvince,
                            Acronym = state.Acronym,
                            CreateDate = state.CreateDate,
                            CreatedBy = state.CreatedBy,
                            ModifiedBy = state.ModifiedBy,
                            ModifyDate = state.ModifyDate,
                            Status = state.Status,
                        }).ToList();
            }
        }

        public int RowCount
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IList<StateProvince> GetAll(string sortType = null, int startRowIndex = -1, int maximumRows = -1)
        {
            throw new NotImplementedException();
        }

        public IList<StateProvinceExtended> GetAll(out int recordCount, string sortType = null, int startRowIndex = -1, int maximumRows = -1)
        {
            recordCount = 0;
            SqlParameter recordCountParameter = new SqlParameter("RecordCount", 0);
            recordCountParameter.Direction = System.Data.ParameterDirection.Output;

            StringBuilder sql = new StringBuilder();
            List<object> parameters = new List<object>();
            parameters.Add(new SqlParameter("StartRowIndex", startRowIndex));
            parameters.Add(new SqlParameter("MaximumRows", maximumRows));
            parameters.Add(recordCountParameter);

            using (var context = new ImobeNetContext())
            {
                sql.AppendLine("exec pStateProvince_List @StartRowIndex, @MaximumRows, @RecordCount out");
                var result = context.Database.SqlQuery<StateProvinceExtended>(sql.ToString(), parameters.ToArray()).ToList();
                recordCount = Convert.ToInt32(recordCountParameter.Value);
                return result;
            }
        }

        public StateProvince GetInsert(string stateName, string idCountry)
        {
            using (var context = new ImobeNetContext())
            {
                var state = (from c in context.StateProvince
                             where c.Acronym.ToUpper() == stateName.ToUpper().Trim()
                             && c.IDCountry == idCountry
                             select c).FirstOrDefault();

                if (state == null)
                {
                    state = new StateProvince()
                    {
                        Acronym = stateName,
                        CreateDate = DateTime.Now,
                        ModifyDate = DateTime.Now,
                        CreatedBy = "AUTO",
                        ModifiedBy = "AUTO",
                        IDCountry = idCountry,
                        Name = stateName,
                        Status = (short)Enums.StatusType.Active
                    };
                    this.Insert(state);
                }

                return state;
            }
        }
    }
}
