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
            modelBuilder.Entity<Profile>().HasData
            (
                new Profile
                {
                    Code = "ADMIN",
                    Name = "Administrator"
                },
                new Profile
                {
                    Code = "ORGA",
                    Name = "Organizer"
                },
                new Profile
                {
                    Code = "PLAYER",
                    Name = "Player"
                },
                new Profile
                {
                    Code = "GUEST",
                    Name = "Guest"
                }
            );

            modelBuilder.Entity<MainNavSection>().HasData
            (
                new MainNavSection
                {
                    Id = 1,
                    Name = "Home",
                    Label = "Accueil",
                    Order = 1,
                    ClassIcon = "uil uil-estate"
                },
                new MainNavSection
                {
                    Id = 2,
                    Name = "Statistical",
                    Label = "Statistique",
                    Order = 2,
                    ClassIcon = "uil uil-chart-pie"
                },
                new MainNavSection
                {
                    Id = 3,
                    Name = "Tournament",
                    Label = "Tournoi",
                    Order = 3,
                    ClassIcon = "uil uil-spade"
                },
                new MainNavSection
                {
                    Id = 4,
                    Name = "Account",
                    Label = "Compte",
                    Order = 4,
                    ClassIcon = "uil uil-user"
                }
            );

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
                    Id = 1,
                    ProfileCode = "ADMIN"
                }
            );
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<MainNavSection> MainNavSections { get; set; }
    }
}
