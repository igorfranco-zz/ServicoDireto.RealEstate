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
    public class CityService : ICityContract
    {
        public void Insert(City entity)
        {
            using (var context = new ImobeNetContext())
            {
                var cityRepository = new BaseRepository<City>(context);
                cityRepository.Insert(entity);
                cityRepository.SaveChanges();
            }
        }

        public void Delete(City entity)
        {
            using (var context = new ImobeNetContext())
            {
                var cityRepository = new BaseRepository<City>(context);
                cityRepository.Delete(entity);
                cityRepository.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var context = new ImobeNetContext())
            {
                var cityRepository = new BaseRepository<City>(context);
                cityRepository.Delete(cityRepository.GetById(id));
                cityRepository.SaveChanges();
            }
        }

        public void Update(City entity)
        {
            using (var context = new ImobeNetContext())
            {
                var cityRepository = new BaseRepository<City>(context);
                cityRepository.Update(entity);
                cityRepository.SaveChanges();
            }
        }

        public void Delete(int[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var cityRepository = new BaseRepository<City>(context);
                var items = cityRepository.Fetch().Where(p => ids.Contains(p.IDCity.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                        cityRepository.Delete(item);

                    cityRepository.SaveChanges();
                }
            }
        }

        public void Inactivate(int[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var cityRepository = new BaseRepository<City>(context);
                var items = cityRepository.Fetch().Where(p => ids.Contains(p.IDCity.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                    {
                        item.Status = (short)Enums.StatusType.Inactive;
                        item.ModifiedBy = ServiceContext.ActiveUserName;
                        cityRepository.Update(item);
                    }

                    cityRepository.SaveChanges();
                }
            }
        }

        public IList<City> GetAll()
        {
            using (var context = new ImobeNetContext())
            {
                return (from city in context.City.ToList()
                        join state in context.StateProvince on city.IDStateProvince equals state.IDStateProvince
                        join country in context.Country on state.IDCountry equals country.IDCountry
                        orderby city.Name
                        select new City
                        {
                            Name = city.Name,
                            //IDCountry = country.IDCountry,
                            //CountryName = country.Name,
                            //StateName = state.Name,
                            IDStateProvince = city.IDStateProvince,
                            IDCity = city.IDCity,
                            CreateDate = city.CreateDate,
                            CreatedBy = city.CreatedBy,
                            ModifiedBy = city.ModifiedBy,
                            ModifyDate = city.ModifyDate,
                            Status = city.Status,
                        }).ToList();
            }
        }

        public City GetById(int id)
        {
            return this.GetAll().Where(p => p.IDCity == id).FirstOrDefault();
        }

        public CityExtended Get(int id)
        {
            int count;
            return this.GetAll(out count).Where(p => p.IDCity == id).FirstOrDefault();
        }

        public IList<CityExtended> GetByStatus(short status)
        {
            int count;
            return this.GetAll(out count);
        }

        public IList<CityExtended> GetAllActive()
        {
            return this.GetByStatus((short)Enums.StatusType.Active).ToList();
        }

        public IList<CityExtended> GetAllActive(string idCountry, int idStateProvince)
        {
            return this.GetAllActive().Where(p => p.IDCountry == idCountry && p.IDStateProvince == idStateProvince).ToList();
        }

        public SelectList GetAllActiveAsSelectList(string idCountry, int idStateProvince)
        {
            return new SelectList(this.GetAllActive(idCountry, idStateProvince), "IDCity", "Name");
        }

        public City GetInsert(string cityName, int idStateProvince)
        {
            using (var context = new ImobeNetContext())
            {
                var city = (from c in context.City
                            where c.Name.ToUpper() == cityName.ToUpper()
                            && c.IDStateProvince == idStateProvince
                            select c).FirstOrDefault();

                if (city == null)
                {
                    city = new City()
                    {
                        CreateDate = DateTime.Now,
                        ModifyDate = DateTime.Now,
                        ModifiedBy = "AUTO",
                        CreatedBy = "AUTO",
                        IDStateProvince = idStateProvince,
                        Name = cityName,
                        Status = (short)Enums.StatusType.Active
                    };

                    this.Insert(city);
                }

                return city;
            }
        }

        public IList<CityExtended> GetAll(out int recordCount, string sortType = null, int startRowIndex = -1, int maximumRows = -1)
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
                sql.AppendLine("exec pCity_List @StartRowIndex, @MaximumRows, @RecordCount out");
                var result = context.Database.SqlQuery<CityExtended>(sql.ToString(), parameters.ToArray()).ToList();
                recordCount = Convert.ToInt32(recordCountParameter.Value);
                return result;
            }
        }

    }
}
