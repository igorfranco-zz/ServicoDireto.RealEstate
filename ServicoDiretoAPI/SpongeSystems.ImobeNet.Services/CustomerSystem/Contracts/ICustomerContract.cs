using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using SpongeSolutions.ServicoDireto.Model;
using System.Threading.Tasks;

namespace SpongeSolutions.ServicoDireto.Services.CustomerSystem.Contracts
{
    public interface ICustomerContract : IBaseService<Customer>
    {
        void Inactivate(int[] ids);
        IList<Customer> GetByStatus(short status);
        IEnumerable<dynamic> AutoComplete(string text);
        Customer GetByUserName(string userName);
        Customer GetBySiteName(string siteName);
        Customer GetByUserEmail(string email);
        Customer GetByActivateKey(Guid activateKey);
        Task<CustomerExtended> GetByIdAsync(int id);

        /// <summary>
        /// Disabilitar eventuais usuários que possuam o mesmo email
        /// </summary>
        /// <param name="email"></param>
        void DisableUser(string email);
        Customer GetInsert(string name, string address, string addressNumber, string email, string phone, string externalSiteID, string logo);
    }
}
