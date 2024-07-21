using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSOA.Server.Migrations
{
    /// <inheritdoc />
    public partial class Add_Sub_Section_Accounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MainNavSubSections",
                columns: new[] { "Id", "Description", "Label", "MainNavSectionId", "Order", "Url" },
                values: new object[] { 9, "Affiche les informations de tous les comptes", "Comptes", 4, 2, "/account/get/all" });

            migrationBuilder.InsertData(
                table: "MainNavSubSectionsByProfileCode",
                columns: new[] { "Id", "MainNavSubSectionId", "ProfileCode" },
                values: new object[] { 23, 9, "ADMIN" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MainNavSubSectionsByProfileCode",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "MainNavSubSections",
                keyColumn: "Id",
                keyValue: 9);
        }
    }
}
