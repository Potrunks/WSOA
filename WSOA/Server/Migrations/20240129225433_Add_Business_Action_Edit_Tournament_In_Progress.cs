using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSOA.Server.Migrations
{
    /// <inheritdoc />
    public partial class Add_Business_Action_Edit_Tournament_In_Progress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BusinessActions",
                columns: new[] { "Code", "Label" },
                values: new object[] { "EDIT_TOURNAMENT_IN_PROGRESS", "Editer un tournoi en cours" });

            migrationBuilder.InsertData(
                table: "BusinessActionsByProfileCode",
                columns: new[] { "Id", "BusinessActionCode", "ProfileCode" },
                values: new object[] { 7, "EDIT_TOURNAMENT_IN_PROGRESS", "ORGA" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BusinessActionsByProfileCode",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "BusinessActions",
                keyColumn: "Code",
                keyValue: "EDIT_TOURNAMENT_IN_PROGRESS");
        }
    }
}
