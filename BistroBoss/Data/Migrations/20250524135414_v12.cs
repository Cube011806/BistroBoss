using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BistroBoss.Data.Migrations
{
    /// <inheritdoc />
    public partial class v12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Koszyki_Zamowienia_ZamowienieId",
                table: "Koszyki");

            migrationBuilder.DropIndex(
                name: "IX_Koszyki_ZamowienieId",
                table: "Koszyki");

            migrationBuilder.DropColumn(
                name: "KoszykId",
                table: "Zamowienia");

            migrationBuilder.CreateTable(
                name: "ZamowienieProdukt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZamowienieId = table.Column<int>(type: "int", nullable: false),
                    ProduktId = table.Column<int>(type: "int", nullable: false),
                    Ilosc = table.Column<int>(type: "int", nullable: false),
                    Cena = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZamowienieProdukt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZamowienieProdukt_Produkty_ProduktId",
                        column: x => x.ProduktId,
                        principalTable: "Produkty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ZamowienieProdukt_Zamowienia_ZamowienieId",
                        column: x => x.ZamowienieId,
                        principalTable: "Zamowienia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Koszyki_ZamowienieId",
                table: "Koszyki",
                column: "ZamowienieId");

            migrationBuilder.CreateIndex(
                name: "IX_ZamowienieProdukt_ProduktId",
                table: "ZamowienieProdukt",
                column: "ProduktId");

            migrationBuilder.CreateIndex(
                name: "IX_ZamowienieProdukt_ZamowienieId",
                table: "ZamowienieProdukt",
                column: "ZamowienieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Koszyki_Zamowienia_ZamowienieId",
                table: "Koszyki",
                column: "ZamowienieId",
                principalTable: "Zamowienia",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Koszyki_Zamowienia_ZamowienieId",
                table: "Koszyki");

            migrationBuilder.DropTable(
                name: "ZamowienieProdukt");

            migrationBuilder.DropIndex(
                name: "IX_Koszyki_ZamowienieId",
                table: "Koszyki");

            migrationBuilder.AddColumn<int>(
                name: "KoszykId",
                table: "Zamowienia",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Koszyki_ZamowienieId",
                table: "Koszyki",
                column: "ZamowienieId",
                unique: true,
                filter: "[ZamowienieId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Koszyki_Zamowienia_ZamowienieId",
                table: "Koszyki",
                column: "ZamowienieId",
                principalTable: "Zamowienia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
