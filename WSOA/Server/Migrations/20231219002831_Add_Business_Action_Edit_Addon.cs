using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSOA.Server.Migrations
{
    /// <inheritdoc />
    public partial class Add_Business_Action_Edit_Addon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BusinessActions",
                keyColumn: "Code",
                keyValue: "EDIT_BN_TNMT_EARN",
                column: "Label",
                value: "Editer le bonus d'un joueur");

            migrationBuilder.InsertData(
                table: "BusinessActions",
                columns: new[] { "Code", "Label" },
                values: new object[] { "EDIT_TOTAL_ADDON", "Editer l'add-on d'un joueur" });

            migrationBuilder.InsertData(
                table: "BusinessActionsByProfileCode",
                columns: new[] { "Id", "BusinessActionCode", "ProfileCode" },
                values: new object[] { 4, "EDIT_TOTAL_ADDON", "ORGA" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BusinessActionsByProfileCode",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "BusinessActions",
                keyColumn: "Code",
                keyValue: "EDIT_TOTAL_ADDON");

            migrationBuilder.UpdateData(
                table: "BusinessActions",
                keyColumn: "Code",
                keyValue: "EDIT_BN_TNMT_EARN",
                column: "Label",
                value: "Editer un bonus d'un joueur");
        }
    }
}
