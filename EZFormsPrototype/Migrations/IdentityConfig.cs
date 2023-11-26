namespace EZFormsPrototype.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class IdentityConfig : DbMigrationsConfiguration<EZFormsPrototype.Models.ApplicationDbContext>
    {
        public IdentityConfig()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EZFormsPrototype.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
