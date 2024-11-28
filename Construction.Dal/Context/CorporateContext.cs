using Construction.Dal.Entity;
using Construction.Dal.Migrations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Construction.Dal.Context
{
    public class ConstructionContext : DbContext
    {
        public ConstructionContext() : base("dbCon")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ConstructionContext, Configuration>());
        }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<ContactSetting> ContactSettings { get; set; }
        public DbSet<ConversionKey> ConversionKeys { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<LanguageConversion> LanguageConversions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<SecurityObject> SecurityObjects { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SliderConversion> SliderConversions { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<UrlRecord> UrlRecords { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Verification> Verifications { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}
