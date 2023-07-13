using Microsoft.EntityFrameworkCore;

namespace CrayonWeb.Api.Models
{
    public static class DevDb
    {
        public static void Populate(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<CrayonDbContext>();
                if (context != null)
                {
                    SeedData(context);
                }                
            }
        }

        private static void SeedData(CrayonDbContext context)
        {
            context.Database.Migrate();
            if (!context.Customers.Any())
            {
                var cowabungaAccounts = new List<Account>()
                {
                    new Account { Name = "Leonardo" },
                    new Account { Name = "Raphael" },
                    new Account { Name = "Donatello" },
                    new Account { Name = "Michelangelo" }
                };
                var watchmenAccounts = new List<Account>()
                {
                    new Account { Name = "Doctor Manhattan" },
                    new Account { Name = "Rorschach" },
                    new Account { Name = "Silk Spectre" },
                    new Account { Name = "Nite Owl" }
                };
                context.Customers.AddRange(
                    new Customer { Name = "Cowabunga Inc.", Accounts = cowabungaAccounts },
                    new Customer { Name = "Watchmen Inc.", Accounts = watchmenAccounts }
                );
                context.SaveChanges();
            }
        }
    }
}
