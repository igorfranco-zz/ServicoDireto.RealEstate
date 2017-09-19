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
    public class AttributeCultureService : IAttributeCultureContract
    {
        public void Insert(AttributeCulture entity)
        {
            using (var context = new ImobeNetContext())
            {
                var attributeCultureRepository = new BaseRepository<AttributeCulture>(context);
                attributeCultureRepository.Insert(entity);
                attributeCultureRepository.SaveChanges();
            }
        }

        public void Delete(AttributeCulture entity)
        {
            using (var context = new ImobeNetContext())
            {
                var attributeCultureRepository = new BaseRepository<AttributeCulture>(context);
                attributeCultureRepository.Delete(entity);
                attributeCultureRepository.SaveChanges();
            }
        }

        public void DeleteByAttribute(int idAttribute)
        {
            using (var context = new ImobeNetContext())
            {
                var cultures = (from p in context.AttributeCulture
                                where p.IDAttribute == idAttribute
                                select p);

                if (cultures != null && cultures.Count() > 0)
                {
                    var attributeCultureRepository = new BaseRepository<AttributeCulture>(context);
                    foreach (var item in cultures)
                        attributeCultureRepository.Delete(item);

                    attributeCultureRepository.SaveChanges();
                }
            }
        }

        public void Delete(int id)
        {
            using (var context = new ImobeNetContext())
            {
                var attributeCultureRepository = new BaseRepository<AttributeCulture>(context);
                attributeCultureRepository.Delete(attributeCultureRepository.GetById(id));
                attributeCultureRepository.SaveChanges();
            }
        }

        public void Update(AttributeCulture entity)
        {
            using (var context = new ImobeNetContext())
            {
                var attributeCultureRepository = new BaseRepository<AttributeCulture>(context);
                attributeCultureRepository.Update(entity);
                attributeCultureRepository.SaveChanges();
            }
        }

        public void Delete(int[] ids)
        {
            foreach (var id in ids)
                this.Delete(id);
        }

        public IList<AttributeCulture> GetAll()
        {
            return (IList<AttributeCulture>)GetAll(null, null);
        }

        public IList<AttributeCultureExtended> GetAll(int? idAttribute, string idCulture)
        {
            StringBuilder sql = new StringBuilder();
            object[] parameters = new object[]
            {
                new SqlParameter("IDCulture", idCulture),
                new SqlParameter("IDAttribute", idAttribute)
            };

            using (var context = new ImobeNetContext())
            {
                sql.AppendLine("exec pAttributeCulture_List @IDAttribute, @IDCulture");
                return context.Database.SqlQuery<AttributeCultureExtended>(sql.ToString(), parameters).ToList();
            }

            //using (var context = new ImobeNetContext())
            //{
            //    return (from pc in context.AttributeCulture//.ToList()
            //            join c in context.Culture on pc.IDCulture equals c.IDCulture
            //            join i in context.Icon on c.IDIcon equals i.IDIcon
            //            select new AttributeCulture
            //            {
            //                IDCulture = pc.IDCulture,
            //                IDAttribute = pc.IDAttribute,
            //                Name = pc.Name,
            //                Value = pc.Value,
            //                CultureName = c.Name,
            //                IconPath = i.Path,
            //            }).ToList();
            //}
        }

        public AttributeCulture GetById(int id)
        {
            return this.GetAll(id, null).FirstOrDefault();
        }

        public AttributeCulture GetById(int idAttribute, string idCulture)
        {
            return this.GetAll(idAttribute, idCulture).FirstOrDefault();
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

        public IList<AttributeCulture> GetAll(string sortType = null, int startRowIndex = -1, int maximumRows = -1)
        {
            throw new NotImplementedException();
        }


        public IList<AttributeCulture> GetAll(out int recordCount, string sortType = null, int startRowIndex = -1, int maximumRows = -1)
        {
            throw new NotImplementedException();
        }
    }
}
