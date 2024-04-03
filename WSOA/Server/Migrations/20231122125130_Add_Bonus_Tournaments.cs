using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WSOA.Server.Migrations
{
    /// <inheritdoc />
    public partial class Add_Bonus_Tournaments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BonusTournaments",
                columns: new[] { "Code", "Label", "LogoPath", "PointAmount" },
                values: new object[,]
                {
                    { "1ST_RKD_KLD", "Elimination 1er au classement", "images/black_skull.png", 20 },
                    { "FOUR_KIND", "Carré", "images/four_kind.png", 10 },
                    { "PRV_WNR_KLD", "Elimination 1er au précédent tournoi", "images/white_skull.png", 20 },
                    { "RYL_STR_FLSH", "Quinte flush royale", "images/royal_straight_flush.png", 50 },
                    { "STR_FLSH", "Quinte flush", "images/straight_flush.png", 30 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BonusTournaments",
                keyColumn: "Code",
                keyValue: "1ST_RKD_KLD");

            migrationBuilder.DeleteData(
                table: "BonusTournaments",
                keyColumn: "Code",
                keyValue: "FOUR_KIND");

            migrationBuilder.DeleteData(
                table: "BonusTournaments",
                keyColumn: "Code",
                keyValue: "PRV_WNR_KLD");

            migrationBuilder.DeleteData(
                table: "BonusTournaments",
                keyColumn: "Code",
                keyValue: "RYL_STR_FLSH");

            migrationBuilder.DeleteData(
                table: "BonusTournaments",
                keyColumn: "Code",
                keyValue: "STR_FLSH");
        }
    }
}
