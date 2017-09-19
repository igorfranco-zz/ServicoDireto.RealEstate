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
    public class CountryService : ICountryContract
    {
        public void Insert(Country entity)
        {
            using (var context = new ImobeNetContext())
            {
                var countryRepository = new BaseRepository<Country>(context);
                countryRepository.Insert(entity);
                countryRepository.SaveChanges();
            }
        }

        public void Delete(Country entity)
        {
            using (var context = new ImobeNetContext())
            {
                var countryRepository = new BaseRepository<Country>(context);
                countryRepository.Delete(entity);
                countryRepository.SaveChanges();
            }
        }

        public void Delete(string id)
        {
            using (var context = new ImobeNetContext())
            {
                var countryRepository = new BaseRepository<Country, string>(context);
                countryRepository.Delete(countryRepository.GetById(id));
                countryRepository.SaveChanges();
            }
        }

        public void Update(Country entity)
        {
            using (var context = new ImobeNetContext())
            {
                var countryRepository = new BaseRepository<Country>(context);
                countryRepository.Update(entity);
                countryRepository.SaveChanges();
            }
        }

        public void Delete(string[] ids)
        {
            foreach (var item in ids)
                this.Delete(item);
        }

        public void Inactivate(string[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var countryRepository = new BaseRepository<Country, string>(context);
                var items = countryRepository.Fetch().Where(p => ids.Contains(p.IDCountry));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                    {
                        item.Status = (short)Enums.StatusType.Inactive;
                        item.ModifiedBy = ServiceContext.ActiveUserName;
                        countryRepository.Update(item);
                    }

                    countryRepository.SaveChanges();
                }
            }
        }

        public IList<Country> GetAll()
        {
            using (var context = new ImobeNetContext())
            {
                var countryRepository = new BaseRepository<Country>(context);
                return countryRepository.GetAll().OrderBy(p => p.Name).ToList();
            }
        }

        public Country GetById(string id)
        {
            using (var context = new ImobeNetContext())
            {
                var countryRepository = new BaseRepository<Country, string>(context);
                return countryRepository.GetById(id);
            }
        }

        public IList<Country> GetByStatus(short status)
        {
            using (var context = new ImobeNetContext())
            {
                var countryRepository = new BaseRepository<Country>(context);
                return countryRepository.GetAll().Where(p => p.Status == status).ToList();
            }
        }

        public IList<Country> GetAllActive()
        {
            return this.GetByStatus((short)Enums.StatusType.Active).ToList();
        }

        public SelectList GetAllActive(string selectedCountry = null)
        {
            return new SelectList(this.GetAllActive(), "IDCountry", "Name", selectedCountry);
        }

        public bool Exists(string id)
        {
            return this.GetById(id) != null;
        }

        public IList<Country> GetAll(out int recordCount, string sortType = null, int startRowIndex = -1, int maximumRows = -1)
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
                sql.AppendLine("exec pCountry_List @StartRowIndex, @MaximumRows, @RecordCount out");
                var result = context.Database.SqlQuery<Country>(sql.ToString(), parameters.ToArray()).ToList();
                recordCount = Convert.ToInt32(recordCountParameter.Value);
                return result;
            }
        }

    }
}
