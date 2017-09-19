using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using SpongeSolutions.ServicoDireto.Model.AspnetDB;

namespace SpongeSolutions.ServicoDireto.Database
{
    public class AspnetDBContext : DbContext
    {
        public AspnetDBContext()
            : base(SpongeSolutions.ServicoDireto.Database.Properties.Settings.Default.AspnetDB)
        { }
        public DbSet<aspnet_Role> aspnet_Roles { get; set; }
    } 
}
