using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Entities
{
    public class Enums
    {
        public enum PhoneType : short
        {
            Landline = 0,
            Cell = 1,
            FAX = 2,
            PABX = 3,
            Other = 4,
        }

        public enum StatusType : short
        {
            Inactive = 0,
            Active = 1,
            WaitingDetail = 2
        }
    }
}
