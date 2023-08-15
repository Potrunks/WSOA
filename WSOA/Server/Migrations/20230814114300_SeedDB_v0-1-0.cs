using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WSOA.Server.Migrations
{
    /// <inheritdoc />
    public partial class SeedDB_v010 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MainNavSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassIcon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainNavSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "MainNavSubSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    ProfileCode = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "MainNavSubSectionsByProfileCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MainNavSubSectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainNavSubSectionsByProfileCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MainNavSubSectionsByProfileCode_MainNavSubSections_MainNavSubSectionId",
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
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Login", "Password" },
                values: new object[] { 1, "Potrunks", "Trunks92!" });

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
                table: "Profiles",
                columns: new[] { "Code", "Name" },
                values: new object[,]
                {
                    { "ADMIN", "Administrator" },
                    { "GUEST", "Guest" },
                    { "ORGA", "Organizer" },
                    { "PLAYER", "Player" }
                });

            migrationBuilder.InsertData(
                table: "MainNavSubSections",
                columns: new[] { "Id", "Label", "MainNavSectionId", "Name", "Order", "Url" },
                values: new object[] { 1, "Inviter", 4, "Inviter de nouveaux utilisateurs", 0, "/account/invite" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccountId", "FirstName", "LastName", "ProfileCode" },
                values: new object[] { 1, 1, "Alexis", "ARRIAL", "ADMIN" });

            migrationBuilder.InsertData(
                table: "MainNavSubSectionsByProfileCode",
                columns: new[] { "Id", "MainNavSubSectionId", "ProfileCode" },
                values: new object[] { 1, 1, "ADMIN" });

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
                name: "MainNavSubSectionsByProfileCode");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "MainNavSubSections");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "MainNavSections");
        }
    }
}
