using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BistroBoss.Data.Migrations
{
    /// <inheritdoc />
    public partial class v8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zamowienia_Statusy_StatusId",
                table: "Zamowienia");

            migrationBuilder.DropIndex(
                name: "IX_Zamowienia_StatusId",
                table: "Zamowienia");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Zamowienia",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Zamowienia",
                newName: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Zamowienia_StatusId",
                table: "Zamowienia",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Zamowienia_Statusy_StatusId",
                table: "Zamowienia",
                column: "StatusId",
                principalTable: "Statusy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
