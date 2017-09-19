using SpongeSolutions.ServicoDireto.Services.ElementSystem.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using SpongeSolutions.ServicoDireto.Database;
using System.Data.SqlClient;

namespace SpongeSolutions.ServicoDireto.Services.ElementSystem.Implementation
{
    public class ElementBookmarkedService : BaseRepository<ElementBookmarked>, IElementBookmarkedContract
    {
        public ElementBookmarkedService() : base(new ImobeNetContext())
        {
        }

        public IList<ElementExtended> List(string idCulture, int idCustomer)
        {
            StringBuilder sql = new StringBuilder();
            object[] parameters = new object[] { new SqlParameter("IDCustomer", idCustomer),
                                                 new SqlParameter("IDCulture", idCulture)
            };
            using (var context = new ImobeNetContext())
            {
                sql.AppendLine("exec pElement_ListBookmarked @IDCustomer, @IDCulture");
                return context.Database.SqlQuery<Model.ElementExtended>(sql.ToString(), parameters).ToList();
            }
        }

        //public void Delete(int id)
        //{
        //    using (var context = new ImobeNetContext())
        //    {
        //        var elementBookmarkedRepository = new BaseRepository<ElementBookmarked>(context);
        //        elementBookmarkedRepository.Delete(elementBookmarkedRepository.GetById(id));
        //        elementBookmarkedRepository.SaveChanges();
        //    }
        //}

        //public void Delete(ElementBookmarked entity)
        //{
        //    using (var context = new ImobeNetContext())
        //    {
        //        var elementBookmarkedRepository = new BaseRepository<ElementBookmarked>(context);
        //        elementBookmarkedRepository.Delete(entity);
        //        elementBookmarkedRepository.SaveChanges();
        //    }
        //}

        //public IList<ElementBookmarked> GetAll()
        //{
        //    using (var context = new ImobeNetContext())
        //    {
        //        var elementBookmarkedRepository = new BaseRepository<ElementBookmarked>(context);
        //        return elementBookmarkedRepository.GetAll().ToList();
        //    }
        //}

        //public ElementBookmarked GetById(int id)
        //{
        //    using (var context = new ImobeNetContext())
        //    {
        //        var elementBookmarkedRepository = new BaseRepository<ElementBookmarked>(context);
        //        return elementBookmarkedRepository.GetById(id);
        //    }
        //}

        //public void Insert(ElementBookmarked entity)
        //{
        //    using (var context = new ImobeNetContext())
        //    {
        //        var elementBookmarkedRepository = new BaseRepository<ElementBookmarked>(context);
        //        elementBookmarkedRepository.Insert(entity);
        //        elementBookmarkedRepository.SaveChanges();
        //    }
        //}

        //public void Update(ElementBookmarked entity)
        //{
        //    using (var context = new ImobeNetContext())
        //    {
        //        var elementBookmarkedRepository = new BaseRepository<ElementBookmarked>(context);
        //        elementBookmarkedRepository.Update(entity);
        //        elementBookmarkedRepository.SaveChanges();
        //    }
        //}
    }
}
