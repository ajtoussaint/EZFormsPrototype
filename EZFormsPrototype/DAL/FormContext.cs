using EZFormsPrototype.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace EZFormsPrototype.DAL
{
    public class FormContext : DbContext
    {
        public FormContext() : base("FormContext")
        {
        }

        public DbSet<Form> Forms { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<TableField> TableFields { get; set; }
        public DbSet<FormField> FormFields { get; set; }
        public DbSet<ExpressionBlock> ExpressionBlocks { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public System.Data.Entity.DbSet<EZFormsPrototype.Models.Flag> Flags { get; set; }
    }
}