using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpongeSolutions.ServicoDireto.Services.StructureSystem.Contracts;
using SpongeSolutions.ServicoDireto.Database;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using SpongeSolutions.Core.LINQ;
using System.Data.SqlClient;

namespace SpongeSolutions.ServicoDireto.Services.StructureSystem.Implementation
{
    public class IconService : IIconContract
    {
        public void Insert(Icon entity)
        {
            using (var context = new ImobeNetContext())
            {
                var iconRepository = new BaseRepository<Icon>(context);
                iconRepository.Insert(entity);
                iconRepository.SaveChanges();
            }
        }

        public void Delete(Icon entity)
        {
            using (var context = new ImobeNetContext())
            {
                var iconRepository = new BaseRepository<Icon>(context);
                iconRepository.Delete(entity);
                iconRepository.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var context = new ImobeNetContext())
            {
                var iconRepository = new BaseRepository<Icon>(context);
                iconRepository.Delete(iconRepository.GetById(id));
                iconRepository.SaveChanges();
            }
        }

        public void Update(Icon entity)
        {
            using (var context = new ImobeNetContext())
            {
                var iconRepository = new BaseRepository<Icon>(context);
                iconRepository.Update(entity);
                iconRepository.SaveChanges();
            }
        }

        public void Delete(int[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var iconRepository = new BaseRepository<Icon>(context);
                var items = iconRepository.Fetch().Where(p => ids.Contains(p.IDIcon.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                        iconRepository.Delete(item);

                    iconRepository.SaveChanges();
                }
            }
        }

        public void Inactivate(int[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var iconRepository = new BaseRepository<Icon>(context);
                var items = iconRepository.Fetch().Where(p => ids.Contains(p.IDIcon.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                    {
                        item.Status = (short)Enums.StatusType.Inactive;
                        item.ModifiedBy = ServiceContext.ActiveUserName;
                        iconRepository.Update(item);
                    }

                    iconRepository.SaveChanges();
                }
            }
        }

        public Icon GetById(int id)
        {
            using (var context = new ImobeNetContext())
            {
                var iconRepository = new BaseRepository<Icon>(context);
                return iconRepository.GetById(id);
            }
        }        

        public IList<Icon> GetAllActive()
        {
            return this.GetByStatus((short)Enums.StatusType.Active).ToList();
        }

        //public IList<Icon> GetAll(out int recordCount, string sortType = null, int startRowIndex = -1, int maximumRows = -1)
        //{
        //    var result = this.GetAll();
        //    var query = result.AsQueryable();
        //    recordCount = result.Count();
        //    if (sortType != null && sortType.Length > 0)
        //        query = GenericSorter.GetListSort(query, sortType);

        //    if (startRowIndex > -1 && maximumRows > -1)
        //        query = query.Distinct().Skip(startRowIndex).Take(maximumRows);
        //    return query.ToList();
        //}

        public IList<Icon> GetAll(out int recordCount, string sortType = null, int startRowIndex = -1, int maximumRows = -1)
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
                sql.AppendLine("exec pIcon_List @StartRowIndex, @MaximumRows, @RecordCount out");
                var result = context.Database.SqlQuery<Icon>(sql.ToString(), parameters.ToArray()).ToList();
                recordCount = Convert.ToInt32(recordCountParameter.Value);
                return result;
            }
        }

        public IList<Icon> GetByStatus(short status)
        {
            return this.GetAll().Where(p => p.Status == status).ToList();
        }

        public IList<Icon> GetAll()
        {
            using (var context = new ImobeNetContext())
            {
                var iconRepository = new BaseRepository<Icon>(context);
                return iconRepository.GetAll().OrderBy(p => p.Name).ToList();
            }
        }

        public IEnumerable<dynamic> AutoComplete(string text)
        {
            using (var context = new ImobeNetContext())
            {
                return (from icon in context.Icon.ToList()
                        where icon.Status == (short)Enums.StatusType.Active && icon.Name.Contains(text)
                        orderby icon.Name
                        select new
                        {
                            id = icon.IDIcon,
                            value = icon.Name,
                            label = icon.Name
                        }).ToList();
            }
        }
    }
}
