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
    public class AttributeTypeCultureService : IAttributeTypeCultureContract
    {
        public void Insert(AttributeTypeCulture entity)
        {
            using (var context = new ImobeNetContext())
            {
                var attributeTypeCultureRepository = new BaseRepository<AttributeTypeCulture>(context);
                attributeTypeCultureRepository.Insert(entity);
                attributeTypeCultureRepository.SaveChanges();
            }
        }

        public void Delete(AttributeTypeCulture entity)
        {
            using (var context = new ImobeNetContext())
            {
                var attributeTypeCultureRepository = new BaseRepository<AttributeTypeCulture>(context);
                attributeTypeCultureRepository.Delete(entity);
                attributeTypeCultureRepository.SaveChanges();
            }
        }

        public void DeleteByAttributeType(int idAttributeType)
        {
            using (var context = new ImobeNetContext())
            {
                var purposes = (from p in context.AttributeTypeCulture
                                where p.IDAttributeType == idAttributeType
                                select p);

                if (purposes != null && purposes.Count() > 0)
                {
                    var attributeTypeCultureRepository = new BaseRepository<AttributeTypeCulture>(context);
                    foreach (var purpose in purposes)
                        attributeTypeCultureRepository.Delete(purpose);

                    attributeTypeCultureRepository.SaveChanges();
                }
            }
        }

        public void Delete(int id)
        {
            using (var context = new ImobeNetContext())
            {
                var attributeTypeCultureRepository = new BaseRepository<AttributeTypeCulture>(context);
                attributeTypeCultureRepository.Delete(attributeTypeCultureRepository.GetById(id));
                attributeTypeCultureRepository.SaveChanges();
            }
        }

        public void Update(AttributeTypeCulture entity)
        {
            using (var context = new ImobeNetContext())
            {
                var attributeTypeCultureRepository = new BaseRepository<AttributeTypeCulture>(context);
                attributeTypeCultureRepository.Update(entity);
                attributeTypeCultureRepository.SaveChanges();
            }
        }

        public void Delete(int[] ids)
        {
            foreach (var id in ids)
                this.Delete(id);
        }

        public IList<AttributeTypeCultureExtended> GetAll(int? idAttributeType, string idCulture)
        {
            StringBuilder sql = new StringBuilder();
            object[] parameters = new object[]
            {
                new SqlParameter("IDCulture", idCulture),
                new SqlParameter("IDAttributeType", idAttributeType)
            };

            using (var context = new ImobeNetContext())
            {
                sql.AppendLine("exec pAttributeTypeCulture_List @IDAttributeType, @IDCulture");
                return context.Database.SqlQuery<AttributeTypeCultureExtended>(sql.ToString(), parameters).ToList();
            }
        }

        public IList<AttributeTypeCulture> GetAll()
        {
            return (IList<AttributeTypeCulture>)this.GetAll(null, null);

            //using (var context = new ImobeNetContext())
            //{
            //    return (from pc in context.AttributeTypeCulture.ToList()
            //            join c in context.Culture on pc.IDCulture equals c.IDCulture
            //            join i in context.Icon on c.IDIcon equals i.IDIcon
            //            select new AttributeTypeCulture
            //            {
            //                IDCulture = pc.IDCulture,
            //                IDAttributeType = pc.IDAttributeType,
            //                Description = pc.Description,
            //                CultureName = c.Name,
            //                IconPath = i.Path,
            //            }).ToList();
            //}
        }

        public AttributeTypeCulture GetById(int id)
        {
            return this.GetAll(id, null).FirstOrDefault();
        }

        public AttributeTypeCulture GetById(int idAttributeType, string idCulture)
        {
            return this.GetAll(idAttributeType, idCulture).FirstOrDefault();
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

        public IList<AttributeTypeCulture> GetAll(string sortType = null, int startRowIndex = -1, int maximumRows = -1)
        {
            throw new NotImplementedException();
        }


        public IList<AttributeTypeCulture> GetAll(out int recordCount, string sortType = null, int startRowIndex = -1, int maximumRows = -1)
        {
            throw new NotImplementedException();
        }
    }
}
