using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpongeSolutions.ServicoDireto.Services.StructureSystem.Contracts;
using SpongeSolutions.ServicoDireto.Database;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace SpongeSolutions.ServicoDireto.Services.StructureSystem.Implementation
{
    public class PurposeCultureService : IPurposeCultureContract
    {
        public void Insert(PurposeCulture entity)
        {
            using (var context = new ImobeNetContext())
            {
                var purposeCultureRepository = new BaseRepository<PurposeCulture>(context);
                purposeCultureRepository.Insert(entity);
                purposeCultureRepository.SaveChanges();
            }
        }

        public void Delete(PurposeCulture entity)
        {
            using (var context = new ImobeNetContext())
            {
                var purposeCultureRepository = new BaseRepository<PurposeCulture>(context);
                purposeCultureRepository.Delete(entity);
                purposeCultureRepository.SaveChanges();
            }
        }

        public void DeleteByPurpose(int idPurpose)
        {
            using (var context = new ImobeNetContext())
            {
                var purposes = (from p in context.PurposeCulture
                                where p.IDPurpose == idPurpose
                                select p);

                if (purposes != null && purposes.Count() > 0)
                {
                    var purposeCultureRepository = new BaseRepository<PurposeCulture>(context);
                    foreach (var purpose in purposes)
                        purposeCultureRepository.Delete(purpose);

                    purposeCultureRepository.SaveChanges();
                }
            }
        }

        public void Delete(int id)
        {
            using (var context = new ImobeNetContext())
            {
                var purposeCultureRepository = new BaseRepository<PurposeCulture>(context);
                purposeCultureRepository.Delete(purposeCultureRepository.GetById(id));
                purposeCultureRepository.SaveChanges();
            }
        }

        public void Update(PurposeCulture entity)
        {
            using (var context = new ImobeNetContext())
            {
                var purposeCultureRepository = new BaseRepository<PurposeCulture>(context);
                purposeCultureRepository.Update(entity);
                purposeCultureRepository.SaveChanges();
            }
        }

        public void Delete(int[] ids)
        {
            foreach (var id in ids)
                this.Delete(id);
        }

        public IList<PurposeCulture> GetAll()
        {
            StringBuilder sql = new StringBuilder();
            using (var context = new ImobeNetContext())
            {
                sql.AppendLine("exec pPurposeCulture_List");
                return context.Database.SqlQuery<PurposeCulture>(sql.ToString()).ToList();
            }
        }

        public PurposeCulture GetById(int id)
        {
            return this.GetAll().Where(p => p.IDPurpose == id).FirstOrDefault();
        }

        public PurposeCulture GetById(int idPurpose, string idCulture)
        {
            return this.GetAll().Where(p => p.IDPurpose == idPurpose && p.IDCulture == idCulture).FirstOrDefault();
        }
    }
}
