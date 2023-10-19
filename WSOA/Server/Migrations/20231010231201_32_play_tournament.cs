using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSOA.Server.Migrations
{
    /// <inheritdoc />
    public partial class _32_play_tournament : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessActions",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Label = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessActions", x => x.Code);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BusinessActionsByProfileCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProfileCode = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BusinessActionCode = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessActionsByProfileCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessActionsByProfileCode_BusinessActions_BusinessActionC~",
                        column: x => x.BusinessActionCode,
                        principalTable: "BusinessActions",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessActionsByProfileCode_Profiles_ProfileCode",
                        column: x => x.ProfileCode,
                        principalTable: "Profiles",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "BusinessActions",
                columns: new[] { "Code", "Label" },
                values: new object[] { "EXEC_TOURNAMENT", "Executer un tournoi" });

            migrationBuilder.InsertData(
                table: "MainNavSubSections",
                columns: new[] { "Id", "Description", "Label", "MainNavSectionId", "Order", "Url" },
                values: new object[] { 5, "Lancer un tournoi", "Lancer tournoi", 3, 2, "/tournament/execute" });

            migrationBuilder.InsertData(
                table: "BusinessActionsByProfileCode",
                columns: new[] { "Id", "BusinessActionCode", "ProfileCode" },
                values: new object[] { 1, "EXEC_TOURNAMENT", "ORGA" });

            migrationBuilder.InsertData(
                table: "MainNavSubSectionsByProfileCode",
                columns: new[] { "Id", "MainNavSubSectionId", "ProfileCode" },
                values: new object[] { 11, 5, "ORGA" });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessActionsByProfileCode_BusinessActionCode",
                table: "BusinessActionsByProfileCode",
                column: "BusinessActionCode");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessActionsByProfileCode_ProfileCode",
                table: "BusinessActionsByProfileCode",
                column: "ProfileCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessActionsByProfileCode");

            migrationBuilder.DropTable(
                name: "BusinessActions");

            migrationBuilder.DeleteData(
                table: "MainNavSubSectionsByProfileCode",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "MainNavSubSections",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
