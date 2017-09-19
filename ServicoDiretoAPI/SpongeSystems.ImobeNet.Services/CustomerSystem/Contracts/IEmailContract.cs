using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using SpongeSolutions.ServicoDireto.Model;
namespace SpongeSolutions.ServicoDireto.Services.CustomerSystem.Contracts
{
    public interface IEmailContract : IBaseService<Email, long>
    {
        IList<EmailExtended> GetAll(string idCulture, out int recordCount, long idEmail = -1, int idCustomerTo = -1, int idCustomerFrom = -1, short status = -1, string sortType = null, int startRowIndex = -1, int maximumRows = -1);
        EmailExtended GetByIdExtended(string idCulture, long id);
        void Delete(long[] ids);
        void Deactivate(long[] ids);
        void MarkAsUnread(long[] ids);
    }
}
