﻿using Microsoft.EntityFrameworkCore;
using WSOA.Shared.Entity;
using WSOA.Shared.Resources;

namespace WSOA.Server.Data
{
    public class WSOADbContext : DbContext
    {
        public WSOADbContext(DbContextOptions<WSOADbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PresenceState>().HasData
            (
                new PresenceState
                {
                    Code = "PRESENT",
                    Label = "Présent"
                },
                new PresenceState
                {
                    Code = "MAYBE",
                    Label = "Peut-être"
                },
                new PresenceState
                {
                    Code = "ABSENT",
                    Label = "Absent"
                }
            );

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
                // ACCOUNT MAIN NAV SECTION
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
                    Id = 9,
                    Label = "Comptes",
                    MainNavSectionId = 4,
                    Description = "Affiche les informations de tous les comptes",
                    Order = 2,
                    Url = "/account/get/all"
                },
                // TOURNAMENT MAIN NAV SECTION
                new MainNavSubSection
                {
                    Id = 3,
                    Label = "Créer tournoi",
                    MainNavSectionId = 3,
                    Description = "Créer un tournoi",
                    Order = 0,
                    Url = "/tournament/create"
                },
                new MainNavSubSection
                {
                    Id = 4,
                    Label = "Futurs tournois",
                    MainNavSectionId = 3,
                    Description = "Futurs tournois",
                    Order = 1,
                    Url = "/tournament/future"
                },
                new MainNavSubSection
                {
                    Id = 5,
                    Label = "Lancer tournoi",
                    MainNavSectionId = 3,
                    Description = "Lancer un tournoi",
                    Order = 2,
                    Url = "/tournament/execute"
                },
                new MainNavSubSection
                {
                    Id = 6,
                    Label = "Tournoi en cours",
                    MainNavSectionId = 3,
                    Description = "Tournoi en cours",
                    Order = 3,
                    Url = "/tournament/inProgress"
                },
                // HOME MAIN NAV SECTION
                new MainNavSubSection
                {
                    Id = 7,
                    Label = "Saison en cours",
                    MainNavSectionId = 1,
                    Description = "Résultats de la saison en cours",
                    Order = 0,
                    Url = "/home"
                },
                // STATISTICAL MAIN NAV SECTION
                new MainNavSubSection
                {
                    Id = 8,
                    Label = "Ma saison",
                    MainNavSectionId = 2,
                    Description = "Mes résultats de la saison en cours",
                    Order = 0,
                    Url = "/statistical/my-season"
                }
            );

            modelBuilder.Entity<MainNavSubSectionByProfileCode>().HasData
            (
                // INVITER
                new MainNavSubSectionByProfileCode
                {
                    Id = 1,
                    MainNavSubSectionId = 1,
                    ProfileCode = "ADMIN"
                },
                // DECONNEXION
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
                // CREER TOURNOI
                new MainNavSubSectionByProfileCode
                {
                    Id = 6,
                    MainNavSubSectionId = 3,
                    ProfileCode = "ORGA"
                },
                new MainNavSubSectionByProfileCode
                {
                    Id = 17,
                    MainNavSubSectionId = 3,
                    ProfileCode = "ADMIN"
                },
                // FUTURS TOURNOIS
                new MainNavSubSectionByProfileCode
                {
                    Id = 7,
                    MainNavSubSectionId = 4,
                    ProfileCode = "ADMIN"
                },
                new MainNavSubSectionByProfileCode
                {
                    Id = 8,
                    MainNavSubSectionId = 4,
                    ProfileCode = "ORGA"
                },
                new MainNavSubSectionByProfileCode
                {
                    Id = 9,
                    MainNavSubSectionId = 4,
                    ProfileCode = "PLAYER"
                },
                new MainNavSubSectionByProfileCode
                {
                    Id = 10,
                    MainNavSubSectionId = 4,
                    ProfileCode = "GUEST"
                },
                // LANCER TOURNOI
                new MainNavSubSectionByProfileCode
                {
                    Id = 11,
                    MainNavSubSectionId = 5,
                    ProfileCode = "ORGA"
                },
                new MainNavSubSectionByProfileCode
                {
                    Id = 18,
                    MainNavSubSectionId = 5,
                    ProfileCode = "ADMIN"
                },
                // TOURNOI EN COURS
                new MainNavSubSectionByProfileCode
                {
                    Id = 12,
                    MainNavSubSectionId = 6,
                    ProfileCode = "ORGA"
                },
                new MainNavSubSectionByProfileCode
                {
                    Id = 19,
                    MainNavSubSectionId = 6,
                    ProfileCode = "ADMIN"
                },
                // SEASON IN PROGRESS
                new MainNavSubSectionByProfileCode
                {
                    Id = 13,
                    MainNavSubSectionId = 7,
                    ProfileCode = "ADMIN"
                },
                new MainNavSubSectionByProfileCode
                {
                    Id = 14,
                    MainNavSubSectionId = 7,
                    ProfileCode = "ORGA"
                },
                new MainNavSubSectionByProfileCode
                {
                    Id = 15,
                    MainNavSubSectionId = 7,
                    ProfileCode = "PLAYER"
                },
                new MainNavSubSectionByProfileCode
                {
                    Id = 16,
                    MainNavSubSectionId = 7,
                    ProfileCode = "GUEST"
                },
                // MY SEASON
                new MainNavSubSectionByProfileCode
                {
                    Id = 20,
                    MainNavSubSectionId = 8,
                    ProfileCode = "ADMIN"
                },
                new MainNavSubSectionByProfileCode
                {
                    Id = 21,
                    MainNavSubSectionId = 8,
                    ProfileCode = "ORGA"
                },
                new MainNavSubSectionByProfileCode
                {
                    Id = 22,
                    MainNavSubSectionId = 8,
                    ProfileCode = "PLAYER"
                },
                // ALL ACCOUNTS
                new MainNavSubSectionByProfileCode
                {
                    Id = 23,
                    MainNavSubSectionId = 9,
                    ProfileCode = "ADMIN"
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

            modelBuilder.Entity<Address>().HasData
            (
                new Address
                {
                    Id = 1,
                    Content = "2 allée Bourvil 94000 Créteil"
                },
                new Address
                {
                    Id = 2,
                    Content = "3 rue Sebastopol 94600 Choisy-Le-Roi"
                }
            );

            modelBuilder.Entity<BusinessAction>().HasData
            (
                new BusinessAction
                {
                    Code = "EXEC_TOURNAMENT",
                    Label = "Executer un tournoi"
                },
                new BusinessAction
                {
                    Code = "ELIM_PLAYER",
                    Label = "Eliminer un joueur"
                },
                new BusinessAction
                {
                    Code = BusinessActionResources.EDIT_BONUS_TOURNAMENT_EARNED,
                    Label = "Editer le bonus d'un joueur"
                },
                new BusinessAction
                {
                    Code = BusinessActionResources.EDIT_TOTAL_ADDON,
                    Label = "Editer l'add-on d'un joueur"
                },
                new BusinessAction
                {
                    Code = BusinessActionResources.EDIT_PLAYER_PRESENCE,
                    Label = "Editer le statut présence du joueur"
                },
                new BusinessAction
                {
                    Code = BusinessActionResources.CANCEL_TOURNAMENT_IN_PROGRESS,
                    Label = "Annuler un tournoi en cours"
                },
                new BusinessAction
                {
                    Code = BusinessActionResources.EDIT_TOURNAMENT_IN_PROGRESS,
                    Label = "Editer un tournoi en cours"
                },
                new BusinessAction
                {
                    Code = BusinessActionResources.EDIT_PLAYABLE_TOURNAMENT,
                    Label = "Editer un tournoi jouable"
                },
                new BusinessAction
                {
                    Code = BusinessActionResources.COMMON_DASHBOARD,
                    Label = "Dashboard commun"
                },
                new BusinessAction
                {
                    Code = BusinessActionResources.EDIT_WINNABLE_MONEYS,
                    Label = "Editer la répartition du jackpot"
                },
                new BusinessAction
                {
                    Code = BusinessActionResources.MY_RESULTS,
                    Label = "Mes résultats"
                }
            );

            modelBuilder.Entity<BusinessActionByProfileCode>().HasData
            (
                new BusinessActionByProfileCode
                {
                    Id = 1,
                    ProfileCode = "ORGA",
                    BusinessActionCode = "EXEC_TOURNAMENT"
                },
                new BusinessActionByProfileCode
                {
                    Id = 2,
                    ProfileCode = "ORGA",
                    BusinessActionCode = "ELIM_PLAYER"
                },
                new BusinessActionByProfileCode
                {
                    Id = 3,
                    ProfileCode = "ORGA",
                    BusinessActionCode = BusinessActionResources.EDIT_BONUS_TOURNAMENT_EARNED
                },
                new BusinessActionByProfileCode
                {
                    Id = 4,
                    ProfileCode = "ORGA",
                    BusinessActionCode = BusinessActionResources.EDIT_TOTAL_ADDON
                },
                new BusinessActionByProfileCode
                {
                    Id = 5,
                    ProfileCode = "ORGA",
                    BusinessActionCode = BusinessActionResources.EDIT_PLAYER_PRESENCE
                },
                new BusinessActionByProfileCode
                {
                    Id = 6,
                    ProfileCode = "ORGA",
                    BusinessActionCode = BusinessActionResources.CANCEL_TOURNAMENT_IN_PROGRESS
                },
                new BusinessActionByProfileCode
                {
                    Id = 7,
                    ProfileCode = "ORGA",
                    BusinessActionCode = BusinessActionResources.EDIT_TOURNAMENT_IN_PROGRESS
                },
                new BusinessActionByProfileCode
                {
                    Id = 8,
                    ProfileCode = "ORGA",
                    BusinessActionCode = BusinessActionResources.EDIT_PLAYABLE_TOURNAMENT
                },
                new BusinessActionByProfileCode
                {
                    Id = 9,
                    ProfileCode = "ORGA",
                    BusinessActionCode = BusinessActionResources.COMMON_DASHBOARD
                },
                new BusinessActionByProfileCode
                {
                    Id = 10,
                    ProfileCode = "ADMIN",
                    BusinessActionCode = BusinessActionResources.COMMON_DASHBOARD
                },
                new BusinessActionByProfileCode
                {
                    Id = 11,
                    ProfileCode = "PLAYER",
                    BusinessActionCode = BusinessActionResources.COMMON_DASHBOARD
                },
                new BusinessActionByProfileCode
                {
                    Id = 12,
                    ProfileCode = "GUEST",
                    BusinessActionCode = BusinessActionResources.COMMON_DASHBOARD
                },
                new BusinessActionByProfileCode
                {
                    Id = 13,
                    ProfileCode = "ADMIN",
                    BusinessActionCode = "EXEC_TOURNAMENT"
                },
                new BusinessActionByProfileCode
                {
                    Id = 14,
                    ProfileCode = "ADMIN",
                    BusinessActionCode = "ELIM_PLAYER"
                },
                new BusinessActionByProfileCode
                {
                    Id = 15,
                    ProfileCode = "ADMIN",
                    BusinessActionCode = BusinessActionResources.EDIT_BONUS_TOURNAMENT_EARNED
                },
                new BusinessActionByProfileCode
                {
                    Id = 16,
                    ProfileCode = "ADMIN",
                    BusinessActionCode = BusinessActionResources.EDIT_TOTAL_ADDON
                },
                new BusinessActionByProfileCode
                {
                    Id = 17,
                    ProfileCode = "ADMIN",
                    BusinessActionCode = BusinessActionResources.EDIT_PLAYER_PRESENCE
                },
                new BusinessActionByProfileCode
                {
                    Id = 18,
                    ProfileCode = "ADMIN",
                    BusinessActionCode = BusinessActionResources.CANCEL_TOURNAMENT_IN_PROGRESS
                },
                new BusinessActionByProfileCode
                {
                    Id = 19,
                    ProfileCode = "ADMIN",
                    BusinessActionCode = BusinessActionResources.EDIT_TOURNAMENT_IN_PROGRESS
                },
                new BusinessActionByProfileCode
                {
                    Id = 20,
                    ProfileCode = "ADMIN",
                    BusinessActionCode = BusinessActionResources.EDIT_PLAYABLE_TOURNAMENT
                },
                new BusinessActionByProfileCode
                {
                    Id = 21,
                    ProfileCode = "ADMIN",
                    BusinessActionCode = BusinessActionResources.EDIT_WINNABLE_MONEYS
                },
                new BusinessActionByProfileCode
                {
                    Id = 22,
                    ProfileCode = "ORGA",
                    BusinessActionCode = BusinessActionResources.EDIT_WINNABLE_MONEYS
                },
                new BusinessActionByProfileCode
                {
                    Id = 23,
                    ProfileCode = "ADMIN",
                    BusinessActionCode = BusinessActionResources.MY_RESULTS
                },
                new BusinessActionByProfileCode
                {
                    Id = 24,
                    ProfileCode = "ORGA",
                    BusinessActionCode = BusinessActionResources.MY_RESULTS
                },
                new BusinessActionByProfileCode
                {
                    Id = 25,
                    ProfileCode = "PLAYER",
                    BusinessActionCode = BusinessActionResources.MY_RESULTS
                }
            );

            modelBuilder.Entity<BonusTournament>().HasData
            (
                new BonusTournament
                {
                    Code = BonusTournamentResources.FIRST_RANKED_KILLED,
                    Label = "Elimination 1er au classement",
                    PointAmount = 20,
                    LogoPath = "images/black_skull.png"
                },
                new BonusTournament
                {
                    Code = BonusTournamentResources.PREVIOUS_WINNER_KILLED,
                    Label = "Elimination 1er au précédent tournoi",
                    PointAmount = 20,
                    LogoPath = "images/white_skull.png"
                },
                new BonusTournament
                {
                    Code = BonusTournamentResources.FOUR_OF_A_KIND,
                    Label = "Carré",
                    PointAmount = 10,
                    LogoPath = "images/four_kind.png"
                },
                new BonusTournament
                {
                    Code = BonusTournamentResources.STRAIGHT_FLUSH,
                    Label = "Quinte flush",
                    PointAmount = 30,
                    LogoPath = "images/straight_flush.png"
                },
                new BonusTournament
                {
                    Code = BonusTournamentResources.ROYAL_STRAIGHT_FLUSH,
                    Label = "Quinte flush royale",
                    PointAmount = 50,
                    LogoPath = "images/royal_straight_flush.png"
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
        public DbSet<BonusTournament> BonusTournaments { get; set; }
        public DbSet<BonusTournamentEarned> BonusTournamentEarneds { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<PresenceState> PresenceStates { get; set; }
        public DbSet<BusinessAction> BusinessActions { get; set; }
        public DbSet<BusinessActionByProfileCode> BusinessActionsByProfileCode { get; set; }
        public DbSet<Elimination> Eliminations { get; set; }
        public DbSet<JackpotDistribution> JackpotDistributions { get; set; }
    }
}
