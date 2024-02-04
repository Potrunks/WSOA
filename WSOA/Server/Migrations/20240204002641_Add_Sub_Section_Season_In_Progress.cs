using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSOA.Server.Migrations
{
    /// <inheritdoc />
    public partial class Add_Sub_Section_Season_In_Progress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MainNavSubSections",
                columns: new[] { "Id", "Description", "Label", "MainNavSectionId", "Order", "Url" },
                values: new object[] { 7, "Résultats de la saison en cours", "Saison en cours", 1, 0, "/home" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MainNavSubSections",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
