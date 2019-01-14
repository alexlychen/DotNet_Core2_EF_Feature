using AutoLotDAL_Core2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoLotDAL_Core2.EF
{
    public class AutoLotContext : DbContext
    {

        public DbSet<CreditRisk> CreditRisks { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Inventory> Cars { get; set; }
        public DbSet<Order> Orders { get; set; }

        //For the new DbContext pooling ASP.NET Core feature to be used, 
        //there can be only one constructure on your context class, 
        //and that's the one that takes a DbContextOptions instance. 
        //To satisfy both of these needs, create an internal parameterless constructor:
        internal AutoLotContext()
        {
        }

        public AutoLotContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = @"server=NOTEBOOK\MSSQLSERVER2014;database=AutoLotCore2;integrated security=True; MultipleActiveResultSets=True;App=EntityFramework;";
                DbContextBuilder(optionsBuilder, connectionString);
            }
        }

        private static void DbContextBuilder(DbContextOptionsBuilder optionsBuilder, string connectionString)
        {            
            string connString = connectionString;

            optionsBuilder.UseSqlServer(connString,
                options =>
                {
                    if (options == null)
                    {
                        throw new ArgumentNullException(nameof(options));
                    }
                    options.EnableRetryOnFailure();
                })
                .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)); //throw an exception when mixed-mode evaluation occurs.
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //create the multi column index
            modelBuilder.Entity<CreditRisk>(entity =>
            {
                entity.HasIndex(e => new { e.FirstName, e.LastName }).IsUnique();
            });

            //set the cascade options on the relationship
            modelBuilder.Entity<Order>().HasOne(e => e.Car).WithMany(e => e.Orders).OnDelete(DeleteBehavior.ClientSetNull);
        }

        public string GetTableName(Type type)
        {
            return Model.FindEntityType(type).SqlServer().TableName;
        }
    }
}
