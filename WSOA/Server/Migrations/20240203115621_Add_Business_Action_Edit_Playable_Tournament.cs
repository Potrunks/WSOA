using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSOA.Server.Migrations
{
    /// <inheritdoc />
    public partial class Add_Business_Action_Edit_Playable_Tournament : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BusinessActions",
                columns: new[] { "Code", "Label" },
                values: new object[] { "EDIT_PLAYABLE_TOURNAMENT", "Editer un tournoi jouable" });

            migrationBuilder.InsertData(
                table: "BusinessActionsByProfileCode",
                columns: new[] { "Id", "BusinessActionCode", "ProfileCode" },
                values: new object[] { 8, "EDIT_PLAYABLE_TOURNAMENT", "ORGA" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BusinessActionsByProfileCode",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "BusinessActions",
                keyColumn: "Code",
                keyValue: "EDIT_PLAYABLE_TOURNAMENT");
        }
    }
}
