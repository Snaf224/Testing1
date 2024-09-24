using BlazorApp1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BlazorApp1.Connection
{
    public class PostgreSql
    {
        public class PostgreSqlContext : DbContext
        {
            public PostgreSqlContext(DbContextOptions<PostgreSqlContext> options) : base(options)
            { }

            public DbSet<Users> users { get; set; }

            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);
            }

            public override int SaveChanges()
            {
                ChangeTracker.DetectChanges();
                return base.SaveChanges();
            }

        }
    }
}
