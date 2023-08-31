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
                    Name = "Administrateur"
                },
                new Profile
                {
                    Code = "ORGA",
                    Name = "Organisateur"
                },
                new Profile
                {
                    Code = "PLAYER",
                    Name = "Joueur"
                },
                new Profile
                {
                    Code = "GUEST",
                    Name = "Invité"
                }
            );

            modelBuilder.Entity<MainNavSection>().HasData
            (
                new MainNavSection
                {
                    Id = 1,
                    Name = "Home",
                    Label = "Accueil",
                    Order = 0,
                    ClassIcon = "uil uil-estate"
                },
                new MainNavSection
                {
                    Id = 2,
                    Name = "Statistical",
                    Label = "Statistique",
                    Order = 1,
                    ClassIcon = "uil uil-chart-pie"
                },
                new MainNavSection
                {
                    Id = 3,
                    Name = "Tournament",
                    Label = "Tournoi",
                    Order = 2,
                    ClassIcon = "uil uil-spade"
                },
                new MainNavSection
                {
                    Id = 4,
                    Name = "Account",
                    Label = "Compte",
                    Order = 3,
                    ClassIcon = "uil uil-user"
                }
            );

            modelBuilder.Entity<MainNavSubSection>().HasData
            (
                new MainNavSubSection
                {
                    Id = 1,
                    Label = "Inviter",
                    MainNavSectionId = 4,
                    Description = "Inviter un nouvel utilisateur",
                    Order = 0,
                    Url = "/account/invite"
                },
                new MainNavSubSection
                {
                    Id = 2,
                    Label = "Deconnexion",
                    MainNavSectionId = 4,
                    Description = "Deconnexion",
                    Order = 1,
                    Url = "/account/logOut"
                },
                new MainNavSubSection
                {
                    Id = 3,
                    Label = "Créer tournoi",
                    MainNavSectionId = 3,
                    Description = "Créer un tournoi",
                    Order = 0,
                    Url = "/tournament/create"
                }
            );

            modelBuilder.Entity<MainNavSubSectionByProfileCode>().HasData
            (
                new MainNavSubSectionByProfileCode
                {
                    Id = 1,
                    MainNavSubSectionId = 1,
                    ProfileCode = "ADMIN"
                },
                new MainNavSubSectionByProfileCode
                {
                    Id = 2,
                    MainNavSubSectionId = 2,
                    ProfileCode = "ADMIN"
                },
                new MainNavSubSectionByProfileCode
                {
                    Id = 3,
                    MainNavSubSectionId = 2,
                    ProfileCode = "ORGA"
                },
                new MainNavSubSectionByProfileCode
                {
                    Id = 4,
                    MainNavSubSectionId = 2,
                    ProfileCode = "PLAYER"
                },
                new MainNavSubSectionByProfileCode
                {
                    Id = 5,
                    MainNavSubSectionId = 2,
                    ProfileCode = "GUEST"
                },
                new MainNavSubSectionByProfileCode
                {
                    Id = 6,
                    MainNavSubSectionId = 3,
                    ProfileCode = "ORGA"
                }
            );

            modelBuilder.Entity<Account>().HasData
            (
                new Account
                {
                    Id = 1,
                    Login = "Potrunks",
                    Password = "1a753d495dab76bf6288f5b5f9736c3af6b60a5bb819f4de4bf75f79af085181"
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
                    ProfileCode = "ADMIN",
                    Email = "potrunks@hotmail.com"
                }
            );
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<MainNavSection> MainNavSections { get; set; }
        public DbSet<MainNavSubSection> MainNavSubSections { get; set; }
        public DbSet<MainNavSubSectionByProfileCode> MainNavSubSectionsByProfileCode { get; set; }
        public DbSet<LinkAccountCreation> LinkAccountCreations { get; set; }
    }
}
