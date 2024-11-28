namespace Construction.Dal.Migrations
{
    using System.Data.Entity.Migrations;
    internal sealed class Configuration : DbMigrationsConfiguration<Context.ConstructionContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Construction.Dal.Context.ConstructionContext";
        }

        protected override void Seed(Context.ConstructionContext context)
        {
           
        }

    }
}