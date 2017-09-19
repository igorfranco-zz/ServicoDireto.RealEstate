using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using SpongeSolutions.ServicoDireto.Database;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Services.StructureSystem.Contracts;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using SpongeSolutions.ServicoDireto.Services.CustomerSystem.Contracts;
using SpongeSolutions.ServicoDireto.Model.AccountSystem;
using System.Web;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SpongeSolutions.Core.Helpers;

namespace SpongeSolutions.ServicoDireto.Services.CustomerSystem.Implementation
{
    public class CustomerService : ICustomerContract
    {
        public void Insert(Customer entity)
        {
            using (var context = new ImobeNetContext())
            {
                var customerRepository = new BaseRepository<Customer>(context);
                customerRepository.Insert(entity);
                customerRepository.SaveChanges();
            }
        }

        public void Delete(Customer entity)
        {
            using (var context = new ImobeNetContext())
            {
                var customerRepository = new BaseRepository<Customer>(context);
                customerRepository.Delete(entity);
                customerRepository.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var context = new ImobeNetContext())
            {
                var customerRepository = new BaseRepository<Customer>(context);
                customerRepository.Delete(customerRepository.GetById(id));
                customerRepository.SaveChanges();
            }
        }

        public void Update(Customer entity)
        {
            using (var context = new ImobeNetContext())
            {
                var customerRepository = new BaseRepository<Customer>(context);
                customerRepository.Update(entity);
                customerRepository.SaveChanges();
            }
        }

        public void Delete(int[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var customerRepository = new BaseRepository<Customer>(context);
                var items = customerRepository.Fetch().Where(p => ids.Contains(p.IDCustomer.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                        customerRepository.Delete(item);

                    customerRepository.SaveChanges();
                }
            }
        }

        public void Inactivate(int[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var customerRepository = new BaseRepository<Customer>(context);
                var items = customerRepository.Fetch().Where(p => ids.Contains(p.IDCustomer.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                    {
                        item.Status = (short)Enums.StatusType.Inactive;
                        item.ModifiedBy = ServiceContext.ActiveUserName;
                        customerRepository.Update(item);
                    }

                    customerRepository.SaveChanges();
                }
            }
        }

        public IList<Customer> GetAll()
        {
            using (var context = new ImobeNetContext())
            {
                return (from customer in context.Customer.ToList()
                        join city in context.City on customer.IDCity equals city.IDCity
                        join state in context.StateProvince on city.IDStateProvince equals state.IDStateProvince
                        join country in context.Country on state.IDCountry equals country.IDCountry
                        select new Customer
                        {
                            //CountryName = String.Format("{0}-{1}", country.IDCountry, country.Name),
                            //StateName = String.Format("{0}-{1}", state.Acronym, state.Name),
                            IDCountry = country.IDCountry,
                            IDStateProvince = state.IDStateProvince.Value,
                            IDCustomer = customer.IDCustomer,
                            IDCustomerParent = customer.IDCustomerParent,
                            IDCity = customer.IDCity,
                            Name = customer.Name,
                            CRECI = customer.CRECI,
                            Address = customer.Address,
                            PostalCode = customer.PostalCode,
                            Phone1 = customer.Phone1,
                            Phone2 = customer.Phone2,
                            CellPhone = customer.CellPhone,
                            Logo = customer.Logo,
                            AddressNumber = customer.AddressNumber,
                            AddressComplement = customer.AddressComplement,
                            UserName = customer.UserName,
                            SiteName = customer.SiteName,
                            Comments = customer.Comments,
                            DetailView = customer.DetailView,
                            ActivateKey = customer.ActivateKey,
                            IsPromoted = customer.IsPromoted,
                            Status = customer.Status,
                            CreateDate = customer.CreateDate,
                            ModifyDate = customer.ModifyDate,
                            CreatedBy = customer.CreatedBy,
                            ModifiedBy = customer.ModifiedBy,
                            NotifyBy = customer.NotifyBy,
                            CPF_CNPJ = customer.CPF_CNPJ,
                            Email = customer.Email
                        }).ToList();
            }
        }

        public IList<Customer> GetByStatus(short status)
        {
            using (var context = new ImobeNetContext())
            {
                var customerRepository = new BaseRepository<Customer>(context);
                return customerRepository.Fetch().Where(p => p.Status == status).ToList();
            }
        }

        public IList<Customer> GetAllActive()
        {
            return this.GetByStatus((short)Enums.StatusType.Active).ToList();
        }

        public IEnumerable<dynamic> AutoComplete(string text)
        {
            using (var context = new ImobeNetContext())
            {
                return (from customer in context.Customer.ToList()
                        join city in context.City on customer.IDCity equals city.IDCity
                        join state in context.StateProvince on city.IDStateProvince equals state.IDStateProvince
                        join country in context.Country on state.IDCountry equals country.IDCountry
                        where customer.Status == (short)Enums.StatusType.Active && customer.Name.Contains(text)
                        orderby customer.Name
                        select new
                        {
                            id = customer.IDCustomer,
                            value = customer.Name,
                            label = customer.Name
                        }).ToList();
            }
        }

        public Customer GetByUserName(string userName)
        {
            using (var context = new ImobeNetContext())
            {
                var customerRepository = new BaseRepository<Customer>(context);
                return customerRepository.Find(p => p.UserName == userName).FirstOrDefault();
            }
        }

        public Customer GetBySiteName(string siteName)
        {
            using (var context = new ImobeNetContext())
            {
                var customerRepository = new BaseRepository<Customer>(context);
                return customerRepository.Find(p => p.SiteName == siteName).FirstOrDefault();
            }
        }

        public Customer GetByUserEmail(string email)
        {
            using (var context = new ImobeNetContext())
            {
                var customerRepository = new BaseRepository<Customer>(context);
                return customerRepository.Find(p => p.Email == email).FirstOrDefault();
            }
        }

        public Customer GetByActivateKey(Guid activateKey)
        {
            using (var context = new ImobeNetContext())
            {
                var customerRepository = new BaseRepository<Customer>(context);
                return customerRepository.Find(p => p.ActivateKey == activateKey).FirstOrDefault();
            }
        }

        public async Task<CustomerExtended> GetByIdAsync(int id)
        {
            StringBuilder sql = new StringBuilder();
            object[] parameters = new object[] { new SqlParameter("IDCustomer", id) };
            using (var context = new ImobeNetContext())
            {
                sql.AppendLine("exec pCustomer_GetById  @IDCustomer");
                return await context.Database.SqlQuery<Model.CustomerExtended>(sql.ToString(), parameters).FirstOrDefaultAsync();
            }
            //using (var context = new ImobeNetContext())
            //{
            //    var result = from c in context.Customer
            //                 join context.

            //    //var customerRepository = new BaseRepository<Customer>(context);
            //    //return await customerRepository.dbSet.FindAsync(id);
            //}
        }

        public void DisableUser(string email)
        {
            foreach (var item in this.GetAll().Where(p => p.Email == email))
            {
                item.Status = (short)Enums.StatusType.Inactive;
                item.ModifiedBy = ServiceContext.ActiveUserName;
                item.ModifyDate = DateTime.Now;
                this.Update(item);
            }
        }

        public Customer GetById(int id)
        {
            using (var context = new ImobeNetContext())
            {
                var customerRepository = new BaseRepository<Customer>(context);
                return customerRepository.Find(p => p.IDCustomer == id).FirstOrDefault();
            }
        }

        public Customer GetInsert(string name, string address, string addressNumber, string email, string phone, string externalSiteID, string logo)
        {
            using (var context = new ImobeNetContext())
            {
                var customer = (from c in context.Customer
                                where (c.ExternalSiteID == externalSiteID)
                                select c).FirstOrDefault();

                if (customer == null)
                {
                    customer = new Customer()
                    {
                        Name = name,
                        Address = (string.IsNullOrEmpty(address)) ? "NAO DEFINIDO" : address,
                        AddressNumber = (string.IsNullOrEmpty(addressNumber)) ? "-" : addressNumber,
                        Email = (string.IsNullOrEmpty(email)) ? "NAO DEFINIDO" : email,
                        Phone1 = (string.IsNullOrEmpty(phone)) ? "-" : phone,
                        CreateDate = DateTime.Now,
                        CreatedBy = "AUTO",
                        ModifiedBy = "AUTO",
                        ModifyDate = DateTime.Now,
                        Status = (short)Enums.StatusType.Pending
                    };
                    this.Insert(customer);
                    ////try
                    ////{
                    //string newFileName = String.Format("{0}.jpg", Guid.NewGuid().ToString());
                    //string basePath = string.Format(@"D:\Avantika\Projects\ServicoDiretoImoveis\ServivoDiretoAPI\trunk\ServicoDiretoAdmin\Uploads\{0}", customer.IDCustomer);
                    //string virtualBasePath = string.Format(@"/Uploads/{0}/{1}", customer.IDCustomer, newFileName);
                    //string imagePath = String.Format(@"{0}\{1}", basePath, newFileName);

                    //if (!string.IsNullOrEmpty(logo))
                    //{
                    //    IOHelper.DownloadFile(new Uri(logo), imagePath);
                    //    customer.Logo = virtualBasePath;
                    //    this.Update(customer);
                    //}
                    ////}
                    ////catch { }
                }
                return customer;
            }
        }
    }
}