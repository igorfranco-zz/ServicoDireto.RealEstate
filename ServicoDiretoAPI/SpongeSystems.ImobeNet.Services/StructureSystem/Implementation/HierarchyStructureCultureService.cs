using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpongeSolutions.ServicoDireto.Services.StructureSystem.Contracts;
using SpongeSolutions.ServicoDireto.Database;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using System.Data.SqlClient;

namespace SpongeSolutions.ServicoDireto.Services.StructureSystem.Implementation
{
    public class HierarchyStructureCultureService : IHierarchyStructureCultureContract
    {
        public void Insert(HierarchyStructureCulture entity)
        {
            using (var context = new ImobeNetContext())
            {
                var hierarchyStructureCultureRepository = new BaseRepository<HierarchyStructureCulture>(context);
                hierarchyStructureCultureRepository.Insert(entity);
                hierarchyStructureCultureRepository.SaveChanges();
            }
        }

        public void Delete(HierarchyStructureCulture entity)
        {
            using (var context = new ImobeNetContext())
            {
                var hierarchyStructureCultureRepository = new BaseRepository<HierarchyStructureCulture>(context);
                hierarchyStructureCultureRepository.Delete(entity);
                hierarchyStructureCultureRepository.SaveChanges();
            }
        }

        public void DeleteByHierarchyStructure(int idHierarchyStructure)
        {
            using (var context = new ImobeNetContext())
            {
                var purposes = (from p in context.HierarchyStructureCulture
                                where p.IDHierarchyStructure == idHierarchyStructure
                                select p);

                if (purposes != null && purposes.Count() > 0)
                {
                    var hierarchyStructureCultureRepository = new BaseRepository<HierarchyStructureCulture>(context);
                    foreach (var purpose in purposes)
                        hierarchyStructureCultureRepository.Delete(purpose);

                    hierarchyStructureCultureRepository.SaveChanges();
                }
            }
        }

        public void Delete(int id)
        {
            using (var context = new ImobeNetContext())
            {
                var hierarchyStructureCultureRepository = new BaseRepository<HierarchyStructureCulture>(context);
                hierarchyStructureCultureRepository.Delete(hierarchyStructureCultureRepository.GetById(id));
                hierarchyStructureCultureRepository.SaveChanges();
            }
        }

        public void Update(HierarchyStructureCulture entity)
        {
            using (var context = new ImobeNetContext())
            {
                var hierarchyStructureCultureRepository = new BaseRepository<HierarchyStructureCulture>(context);
                hierarchyStructureCultureRepository.Update(entity);
                hierarchyStructureCultureRepository.SaveChanges();
            }
        }

        public void Delete(int[] ids)
        {
            foreach (var id in ids)
                this.Delete(id);
        }

        public IList<HierarchyStructureCulture> GetAll(int? idHierarchyStructure, string idCulture)
        {
            using (var context = new ImobeNetContext())
            {
                StringBuilder sql = new StringBuilder();
                object[] parameters = new object[] { new SqlParameter("IDHierarchyStructure", idHierarchyStructure) ,
                                                     new SqlParameter("IDCulture", idCulture)};

                sql.AppendLine("exec pHierarchyStructureCulture_List @IDCulture, @IDHierarchyStructure");
                return context.Database.SqlQuery<Model.HierarchyStructureCulture>(sql.ToString(), parameters).ToList();
                //return (from pc in context.HierarchyStructureCulture.ToList()
                //        join c in context.Culture on pc.IDCulture equals c.IDCulture
                //        join i in context.Icon on c.IDIcon equals i.IDIcon
                //        select new HierarchyStructureCulture
                //        {
                //            IDCulture = pc.IDCulture,
                //            IDHierarchyStructure = pc.IDHierarchyStructure,
                //            Description = pc.Description,
                //            CultureName = c.Name,
                //            IconPath = i.Path,
                //        }).ToList();
            }
        }

        public HierarchyStructureCulture GetById(int id)
        {
            return null;
        }

        public HierarchyStructureCulture GetById(int idHierarchyStructure, string idCulture)
        {
            return this.GetAll(idHierarchyStructure, idCulture).FirstOrDefault();
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


        public IList<HierarchyStructureCulture> GetAll(out int recordCount, string sortType = null, int startRowIndex = -1, int maximumRows = -1)
        {
            throw new NotImplementedException();
        }

        public IList<HierarchyStructureCulture> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
