
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using SpongeSolutions.ServicoDireto.Model;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using System.Data.Entity.ModelConfiguration.Configuration;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using SpongeSolutions.ServicoDireto.Model.Advertisement;

namespace SpongeSolutions.ServicoDireto.Database
{
    public class ImobeNetContext : DbContext
    {
        //public DbSet<BaseEntity> BaseEntity { get; set; }
        public DbSet<Icon> Icon { get; set; }
        public DbSet<Culture> Culture { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<StateProvince> StateProvince { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Purpose> Purpose { get; set; }
        public DbSet<PurposeCulture> PurposeCulture { get; set; }
        public DbSet<Model.Attribute> Attribute { get; set; }
        public DbSet<AttributeCulture> AttributeCulture { get; set; }
        public DbSet<AttributeType> AttributeType { get; set; }
        public DbSet<AttributeTypeCulture> AttributeTypeCulture { get; set; }
        public DbSet<Attribute_AttributeType> Attribute_AttributeType { get; set; }
        public DbSet<HierarchyStructure> HierarchyStructure { get; set; }
        public DbSet<HierarchyStructureCulture> HierarchyStructureCulture { get; set; }
        public DbSet<HierarchyStructurePurpose> HierarchyStructurePurpose { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Element> Element { get; set; }
        public DbSet<ElementAttribute> ElementAttribute { get; set; }
        public DbSet<ElementCulture> ElementCulture { get; set; }
        public DbSet<Filter> Filter { get; set; }
        public DbSet<FilterPurpose> FilterPurpose { get; set; }
        public DbSet<Email> Email { get; set; }
        public DbSet<ElementBookmarked> ElementBookmarked { get; set; }        
        public DbSet<AdsCategory> AdsCategory { get; set; }
        public DbSet<AdsCategoryCulture> AdsCategoryCulture { get; set; }
        public DbSet<AdsCategoryRelation> AdsCategoryRelation { get; set; }
        public DbSet<Alert> Alert { get; set; }
        public DbSet<AlertAttribute> AlertAttribute { get; set; }

        public ImobeNetContext()
            : base(SpongeSolutions.ServicoDireto.Database.Properties.Settings.Default.ImobeNet)
        {
            base.Configuration.AutoDetectChangesEnabled = false;
            base.Configuration.LazyLoadingEnabled = true;

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
            
            //modelBuilder.Entity<Element>().Map(m => { m.Requires("Discriminator").HasValue("E"); });
            //modelBuilder.Entity<ElementExtended>().Map(m => { m.Requires("Discriminator").HasValue("X"); });
            
            //modelBuilder.Entity<ElementCulture>().Map(m => { m.Requires("Discriminator").HasValue("E"); });
            //modelBuilder.Entity<ElementCultureExtended>().Map(m => { m.Requires("Discriminator").HasValue("X"); });
            
        }
    }
}
