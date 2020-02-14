using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SpatialiteCrash.Database
{
    public class SpatialiteCrashDbContext : DbContext
    {
        public static string ConnectionString = "Data Source=EFCoreGithubIssue14561.db";

        public SpatialiteCrashDbContext()
            : base(new DbContextOptionsBuilder()
                  .UseLazyLoadingProxies()
                  .UseSqlite(ConnectionString, sqliteOptions => sqliteOptions.UseNetTopologySuite())
                  .Options)
        {
            Console.WriteLine($"Connection String: {ConnectionString}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            RemoveCascadeDeleteBehavior(modelBuilder);
        }

        private void RemoveCascadeDeleteBehavior(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
