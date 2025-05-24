using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BistroBoss.Data.Migrations
{
    /// <inheritdoc />
    public partial class v11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zamowienia_Dostawy_DostawaId",
                table: "Zamowienia");

            migrationBuilder.DropIndex(
                name: "IX_Zamowienia_DostawaId",
                table: "Zamowienia");

            migrationBuilder.DropColumn(
                name: "DostawaId",
                table: "Zamowienia");

            migrationBuilder.AddColumn<string>(
                name: "KodPocztowy",
                table: "Zamowienia",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Miejscowosc",
                table: "Zamowienia",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NumerBudynku",
                table: "Zamowienia",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ulica",
                table: "Zamowienia",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KodPocztowy",
                table: "Zamowienia");

            migrationBuilder.DropColumn(
                name: "Miejscowosc",
                table: "Zamowienia");

            migrationBuilder.DropColumn(
                name: "NumerBudynku",
                table: "Zamowienia");

            migrationBuilder.DropColumn(
                name: "Ulica",
                table: "Zamowienia");

            migrationBuilder.AddColumn<int>(
                name: "DostawaId",
                table: "Zamowienia",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Zamowienia_DostawaId",
                table: "Zamowienia",
                column: "DostawaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Zamowienia_Dostawy_DostawaId",
                table: "Zamowienia",
                column: "DostawaId",
                principalTable: "Dostawy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
