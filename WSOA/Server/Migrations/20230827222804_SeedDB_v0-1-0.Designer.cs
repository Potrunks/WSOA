﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WSOA.Server.Data;

#nullable disable

namespace WSOA.Server.Migrations
{
    [DbContext(typeof(WSOADbContext))]
    [Migration("20230827222804_SeedDB_v0-1-0")]
    partial class SeedDB_v010
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WSOA.Shared.Entity.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Accounts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Login = "Potrunks",
                            Password = "1a753d495dab76bf6288f5b5f9736c3af6b60a5bb819f4de4bf75f79af085181"
                        });
                });

            modelBuilder.Entity("WSOA.Shared.Entity.LinkAccountCreation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProfileCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecipientMail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("LinkAccountCreations");
                });

            modelBuilder.Entity("WSOA.Shared.Entity.MainNavSection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClassIcon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MainNavSectionId")
                        .HasColumnType("int");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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
                        });
                });

            modelBuilder.Entity("WSOA.Shared.Entity.MainNavSubSectionByProfileCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MainNavSubSectionId")
                        .HasColumnType("int");

                    b.Property<string>("ProfileCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

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
                        });
                });

            modelBuilder.Entity("WSOA.Shared.Entity.Profile", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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

            modelBuilder.Entity("WSOA.Shared.Entity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfileCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

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
                        });
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