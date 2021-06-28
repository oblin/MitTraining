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
        public DbSet<IpdFile> IpdFiles { get; set; }
        public DbSet<BedBasic> BedBasics { get; set; }
        public DbSet<CodeFile> CodeFiles { get; set; }
        public DbSet<CodeDetail> CodeDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("dba");

            modelBuilder.Entity<IpdFile>().ToTable("ipd_file");
            MappingToSnakeCase(modelBuilder, typeof(IpdFile));
            modelBuilder.Entity<IpdFile>().Property(i => i.Name).HasColumnName("p_name");

            modelBuilder.Entity<RegFile>().ToTable("reg_file");
            MappingToSnakeCase(modelBuilder, typeof(RegFile));
            modelBuilder.Entity<RegFile>().Property(i => i.Name).HasColumnName("p_name");

            MappingToSnakeCase(modelBuilder, typeof(BedBasic));

            modelBuilder.Entity<CodeFile>().ToTable("code_file");
            MappingToSnakeCase(modelBuilder, typeof(CodeFile));

            modelBuilder.Entity<CodeDetail>().ToTable("code_dtl");
            MappingToSnakeCase(modelBuilder, typeof(CodeDetail));
            modelBuilder.Entity<CodeDetail>().Property(i => i.Description).HasColumnName("desc_1");
            modelBuilder.Entity<CodeDetail>().HasKey(c => new { c.ItemType, c.ItemCode });

            modelBuilder.Entity<WeekIncome>().ToTable("week_income_report");
            modelBuilder.Entity<WeekIncome>().HasKey(w => new { w.Year, w.Week, w.RegNo });
            modelBuilder.Entity<WeekIncome>().Property(i => i.RegNo).HasColumnName("reg_no");
            //modelBuilder.Entity<WeekIncome>().Property(i => i.MothlyAmount).HasColumnName<decimal>("ca_month_amt");
            //modelBuilder.Entity<WeekIncome>().Property(i => i.SocialMothlyAmount).HasColumnName<decimal>("al_month_amt");
            //modelBuilder.Entity<WeekIncome>().Property(i => i.WeeklyAmount).HasColumnName<decimal>("ca_week_amt");
            //modelBuilder.Entity<WeekIncome>().Property(i => i.SocialWeeklyAmount).HasColumnName<decimal>("al_week_amt");
            //modelBuilder.Entity<WeekIncome>().Property(i => i.UsedWeeklyAmount).HasColumnName<decimal>("used_week_amt");
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
