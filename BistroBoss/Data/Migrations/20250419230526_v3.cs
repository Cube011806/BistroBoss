using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BistroBoss.Data.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ZamowieniaProdukty");

            migrationBuilder.AddColumn<int>(
                name: "KoszykId",
                table: "Zamowienia",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Koszyki",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZamowienieId = table.Column<int>(type: "int", nullable: false),
                    UzytkownikId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Koszyki", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Koszyki_AspNetUsers_UzytkownikId",
                        column: x => x.UzytkownikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Koszyki_Zamowienia_ZamowienieId",
                        column: x => x.ZamowienieId,
                        principalTable: "Zamowienia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_Zamowienia_KoszykId",
                table: "Zamowienia",
                column: "KoszykId",
                unique: true);

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
                name: "FK_Zamowienia_Koszyki_KoszykId",
                table: "Zamowienia",
                column: "KoszykId",
                principalTable: "Koszyki",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zamowienia_Koszyki_KoszykId",
                table: "Zamowienia");

            migrationBuilder.DropTable(
                name: "KoszykProdukt");

            migrationBuilder.DropTable(
                name: "Koszyki");

            migrationBuilder.DropIndex(
                name: "IX_Zamowienia_KoszykId",
                table: "Zamowienia");

            migrationBuilder.DropColumn(
                name: "KoszykId",
                table: "Zamowienia");

            migrationBuilder.CreateTable(
                name: "ZamowieniaProdukty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProduktId = table.Column<int>(type: "int", nullable: false),
                    ZamowienieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZamowieniaProdukty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZamowieniaProdukty_Produkty_ProduktId",
                        column: x => x.ProduktId,
                        principalTable: "Produkty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ZamowieniaProdukty_Zamowienia_ZamowienieId",
                        column: x => x.ZamowienieId,
                        principalTable: "Zamowienia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ZamowieniaProdukty_ProduktId",
                table: "ZamowieniaProdukty",
                column: "ProduktId");

            migrationBuilder.CreateIndex(
                name: "IX_ZamowieniaProdukty_ZamowienieId",
                table: "ZamowieniaProdukty",
                column: "ZamowienieId");
        }
    }
}
