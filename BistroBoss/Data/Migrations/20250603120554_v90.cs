using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BistroBoss.Data.Migrations
{
    /// <inheritdoc />
    public partial class v90 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Opinie_Zamowienia_ZamowienieId",
                table: "Opinie");

            migrationBuilder.DropForeignKey(
                name: "FK_Zamowienia_AspNetUsers_UzytkownikId",
                table: "Zamowienia");

            migrationBuilder.DropForeignKey(
                name: "FK_Zamowienia_Opinie_OpiniaId",
                table: "Zamowienia");

            migrationBuilder.DropIndex(
                name: "IX_Opinie_ZamowienieId",
                table: "Opinie");

            migrationBuilder.AddForeignKey(
                name: "FK_Zamowienia_AspNetUsers_UzytkownikId",
                table: "Zamowienia",
                column: "UzytkownikId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Zamowienia_Opinie_OpiniaId",
                table: "Zamowienia",
                column: "OpiniaId",
                principalTable: "Opinie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zamowienia_AspNetUsers_UzytkownikId",
                table: "Zamowienia");

            migrationBuilder.DropForeignKey(
                name: "FK_Zamowienia_Opinie_OpiniaId",
                table: "Zamowienia");

            migrationBuilder.CreateIndex(
                name: "IX_Opinie_ZamowienieId",
                table: "Opinie",
                column: "ZamowienieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Opinie_Zamowienia_ZamowienieId",
                table: "Opinie",
                column: "ZamowienieId",
                principalTable: "Zamowienia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Zamowienia_AspNetUsers_UzytkownikId",
                table: "Zamowienia",
                column: "UzytkownikId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Zamowienia_Opinie_OpiniaId",
                table: "Zamowienia",
                column: "OpiniaId",
                principalTable: "Opinie",
                principalColumn: "Id");
        }
    }
}
