using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using SpongeSolutions.ServicoDireto.Database;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Services.StructureSystem.Contracts;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using SpongeSolutions.Core.LINQ;
namespace SpongeSolutions.ServicoDireto.Services.StructureSystem.Implementation
{
    public class CultureService : ICultureContract
    {
        public void Insert(Culture entity)
        {
            using (var context = new ImobeNetContext())
            {
                var cultureRepository = new BaseRepository<Culture>(context);
                cultureRepository.Insert(entity);
                cultureRepository.SaveChanges();
            }
        }

        public void Delete(Culture entity)
        {
            using (var context = new ImobeNetContext())
            {
                var cultureRepository = new BaseRepository<Culture>(context);
                cultureRepository.Delete(entity);
                cultureRepository.SaveChanges();
            }
        }

        public void Delete(string id)
        {
            using (var context = new ImobeNetContext())
            {
                var cultureRepository = new BaseRepository<Culture, string>(context);
                cultureRepository.Delete(cultureRepository.GetById(id));
                cultureRepository.SaveChanges();
            }
        }

        public void Update(Culture entity)
        {
            using (var context = new ImobeNetContext())
            {
                var cultureRepository = new BaseRepository<Culture>(context);
                cultureRepository.Update(entity);
                cultureRepository.SaveChanges();
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
                var cultureRepository = new BaseRepository<Culture, string>(context);
                var items = cultureRepository.Fetch().Where(p => ids.Contains(p.IDCulture));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                    {
                        item.Status = (short)Enums.StatusType.Inactive;
                        item.ModifiedBy = ServiceContext.ActiveUserName;
                        cultureRepository.Update(item);
                    }

                    cultureRepository.SaveChanges();
                }
            }
        }

        public CultureExtended GetById(string id)
        {
            int recordCount;
            return this.GetAll(out recordCount).Where(p => p.IDCulture == id).FirstOrDefault();
        }

        public IList<CultureExtended> GetByStatus(short status, out int recordCount, string sortType = null, int startRowIndex = -1, int maximumRows = -1)
        {
            return this.GetAll(out recordCount, sortType, startRowIndex, maximumRows).Where(p => p.Status == status).ToList();
        }

        public bool Exists(string id)
        {
            return (this.GetById(id) != null);
        }

        public IList<CultureExtended> GetAll(out int recordCount, string sortType = null, int startRowIndex = -1, int maximumRows = -1)
        {
            using (var context = new ImobeNetContext())
            {
                var result = (from c in context.Culture
                              join i in context.Icon on c.IDIcon equals i.IDIcon
                              //where c.Status == (short)Enums.StatusType.Active
                              orderby c.Name
                              select new CultureExtended
                              {
                                  Name = c.Name,
                                  IDCulture = c.IDCulture,
                                  CreateDate = c.CreateDate,
                                  CreatedBy = c.CreatedBy,
                                  ModifiedBy = c.ModifiedBy,
                                  ModifyDate = c.ModifyDate,
                                  Status = c.Status,
                                  IDIcon = i.IDIcon.Value,
                                  IconPath = i.Path,
                                  IconName = i.Name,
                              }).ToList();

                var query = result.AsQueryable();
                recordCount = result.Count();
                if (sortType != null && sortType.Length > 0)
                    query = GenericSorter.GetListSort(query, sortType);

                if (startRowIndex > -1 && maximumRows > -1)
                    query = query.Distinct().Skip(startRowIndex).Take(maximumRows);
                return query.ToList();
            }
        }

        public IList<CultureExtended> GetAllActive(out int recordCount, string sortType = null, int startRowIndex = -1, int maximumRows = -1)
        {
            return this.GetByStatus((short)Enums.StatusType.Active, out recordCount, sortType, startRowIndex, maximumRows).ToList();
        }

        public IList<CultureExtended> GetAllActive()
        {
            int recordCount;
            return this.GetAllActive(out recordCount);
        }

        public IList<CultureExtended> GetAll()
        {
            using (var context = new ImobeNetContext())
            {
                return (from c in context.Culture.ToList()
                        join i in context.Icon on c.IDIcon equals i.IDIcon
                        orderby c.Name
                        select new CultureExtended
                        {
                            Name = c.Name,
                            IDCulture = c.IDCulture,
                            CreateDate = c.CreateDate,
                            CreatedBy = c.CreatedBy,
                            ModifiedBy = c.ModifiedBy,
                            ModifyDate = c.ModifyDate,
                            Status = c.Status,
                            IDIcon = i.IDIcon.Value,
                            IconPath = i.Path,
                            IconName = i.Name,
                        }).ToList();
            }
        }

        Culture IBaseService<Culture, string>.GetById(string id)
        {
            throw new NotImplementedException();
        }

        IList<Culture> IBaseService<Culture, string>.GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
