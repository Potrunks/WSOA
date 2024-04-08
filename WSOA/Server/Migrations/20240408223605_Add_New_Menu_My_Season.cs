using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WSOA.Server.Migrations
{
    /// <inheritdoc />
    public partial class Add_New_Menu_My_Season : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MainNavSubSections",
                columns: new[] { "Id", "Description", "Label", "MainNavSectionId", "Order", "Url" },
                values: new object[] { 8, "Mes résultats de la saison en cours", "Ma saison", 2, 0, "/statistical/my-season" });

            migrationBuilder.InsertData(
                table: "MainNavSubSectionsByProfileCode",
                columns: new[] { "Id", "MainNavSubSectionId", "ProfileCode" },
                values: new object[,]
                {
                    { 20, 8, "ADMIN" },
                    { 21, 8, "ORGA" },
                    { 22, 8, "PLAYER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MainNavSubSectionsByProfileCode",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "MainNavSubSectionsByProfileCode",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "MainNavSubSectionsByProfileCode",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "MainNavSubSections",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
