using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BistroBoss.Data.Migrations
{
    /// <inheritdoc />
    public partial class v6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Koszyki_AspNetUsers_UzytkownikId",
                table: "Koszyki");

            migrationBuilder.DropForeignKey(
                name: "FK_Koszyki_Zamowienia_ZamowienieId",
                table: "Koszyki");

            migrationBuilder.DropTable(
                name: "KoszykProdukt");

            migrationBuilder.DropIndex(
                name: "IX_Koszyki_UzytkownikId",
                table: "Koszyki");

            migrationBuilder.DropIndex(
                name: "IX_Koszyki_ZamowienieId",
                table: "Koszyki");

            migrationBuilder.DropColumn(
                name: "ZamowienieId",
                table: "Koszyki");

            migrationBuilder.AlterColumn<string>(
                name: "UzytkownikId",
                table: "Koszyki",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "KoszykProdukty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KoszykId = table.Column<int>(type: "int", nullable: false),
                    ProduktId = table.Column<int>(type: "int", nullable: false),
                    Ilosc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KoszykProdukty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KoszykProdukty_Koszyki_KoszykId",
                        column: x => x.KoszykId,
                        principalTable: "Koszyki",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KoszykProdukty_Produkty_ProduktId",
                        column: x => x.ProduktId,
                        principalTable: "Produkty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Koszyki_UzytkownikId",
                table: "Koszyki",
                column: "UzytkownikId",
                unique: true,
                filter: "[UzytkownikId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_KoszykProdukty_KoszykId",
                table: "KoszykProdukty",
                column: "KoszykId");

            migrationBuilder.CreateIndex(
                name: "IX_KoszykProdukty_ProduktId",
                table: "KoszykProdukty",
                column: "ProduktId");

            migrationBuilder.AddForeignKey(
                name: "FK_Koszyki_AspNetUsers_UzytkownikId",
                table: "Koszyki",
                column: "UzytkownikId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Koszyki_AspNetUsers_UzytkownikId",
                table: "Koszyki");

            migrationBuilder.DropTable(
                name: "KoszykProdukty");

            migrationBuilder.DropIndex(
                name: "IX_Koszyki_UzytkownikId",
                table: "Koszyki");

            migrationBuilder.AlterColumn<string>(
                name: "UzytkownikId",
                table: "Koszyki",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ZamowienieId",
                table: "Koszyki",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "KoszykProdukt",
                columns: table => new
                {
                    KoszykiId = table.Column<int>(type: "int", nullable: false),
                    ProduktyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KoszykProdukt", x => new { x.KoszykiId, x.ProduktyId });
                    table.ForeignKey(
                        name: "FK_KoszykProdukt_Koszyki_KoszykiId",
                        column: x => x.KoszykiId,
                        principalTable: "Koszyki",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KoszykProdukt_Produkty_ProduktyId",
                        column: x => x.ProduktyId,
                        principalTable: "Produkty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Koszyki_UzytkownikId",
                table: "Koszyki",
                column: "UzytkownikId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Koszyki_ZamowienieId",
                table: "Koszyki",
                column: "ZamowienieId");

            migrationBuilder.CreateIndex(
                name: "IX_KoszykProdukt_ProduktyId",
                table: "KoszykProdukt",
                column: "ProduktyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Koszyki_AspNetUsers_UzytkownikId",
                table: "Koszyki",
                column: "UzytkownikId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Koszyki_Zamowienia_ZamowienieId",
                table: "Koszyki",
                column: "ZamowienieId",
                principalTable: "Zamowienia",
                principalColumn: "Id");
        }
    }
}
