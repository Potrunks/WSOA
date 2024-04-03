using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSOA.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddTournamentInProgressSubSection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MainNavSubSections",
                columns: new[] { "Id", "Description", "Label", "MainNavSectionId", "Order", "Url" },
                values: new object[] { 6, "Tournoi en cours", "Tournoi en cours", 3, 3, "/tournament/inProgress" });

            migrationBuilder.InsertData(
                table: "MainNavSubSectionsByProfileCode",
                columns: new[] { "Id", "MainNavSubSectionId", "ProfileCode" },
                values: new object[] { 12, 6, "ORGA" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MainNavSubSectionsByProfileCode",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "MainNavSubSections",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
