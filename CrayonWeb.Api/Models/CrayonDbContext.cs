using Microsoft.EntityFrameworkCore;

namespace CrayonWeb.Api.Models
{
    public class CrayonDbContext : DbContext
    {
        public CrayonDbContext(DbContextOptions<CrayonDbContext> options) : base(options)
        {
            
        }

        public CrayonDbContext() : base()
        {

        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }

    }
}
