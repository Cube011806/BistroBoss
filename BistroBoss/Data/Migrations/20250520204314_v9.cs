using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BistroBoss.Data.Migrations
{
    /// <inheritdoc />
    public partial class v9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zamowienia_Koszyki_KoszykId",
                table: "Zamowienia");

            migrationBuilder.DropIndex(
                name: "IX_Zamowienia_KoszykId",
                table: "Zamowienia");

            migrationBuilder.AddColumn<int>(
                name: "ZamowienieId",
                table: "Koszyki",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Koszyki_ZamowienieId",
                table: "Koszyki",
                column: "ZamowienieId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Koszyki_Zamowienia_ZamowienieId",
                table: "Koszyki",
                column: "ZamowienieId",
                principalTable: "Zamowienia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Koszyki_Zamowienia_ZamowienieId",
                table: "Koszyki");

            migrationBuilder.DropIndex(
                name: "IX_Koszyki_ZamowienieId",
                table: "Koszyki");

            migrationBuilder.DropColumn(
                name: "ZamowienieId",
                table: "Koszyki");

            migrationBuilder.CreateIndex(
                name: "IX_Zamowienia_KoszykId",
                table: "Zamowienia",
                column: "KoszykId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Zamowienia_Koszyki_KoszykId",
                table: "Zamowienia",
                column: "KoszykId",
                principalTable: "Koszyki",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
