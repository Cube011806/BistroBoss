using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BistroBoss.Data.Migrations
{
    /// <inheritdoc />
    public partial class v13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ZamowienieProdukt_Produkty_ProduktId",
                table: "ZamowienieProdukt");

            migrationBuilder.DropForeignKey(
                name: "FK_ZamowienieProdukt_Zamowienia_ZamowienieId",
                table: "ZamowienieProdukt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ZamowienieProdukt",
                table: "ZamowienieProdukt");

            migrationBuilder.RenameTable(
                name: "ZamowienieProdukt",
                newName: "ZamowieniaProdukty");

            migrationBuilder.RenameIndex(
                name: "IX_ZamowienieProdukt_ZamowienieId",
                table: "ZamowieniaProdukty",
                newName: "IX_ZamowieniaProdukty_ZamowienieId");

            migrationBuilder.RenameIndex(
                name: "IX_ZamowienieProdukt_ProduktId",
                table: "ZamowieniaProdukty",
                newName: "IX_ZamowieniaProdukty_ProduktId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ZamowieniaProdukty",
                table: "ZamowieniaProdukty",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ZamowieniaProdukty_Produkty_ProduktId",
                table: "ZamowieniaProdukty",
                column: "ProduktId",
                principalTable: "Produkty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ZamowieniaProdukty_Zamowienia_ZamowienieId",
                table: "ZamowieniaProdukty",
                column: "ZamowienieId",
                principalTable: "Zamowienia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ZamowieniaProdukty_Produkty_ProduktId",
                table: "ZamowieniaProdukty");

            migrationBuilder.DropForeignKey(
                name: "FK_ZamowieniaProdukty_Zamowienia_ZamowienieId",
                table: "ZamowieniaProdukty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ZamowieniaProdukty",
                table: "ZamowieniaProdukty");

            migrationBuilder.RenameTable(
                name: "ZamowieniaProdukty",
                newName: "ZamowienieProdukt");

            migrationBuilder.RenameIndex(
                name: "IX_ZamowieniaProdukty_ZamowienieId",
                table: "ZamowienieProdukt",
                newName: "IX_ZamowienieProdukt_ZamowienieId");

            migrationBuilder.RenameIndex(
                name: "IX_ZamowieniaProdukty_ProduktId",
                table: "ZamowienieProdukt",
                newName: "IX_ZamowienieProdukt_ProduktId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ZamowienieProdukt",
                table: "ZamowienieProdukt",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ZamowienieProdukt_Produkty_ProduktId",
                table: "ZamowienieProdukt",
                column: "ProduktId",
                principalTable: "Produkty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ZamowienieProdukt_Zamowienia_ZamowienieId",
                table: "ZamowienieProdukt",
                column: "ZamowienieId",
                principalTable: "Zamowienia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
