using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WSOA.Server.Migrations
{
    /// <inheritdoc />
    public partial class Add_Profile_Sub_Section_Season_In_Progress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MainNavSubSectionsByProfileCode",
                columns: new[] { "Id", "MainNavSubSectionId", "ProfileCode" },
                values: new object[,]
                {
                    { 13, 7, "ADMIN" },
                    { 14, 7, "ORGA" },
                    { 15, 7, "PLAYER" },
                    { 16, 7, "GUEST" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MainNavSubSectionsByProfileCode",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "MainNavSubSectionsByProfileCode",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "MainNavSubSectionsByProfileCode",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "MainNavSubSectionsByProfileCode",
                keyColumn: "Id",
                keyValue: 16);
        }
    }
}
