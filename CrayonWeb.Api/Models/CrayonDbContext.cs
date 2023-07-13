using Microsoft.EntityFrameworkCore;

namespace CrayonWeb.Api.Models
{
    public class CrayonDbContext : DbContext
    {
        public CrayonDbContext(DbContextOptions<CrayonDbContext> options) : base(options)
        {
            
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

    }
}
