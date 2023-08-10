using Microsoft.EntityFrameworkCore;
using WSOA.Shared.Entity;

namespace WSOA.Server.Data
{
    public class WSOADbContext : DbContext
    {
        public WSOADbContext(DbContextOptions<WSOADbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasData
            (
                new Account
                {
                    Id = 1,
                    Login = "Potrunks",
                    Password = "Trunks92!"
                }
            );

            modelBuilder.Entity<User>().HasData
            (
                new User
                {
                    FirstName = "Alexis",
                    LastName = "ARRIAL",
                    AccountId = 1,
                    Id = 1
                }
            );
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
