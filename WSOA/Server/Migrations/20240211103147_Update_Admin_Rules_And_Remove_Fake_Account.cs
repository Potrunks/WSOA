using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WSOA.Server.Migrations
{
    /// <inheritdoc />
    public partial class Update_Admin_Rules_And_Remove_Fake_Account : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.InsertData(
                table: "BusinessActionsByProfileCode",
                columns: new[] { "Id", "BusinessActionCode", "ProfileCode" },
                values: new object[,]
                {
                    { 13, "EXEC_TOURNAMENT", "ADMIN" },
                    { 14, "ELIM_PLAYER", "ADMIN" },
                    { 15, "EDIT_BN_TNMT_EARN", "ADMIN" },
                    { 16, "EDIT_TOTAL_ADDON", "ADMIN" },
                    { 17, "EDIT_PLAYER_PRESENCE", "ADMIN" },
                    { 18, "CANCEL_TOURNAMENT_IN_PROGRESS", "ADMIN" },
                    { 19, "EDIT_TOURNAMENT_IN_PROGRESS", "ADMIN" },
                    { 20, "EDIT_PLAYABLE_TOURNAMENT", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "MainNavSubSectionsByProfileCode",
                columns: new[] { "Id", "MainNavSubSectionId", "ProfileCode" },
                values: new object[,]
                {
                    { 17, 3, "ADMIN" },
                    { 18, 5, "ADMIN" },
                    { 19, 6, "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BusinessActionsByProfileCode",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "BusinessActionsByProfileCode",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "BusinessActionsByProfileCode",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "BusinessActionsByProfileCode",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "BusinessActionsByProfileCode",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "BusinessActionsByProfileCode",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "BusinessActionsByProfileCode",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "BusinessActionsByProfileCode",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "MainNavSubSectionsByProfileCode",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "MainNavSubSectionsByProfileCode",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "MainNavSubSectionsByProfileCode",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Login", "Password" },
                values: new object[,]
                {
                    { 2, "PotrunksOrga", "1a753d495dab76bf6288f5b5f9736c3af6b60a5bb819f4de4bf75f79af085181" },
                    { 3, "PotrunksPlayer", "1a753d495dab76bf6288f5b5f9736c3af6b60a5bb819f4de4bf75f79af085181" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccountId", "Email", "FirstName", "LastName", "ProfileCode" },
                values: new object[,]
                {
                    { 2, 2, "potrunks@gmail.com", "Organisateur", "ORGANISATEUR", "ORGA" },
                    { 3, 3, "arrial.alexis@hotmail.fr", "Player", "PLAYER", "PLAYER" }
                });
        }
    }
}
