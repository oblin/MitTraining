using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JagiCore.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Lhc.Data.Data
{
    public class LhcContext : DbContext
    {
        public LhcContext(DbContextOptions options) : base(options) { }

        public DbSet<RegFile> RegFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("dba");

            modelBuilder.Entity<RegFile>().ToTable("reg_file");
            MappingToSnakeCase(modelBuilder, typeof(RegFile));
            modelBuilder.Entity<RegFile>().Property(i => i.Name).HasColumnName("p_name");
        }

        private static void MappingToSnakeCase(ModelBuilder modelBuilder, Type type)
        {
            var entity = modelBuilder.Model.GetEntityTypes(type).SingleOrDefault();

            foreach (var prop in entity.GetProperties())
            {
                prop.Relational().ColumnName = prop.Name.ToSnakeCase();
            }
        }
    }
}
