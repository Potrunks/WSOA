using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WSOA.Server.Migrations
{
    /// <inheritdoc />
    public partial class Add_New_Business_Action_Dashboard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BusinessActions",
                columns: new[] { "Code", "Label" },
                values: new object[] { "COMMON_DASHBOARD", "Dashboard commun" });

            migrationBuilder.InsertData(
                table: "BusinessActionsByProfileCode",
                columns: new[] { "Id", "BusinessActionCode", "ProfileCode" },
                values: new object[,]
                {
                    { 9, "COMMON_DASHBOARD", "ORGA" },
                    { 10, "COMMON_DASHBOARD", "ADMIN" },
                    { 11, "COMMON_DASHBOARD", "PLAYER" },
                    { 12, "COMMON_DASHBOARD", "GUEST" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BusinessActionsByProfileCode",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "BusinessActionsByProfileCode",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "BusinessActionsByProfileCode",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "BusinessActionsByProfileCode",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "BusinessActions",
                keyColumn: "Code",
                keyValue: "COMMON_DASHBOARD");
        }
    }
}
