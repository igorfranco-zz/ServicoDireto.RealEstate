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
using SpongeSolutions.ServicoDireto.Model.AccountSystem;
using SpongeSolutions.ServicoDireto.Services.CustomerSystem.Contracts;
using System.Data.SqlClient;

namespace SpongeSolutions.ServicoDireto.Services.CustomerSystem.Implementation
{
    public class EmailService : IEmailContract
    {
        public void Insert(Email entity)
        {
            using (var context = new ImobeNetContext())
            {
                var emailRepository = new BaseRepository<Email>(context);
                emailRepository.Insert(entity);
                emailRepository.SaveChanges();
            }
        }

        public void Delete(Email entity)
        {
            using (var context = new ImobeNetContext())
            {
                var emailRepository = new BaseRepository<Email>(context);
                emailRepository.Delete(entity);
                emailRepository.SaveChanges();
            }
        }

        public void Delete(long[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var emailRepository = new BaseRepository<Email>(context);
                var items = emailRepository.Fetch().Where(p => ids.Contains(p.IDEmail.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                        emailRepository.Delete(item);

                    emailRepository.SaveChanges();
                }
            }
        }

        public void Deactivate(long[] ids)
        {
            using (var context = new ImobeNetContext())
            {
                var emailRepository = new BaseRepository<Email>(context);
                var items = emailRepository.Fetch().Where(p => ids.Contains(p.IDEmail.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                    {
                        item.Status = (short)Enums.EmailType.Excluded;
                        item.ModifiedBy = ServiceContext.ActiveUserName;
                        item.ModifyDate = DateTime.Now;
                        this.Update(item);
                    }
                }
            }
        }

        public void Update(Email entity)
        {
            using (var context = new ImobeNetContext())
            {
                var emailRepository = new BaseRepository<Email>(context);
                emailRepository.Update(entity);
                emailRepository.SaveChanges();
            }
        }

        public IList<Email> GetAll()
        {
            using (var context = new ImobeNetContext())
            {
                return (from email in context.Email.ToList()
                        orderby email.CreateDate
                        select email).ToList();
            }
        }

        public IList<Email> GetByStatus(short status)
        {
            return this.GetAll().Where(p => p.Status == status).ToList();
        }

        public IList<Email> GetAllActive()
        {
            return this.GetByStatus((short)Enums.StatusType.Active).ToList();
        }

        public void Delete(long id)
        {
            using (var context = new ImobeNetContext())
            {
                var emailRepository = new BaseRepository<Email>(context);
                emailRepository.Delete(this.GetById(id));
                emailRepository.SaveChanges();
            }
        }

        public Email GetById(long id)
        {
            return this.GetAll().Where(p => p.IDEmail == id).FirstOrDefault();
        }

        public EmailExtended GetByIdExtended(string idCulture, long id)
        {
            int recordCount = 0;
            return this.GetAll(idCulture, out recordCount, id, startRowIndex: 0, maximumRows: 1).FirstOrDefault();
        }

        public IList<EmailExtended> GetAll(string idCulture, out int recordCount, long idEmail = -1, int idCustomerTo = -1, int idCustomerFrom = -1, short status = -1, string sortType = null, int startRowIndex = -1, int maximumRows = -1)
        {
            using (var context = new ImobeNetContext())
            {
                recordCount = 0;
                SqlParameter recordCountParameter = new SqlParameter("RecordCount", 0);
                recordCountParameter.Direction = System.Data.ParameterDirection.Output;

                StringBuilder sql = new StringBuilder();
                object[] parameters = new object[] {
                                                     new SqlParameter("IDCulture", idCulture),
                                                     new SqlParameter("IDEmail", idEmail) ,   
                                                     new SqlParameter("IDCustomerTo", idCustomerTo) ,
                                                     new SqlParameter("IDCustomerFrom", idCustomerFrom) ,
                                                     new SqlParameter("Status", status) ,
                                                     new SqlParameter("StartRowIndex", startRowIndex) ,
                                                     new SqlParameter("MaximumRows", maximumRows) ,
                                                     recordCountParameter};

                sql.AppendLine("exec pEmail_GetAll @IDCulture, @IDEmail, @IDCustomerTo, @IDCustomerFrom, @Status, @StartRowIndex, @MaximumRows, @RecordCount out");
                var result = context.Database.SqlQuery<Model.EmailExtended>(sql.ToString(), parameters).ToList();
                recordCount = Convert.ToInt32(recordCountParameter.Value);
                return result;
            }
        }

        public void MarkAsUnread(long[] ids) 
        {
            using (var context = new ImobeNetContext())
            {
                var emailRepository = new BaseRepository<Email>(context);
                var items = emailRepository.Fetch().Where(p => ids.Contains(p.IDEmail.Value));
                if (items != null && items.Count() > 0)
                {
                    foreach (var item in items)
                    {
                        item.Read = false;
                        item.ModifiedBy = ServiceContext.ActiveUserName;
                        item.ModifyDate = DateTime.Now;
                        this.Update(item);
                    }
                }
            }
        }
    }
}