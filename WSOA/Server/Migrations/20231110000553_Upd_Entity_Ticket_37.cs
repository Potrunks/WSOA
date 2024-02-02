using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSOA.Server.Migrations
{
    /// <inheritdoc />
    public partial class Upd_Entity_Ticket_37 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Players_EliminatorPlayerId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_EliminatorPlayerId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "EliminatorPlayerId",
                table: "Players");

            migrationBuilder.AlterColumn<bool>(
                name: "WasFinalTable",
                table: "Players",
                type: "tinyint(1)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AddColumn<bool>(
                name: "WasAddOn",
                table: "Players",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoPath",
                table: "BonusTournaments",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Occurrence",
                table: "BonusTournamentEarneds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Eliminations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlayerEliminatorId = table.Column<int>(type: "int", nullable: false),
                    PlayerVictimId = table.Column<int>(type: "int", nullable: false),
                    IsDefinitive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eliminations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Eliminations_Players_PlayerEliminatorId",
                        column: x => x.PlayerEliminatorId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Eliminations_Players_PlayerVictimId",
                        column: x => x.PlayerVictimId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Eliminations_PlayerEliminatorId",
                table: "Eliminations",
                column: "PlayerEliminatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Eliminations_PlayerVictimId",
                table: "Eliminations",
                column: "PlayerVictimId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Eliminations");

            migrationBuilder.DropColumn(
                name: "WasAddOn",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "LogoPath",
                table: "BonusTournaments");

            migrationBuilder.DropColumn(
                name: "Occurrence",
                table: "BonusTournamentEarneds");

            migrationBuilder.AlterColumn<bool>(
                name: "WasFinalTable",
                table: "Players",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EliminatorPlayerId",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_EliminatorPlayerId",
                table: "Players",
                column: "EliminatorPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Players_EliminatorPlayerId",
                table: "Players",
                column: "EliminatorPlayerId",
                principalTable: "Players",
                principalColumn: "Id");
        }
    }
}
