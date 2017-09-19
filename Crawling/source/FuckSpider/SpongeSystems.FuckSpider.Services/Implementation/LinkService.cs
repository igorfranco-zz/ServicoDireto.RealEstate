using MongoDB.Bson;
using MongoDB.Driver;
using SpongeSystems.Spider.Entities.ServicoDireto;
using SpongeSystems.Spider.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Services.Implementation
{
    public class LinkService : BaseService<Link>, ILinkContract
    {
        public LinkService()
            : base("ServicoDireto")
        {

        }

        public async Task UpdateByLinkID(Link record)
        {
            await _collection.ReplaceOneAsync(x => x.LinkID == record.LinkID, record);
        }

        public void InsertUpdate(Link record)
        {
            var item = _collection.Find(p => p.LinkID == record.LinkID).FirstOrDefaultAsync().GetAwaiter().GetResult();
            if (item == null)
                this.Insert(record).Wait();
            else
            {
                record.Id = item.Id;
                this.UpdateByLinkID(record).Wait();
            }
        }
    }
}


