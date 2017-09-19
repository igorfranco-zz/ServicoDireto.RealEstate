using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using SpongeSystems.Spider.Entities.ServicoDireto;

namespace SpongeSystems.Spider.Services.Contracts
{
    public interface ILinkContract : IBaseService<Link>
    {
        Task UpdateByLinkID(Link record);
        void InsertUpdate(Link record);
    }
}
