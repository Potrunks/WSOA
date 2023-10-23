﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WSOA.Server.Data;

#nullable disable

namespace WSOA.Server.Migrations
{
    [DbContext(typeof(WSOADbContext))]
    partial class WSOADbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("WSOA.Shared.Entity.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Accounts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Login = "Potrunks",
                            Password = "1a753d495dab76bf6288f5b5f9736c3af6b60a5bb819f4de4bf75f79af085181"
                        },
                        new
                        {
                            Id = 2,
                            Login = "PotrunksOrga",
                            Password = "1a753d495dab76bf6288f5b5f9736c3af6b60a5bb819f4de4bf75f79af085181"
                        },
                        new
                        {
                            Id = 3,
                            Login = "PotrunksPlayer",
                            Password = "1a753d495dab76bf6288f5b5f9736c3af6b60a5bb819f4de4bf75f79af085181"
                        });
                });

            modelBuilder.Entity("WSOA.Shared.Entity.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Addresses");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Content = "2 allée Bourvil 94000 Créteil"
                        },
                        new
                        {
                            Id = 2,
                            Content = "3 rue Sebastopol 94600 Choisy-Le-Roi"
                        });
                });

            modelBuilder.Entity("WSOA.Shared.Entity.BonusTournament", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("PointAmount")
                        .HasColumnType("int");

                    b.HasKey("Code");

                    b.ToTable("BonusTournaments");
                });

            modelBuilder.Entity("WSOA.Shared.Entity.BonusTournamentEarned", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("BonusTournamentCode")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int>("PointAmount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BonusTournamentCode");

                    b.HasIndex("PlayerId");

                    b.ToTable("BonusTournamentEarneds");
                });

            modelBuilder.Entity("WSOA.Shared.Entity.BusinessAction", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Code");

                    b.ToTable("BusinessActions");

                    b.HasData(
                        new
                        {
                            Code = "EXEC_TOURNAMENT",
                            Label = "Executer un tournoi"
                        });
                });

            modelBuilder.Entity("WSOA.Shared.Entity.BusinessActionByProfileCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("BusinessActionCode")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProfileCode")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("BusinessActionCode");

                    b.HasIndex("ProfileCode");

                    b.ToTable("BusinessActionsByProfileCode");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BusinessActionCode = "EXEC_TOURNAMENT",
                            ProfileCode = "ORGA"
                        });
                });

            modelBuilder.Entity("WSOA.Shared.Entity.LinkAccountCreation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ProfileCode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("RecipientMail")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("LinkAccountCreations");
                });

            modelBuilder.Entity("WSOA.Shared.Entity.MainNavSection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClassIcon")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("MainNavSections");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ClassIcon = "uil uil-estate",
                            Label = "Accueil",
                            Name = "Home",
                            Order = 0
                        },
                        new
                        {
                            Id = 2,
                            ClassIcon = "uil uil-chart-pie",
                            Label = "Statistique",
                            Name = "Statistical",
                            Order = 1
                        },
                        new
                        {
                            Id = 3,
                            ClassIcon = "uil uil-spade",
                            Label = "Tournoi",
                            Name = "Tournament",
                            Order = 2
                        },
                        new
                        {
                            Id = 4,
                            ClassIcon = "uil uil-user",
                            Label = "Compte",
                            Name = "Account",
                            Order = 3
                        });
                });

            modelBuilder.Entity("WSOA.Shared.Entity.MainNavSubSection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("MainNavSectionId")
                        .HasColumnType("int");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("MainNavSectionId");

                    b.ToTable("MainNavSubSections");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Inviter un nouvel utilisateur",
                            Label = "Inviter",
                            MainNavSectionId = 4,
                            Order = 0,
                            Url = "/account/invite"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Deconnexion",
                            Label = "Deconnexion",
                            MainNavSectionId = 4,
                            Order = 1,
                            Url = "/account/logOut"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Créer un tournoi",
                            Label = "Créer tournoi",
                            MainNavSectionId = 3,
                            Order = 0,
                            Url = "/tournament/create"
                        },
                        new
                        {
                            Id = 4,
                            Description = "Futurs tournois",
                            Label = "Futurs tournois",
                            MainNavSectionId = 3,
                            Order = 1,
                            Url = "/tournament/future"
                        },
                        new
                        {
                            Id = 5,
                            Description = "Lancer un tournoi",
                            Label = "Lancer tournoi",
                            MainNavSectionId = 3,
                            Order = 2,
                            Url = "/tournament/execute"
                        },
                        new
                        {
                            Id = 6,
                            Description = "Tournoi en cours",
                            Label = "Tournoi en cours",
                            MainNavSectionId = 3,
                            Order = 3,
                            Url = "/tournament/inProgress"
                        });
                });

            modelBuilder.Entity("WSOA.Shared.Entity.MainNavSubSectionByProfileCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("MainNavSubSectionId")
                        .HasColumnType("int");

                    b.Property<string>("ProfileCode")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("MainNavSubSectionId");

                    b.HasIndex("ProfileCode");

                    b.ToTable("MainNavSubSectionsByProfileCode");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            MainNavSubSectionId = 1,
                            ProfileCode = "ADMIN"
                        },
                        new
                        {
                            Id = 2,
                            MainNavSubSectionId = 2,
                            ProfileCode = "ADMIN"
                        },
                        new
                        {
                            Id = 3,
                            MainNavSubSectionId = 2,
                            ProfileCode = "ORGA"
                        },
                        new
                        {
                            Id = 4,
                            MainNavSubSectionId = 2,
                            ProfileCode = "PLAYER"
                        },
                        new
                        {
                            Id = 5,
                            MainNavSubSectionId = 2,
                            ProfileCode = "GUEST"
                        },
                        new
                        {
                            Id = 6,
                            MainNavSubSectionId = 3,
                            ProfileCode = "ORGA"
                        },
                        new
                        {
                            Id = 7,
                            MainNavSubSectionId = 4,
                            ProfileCode = "ADMIN"
                        },
                        new
                        {
                            Id = 8,
                            MainNavSubSectionId = 4,
                            ProfileCode = "ORGA"
                        },
                        new
                        {
                            Id = 9,
                            MainNavSubSectionId = 4,
                            ProfileCode = "PLAYER"
                        },
                        new
                        {
                            Id = 10,
                            MainNavSubSectionId = 4,
                            ProfileCode = "GUEST"
                        },
                        new
                        {
                            Id = 11,
                            MainNavSubSectionId = 5,
                            ProfileCode = "ORGA"
                        },
                        new
                        {
                            Id = 12,
                            MainNavSubSectionId = 6,
                            ProfileCode = "ORGA"
                        });
                });

            modelBuilder.Entity("WSOA.Shared.Entity.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("CurrentTournamentPosition")
                        .HasColumnType("int");

                    b.Property<int?>("EliminatorPlayerId")
                        .HasColumnType("int");

                    b.Property<int>("PlayedTournamentId")
                        .HasColumnType("int");

                    b.Property<string>("PresenceStateCode")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("TotalAddOn")
                        .HasColumnType("int");

                    b.Property<int?>("TotalReBuy")
                        .HasColumnType("int");

                    b.Property<int?>("TotalWinningsAmount")
                        .HasColumnType("int");

                    b.Property<int?>("TotalWinningsPoint")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<bool>("WasFinalTable")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("EliminatorPlayerId");

                    b.HasIndex("PlayedTournamentId");

                    b.HasIndex("PresenceStateCode");

                    b.HasIndex("UserId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("WSOA.Shared.Entity.PresenceState", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Code");

                    b.ToTable("PresenceStates");

                    b.HasData(
                        new
                        {
                            Code = "PRESENT",
                            Label = "Présent"
                        },
                        new
                        {
                            Code = "MAYBE",
                            Label = "Peut-être"
                        },
                        new
                        {
                            Code = "ABSENT",
                            Label = "Absent"
                        });
                });

            modelBuilder.Entity("WSOA.Shared.Entity.Profile", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Code");

                    b.ToTable("Profiles");

                    b.HasData(
                        new
                        {
                            Code = "ADMIN",
                            Name = "Administrateur"
                        },
                        new
                        {
                            Code = "ORGA",
                            Name = "Organisateur"
                        },
                        new
                        {
                            Code = "PLAYER",
                            Name = "Joueur"
                        },
                        new
                        {
                            Code = "GUEST",
                            Name = "Invité"
                        });
                });

            modelBuilder.Entity("WSOA.Shared.Entity.Tournament", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<int>("BuyIn")
                        .HasColumnType("int");

                    b.Property<bool>("IsInProgress")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsOver")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Season")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.ToTable("Tournaments");
                });

            modelBuilder.Entity("WSOA.Shared.Entity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ProfileCode")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("ProfileCode");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AccountId = 1,
                            Email = "potrunks@hotmail.com",
                            FirstName = "Alexis",
                            LastName = "ARRIAL",
                            ProfileCode = "ADMIN"
                        },
                        new
                        {
                            Id = 2,
                            AccountId = 2,
                            Email = "potrunks@gmail.com",
                            FirstName = "Organisateur",
                            LastName = "ORGANISATEUR",
                            ProfileCode = "ORGA"
                        },
                        new
                        {
                            Id = 3,
                            AccountId = 3,
                            Email = "arrial.alexis@hotmail.fr",
                            FirstName = "Player",
                            LastName = "PLAYER",
                            ProfileCode = "PLAYER"
                        });
                });

            modelBuilder.Entity("WSOA.Shared.Entity.BonusTournamentEarned", b =>
                {
                    b.HasOne("WSOA.Shared.Entity.BonusTournament", "BonusTournament")
                        .WithMany()
                        .HasForeignKey("BonusTournamentCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WSOA.Shared.Entity.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BonusTournament");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("WSOA.Shared.Entity.BusinessActionByProfileCode", b =>
                {
                    b.HasOne("WSOA.Shared.Entity.BusinessAction", "BusinessAction")
                        .WithMany()
                        .HasForeignKey("BusinessActionCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WSOA.Shared.Entity.Profile", "Profile")
                        .WithMany()
                        .HasForeignKey("ProfileCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BusinessAction");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("WSOA.Shared.Entity.MainNavSubSection", b =>
                {
                    b.HasOne("WSOA.Shared.Entity.MainNavSection", "MainNavSection")
                        .WithMany()
                        .HasForeignKey("MainNavSectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MainNavSection");
                });

            modelBuilder.Entity("WSOA.Shared.Entity.MainNavSubSectionByProfileCode", b =>
                {
                    b.HasOne("WSOA.Shared.Entity.MainNavSubSection", "MainNavSubSection")
                        .WithMany()
                        .HasForeignKey("MainNavSubSectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WSOA.Shared.Entity.Profile", "Profile")
                        .WithMany()
                        .HasForeignKey("ProfileCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MainNavSubSection");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("WSOA.Shared.Entity.Player", b =>
                {
                    b.HasOne("WSOA.Shared.Entity.Player", "EliminatorPlayer")
                        .WithMany()
                        .HasForeignKey("EliminatorPlayerId");

                    b.HasOne("WSOA.Shared.Entity.Tournament", "PlayedTournament")
                        .WithMany()
                        .HasForeignKey("PlayedTournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WSOA.Shared.Entity.PresenceState", "PresenceState")
                        .WithMany()
                        .HasForeignKey("PresenceStateCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WSOA.Shared.Entity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EliminatorPlayer");

                    b.Navigation("PlayedTournament");

                    b.Navigation("PresenceState");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WSOA.Shared.Entity.Tournament", b =>
                {
                    b.HasOne("WSOA.Shared.Entity.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("WSOA.Shared.Entity.User", b =>
                {
                    b.HasOne("WSOA.Shared.Entity.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WSOA.Shared.Entity.Profile", "Profile")
                        .WithMany()
                        .HasForeignKey("ProfileCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Profile");
                });
#pragma warning restore 612, 618
        }
    }
}
