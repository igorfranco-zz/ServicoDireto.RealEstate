using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpongeSolutions.ServicoDireto.Services.StructureSystem.Contracts;
using SpongeSolutions.ServicoDireto.Database;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Services.ElementSystem.Contracts;
using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using System.Data.SqlClient;
namespace SpongeSolutions.ServicoDireto.Services.ElementSystem.Implementation
{
    public class ElementCultureService : IElementCultureContract
    {
        public void Insert(ElementCulture entity)
        {
            using (var context = new ImobeNetContext())
            {
                var elementCultureRepository = new BaseRepository<ElementCulture>(context);
                elementCultureRepository.Insert(entity);
                elementCultureRepository.SaveChanges();
            }
        }

        public void Delete(ElementCulture entity)
        {
            using (var context = new ImobeNetContext())
            {
                var elementCultureRepository = new BaseRepository<ElementCulture>(context);
                elementCultureRepository.Delete(entity);
                elementCultureRepository.SaveChanges();
            }
        }

        public void DeleteByElement(int idElement)
        {
            using (var context = new ImobeNetContext())
            {
                var cultures = (from p in context.ElementCulture
                                where p.IDElement == idElement
                                select p);

                if (cultures != null && cultures.Count() > 0)
                {
                    var elementCultureRepository = new BaseRepository<ElementCulture>(context);
                    foreach (var item in cultures)
                        elementCultureRepository.Delete(item);

                    elementCultureRepository.SaveChanges();
                }
            }
        }

        public void Delete(int id)
        {
            using (var context = new ImobeNetContext())
            {
                var elementCultureRepository = new BaseRepository<ElementCulture>(context);
                elementCultureRepository.Delete(elementCultureRepository.GetById(id));
                elementCultureRepository.SaveChanges();
            }
        }

        public void Update(ElementCulture entity)
        {
            using (var context = new ImobeNetContext())
            {
                var elementCultureRepository = new BaseRepository<ElementCulture>(context);
                elementCultureRepository.Update(entity);
                elementCultureRepository.SaveChanges();
            }
        }

        public void Delete(int[] ids)
        {
            foreach (var id in ids)
                this.Delete(id);
        }

        public IList<ElementCultureExtended> GetAll(out int recordCount, int idElement = -1, string idCulture = null, string sortType = null, int startRowIndex = -1, int maximumRows = -1)
        {
            using (var context = new ImobeNetContext())
            {
                recordCount = 0;
                SqlParameter recordCountParameter = new SqlParameter("RecordCount", 0);
                recordCountParameter.Direction = System.Data.ParameterDirection.Output;

                StringBuilder sql = new StringBuilder();
                object[] parameters = new object[] { new SqlParameter("IDElement", idElement) ,                                                     
                                                     new SqlParameter("IDCulture", idCulture) ,
                                                     new SqlParameter("StartRowIndex", startRowIndex) ,
                                                     new SqlParameter("MaximumRows", maximumRows) ,
                                                     recordCountParameter};

                sql.AppendLine("exec pElementCulture_GetAll @IDElement, @IDCulture, @StartRowIndex, @MaximumRows, @RecordCount out");
                var result = context.Database.SqlQuery<Model.ElementCultureExtended>(sql.ToString(), parameters).ToList();
                recordCount = Convert.ToInt32(recordCountParameter.Value);
                return result;
            }
        }

        public IList<ElementCulture> GetAll()
        {
            using (var context = new ImobeNetContext())
            {
                return (from oc in context.ElementCulture.ToList()
                        select new ElementCulture
                        {
                            IDCulture = oc.IDCulture,
                            IDElement = oc.IDElement,
                            Name = oc.Name,
                            Description = oc.Description,
                        }).ToList();
            }
        }

        public ElementCulture GetById(int id)
        {
            int recordCount = 0;
            return this.GetAll(out recordCount, id).FirstOrDefault();
        }

        public ElementCultureExtended GetById(int idObject, string idCulture)
        {
            int recordCount = 0;
            return this.GetAll(out recordCount, idObject, idCulture, null, 0, 1).FirstOrDefault();
        }
    }
}
