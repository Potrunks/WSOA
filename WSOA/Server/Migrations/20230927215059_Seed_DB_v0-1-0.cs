using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WSOA.Server.Migrations
{
    /// <inheritdoc />
    public partial class Seed_DB_v010 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Login = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BonusTournaments",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Label = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PointAmount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonusTournaments", x => x.Code);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LinkAccountCreations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RecipientMail = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpirationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ProfileCode = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkAccountCreations", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MainNavSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Label = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClassIcon = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainNavSections", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PresenceStates",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Label = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresenceStates", x => x.Code);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Code);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Season = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsInProgress = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsOver = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    BuyIn = table.Column<int>(type: "int", nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tournaments_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MainNavSubSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Label = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MainNavSectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainNavSubSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MainNavSubSections_MainNavSections_MainNavSectionId",
                        column: x => x.MainNavSectionId,
                        principalTable: "MainNavSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    ProfileCode = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Profiles_ProfileCode",
                        column: x => x.ProfileCode,
                        principalTable: "Profiles",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MainNavSubSectionsByProfileCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProfileCode = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MainNavSubSectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainNavSubSectionsByProfileCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MainNavSubSectionsByProfileCode_MainNavSubSections_MainNavSu~",
                        column: x => x.MainNavSubSectionId,
                        principalTable: "MainNavSubSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MainNavSubSectionsByProfileCode_Profiles_ProfileCode",
                        column: x => x.ProfileCode,
                        principalTable: "Profiles",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TotalWinningsPoint = table.Column<int>(type: "int", nullable: true),
                    CurrentTournamentPosition = table.Column<int>(type: "int", nullable: true),
                    EliminatorPlayerId = table.Column<int>(type: "int", nullable: true),
                    TotalReBuy = table.Column<int>(type: "int", nullable: true),
                    TotalAddOn = table.Column<int>(type: "int", nullable: true),
                    PresenceStateCode = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WasPresent = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    WasFinalTable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TotalWinningsAmount = table.Column<int>(type: "int", nullable: true),
                    PlayedTournamentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_Players_EliminatorPlayerId",
                        column: x => x.EliminatorPlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Players_PresenceStates_PresenceStateCode",
                        column: x => x.PresenceStateCode,
                        principalTable: "PresenceStates",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Players_Tournaments_PlayedTournamentId",
                        column: x => x.PlayedTournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Players_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BonusTournamentEarneds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BonusTournamentCode = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    PointAmount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonusTournamentEarneds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BonusTournamentEarneds_BonusTournaments_BonusTournamentCode",
                        column: x => x.BonusTournamentCode,
                        principalTable: "BonusTournaments",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BonusTournamentEarneds_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Login", "Password" },
                values: new object[,]
                {
                    { 1, "Potrunks", "1a753d495dab76bf6288f5b5f9736c3af6b60a5bb819f4de4bf75f79af085181" },
                    { 2, "PotrunksOrga", "1a753d495dab76bf6288f5b5f9736c3af6b60a5bb819f4de4bf75f79af085181" },
                    { 3, "PotrunksPlayer", "1a753d495dab76bf6288f5b5f9736c3af6b60a5bb819f4de4bf75f79af085181" }
                });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Content" },
                values: new object[,]
                {
                    { 1, "2 allée Bourvil 94000 Créteil" },
                    { 2, "3 rue Sebastopol 94600 Choisy-Le-Roi" }
                });

            migrationBuilder.InsertData(
                table: "MainNavSections",
                columns: new[] { "Id", "ClassIcon", "Label", "Name", "Order" },
                values: new object[,]
                {
                    { 1, "uil uil-estate", "Accueil", "Home", 0 },
                    { 2, "uil uil-chart-pie", "Statistique", "Statistical", 1 },
                    { 3, "uil uil-spade", "Tournoi", "Tournament", 2 },
                    { 4, "uil uil-user", "Compte", "Account", 3 }
                });

            migrationBuilder.InsertData(
                table: "PresenceStates",
                columns: new[] { "Code", "Label" },
                values: new object[,]
                {
                    { "ABSENT", "Absent" },
                    { "MAYBE", "Peut-être" },
                    { "PRESENT", "Présent" }
                });

            migrationBuilder.InsertData(
                table: "Profiles",
                columns: new[] { "Code", "Name" },
                values: new object[,]
                {
                    { "ADMIN", "Administrateur" },
                    { "GUEST", "Invité" },
                    { "ORGA", "Organisateur" },
                    { "PLAYER", "Joueur" }
                });

            migrationBuilder.InsertData(
                table: "MainNavSubSections",
                columns: new[] { "Id", "Description", "Label", "MainNavSectionId", "Order", "Url" },
                values: new object[,]
                {
                    { 1, "Inviter un nouvel utilisateur", "Inviter", 4, 0, "/account/invite" },
                    { 2, "Deconnexion", "Deconnexion", 4, 1, "/account/logOut" },
                    { 3, "Créer un tournoi", "Créer tournoi", 3, 0, "/tournament/create" },
                    { 4, "Futurs tournois", "Futurs tournois", 3, 1, "/tournament/future" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccountId", "Email", "FirstName", "LastName", "ProfileCode" },
                values: new object[,]
                {
                    { 1, 1, "potrunks@hotmail.com", "Alexis", "ARRIAL", "ADMIN" },
                    { 2, 2, "potrunks@gmail.com", "Organisateur", "ORGANISATEUR", "ORGA" },
                    { 3, 3, "arrial.alexis@hotmail.fr", "Player", "PLAYER", "PLAYER" }
                });

            migrationBuilder.InsertData(
                table: "MainNavSubSectionsByProfileCode",
                columns: new[] { "Id", "MainNavSubSectionId", "ProfileCode" },
                values: new object[,]
                {
                    { 1, 1, "ADMIN" },
                    { 2, 2, "ADMIN" },
                    { 3, 2, "ORGA" },
                    { 4, 2, "PLAYER" },
                    { 5, 2, "GUEST" },
                    { 6, 3, "ORGA" },
                    { 7, 4, "ADMIN" },
                    { 8, 4, "ORGA" },
                    { 9, 4, "PLAYER" },
                    { 10, 4, "GUEST" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BonusTournamentEarneds_BonusTournamentCode",
                table: "BonusTournamentEarneds",
                column: "BonusTournamentCode");

            migrationBuilder.CreateIndex(
                name: "IX_BonusTournamentEarneds_PlayerId",
                table: "BonusTournamentEarneds",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_MainNavSubSections_MainNavSectionId",
                table: "MainNavSubSections",
                column: "MainNavSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_MainNavSubSectionsByProfileCode_MainNavSubSectionId",
                table: "MainNavSubSectionsByProfileCode",
                column: "MainNavSubSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_MainNavSubSectionsByProfileCode_ProfileCode",
                table: "MainNavSubSectionsByProfileCode",
                column: "ProfileCode");

            migrationBuilder.CreateIndex(
                name: "IX_Players_EliminatorPlayerId",
                table: "Players",
                column: "EliminatorPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_PlayedTournamentId",
                table: "Players",
                column: "PlayedTournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_PresenceStateCode",
                table: "Players",
                column: "PresenceStateCode");

            migrationBuilder.CreateIndex(
                name: "IX_Players_UserId",
                table: "Players",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_AddressId",
                table: "Tournaments",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AccountId",
                table: "Users",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProfileCode",
                table: "Users",
                column: "ProfileCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BonusTournamentEarneds");

            migrationBuilder.DropTable(
                name: "LinkAccountCreations");

            migrationBuilder.DropTable(
                name: "MainNavSubSectionsByProfileCode");

            migrationBuilder.DropTable(
                name: "BonusTournaments");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "MainNavSubSections");

            migrationBuilder.DropTable(
                name: "PresenceStates");

            migrationBuilder.DropTable(
                name: "Tournaments");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "MainNavSections");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Profiles");
        }
    }
}
