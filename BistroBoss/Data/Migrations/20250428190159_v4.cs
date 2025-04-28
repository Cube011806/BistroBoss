using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BistroBoss.Data.Migrations
{
    /// <inheritdoc />
    public partial class v4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Koszyki_Zamowienia_ZamowienieId",
                table: "Koszyki");

            migrationBuilder.AlterColumn<int>(
                name: "ZamowienieId",
                table: "Koszyki",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.AlterColumn<int>(
                name: "ZamowienieId",
                table: "Koszyki",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Koszyki_Zamowienia_ZamowienieId",
                table: "Koszyki",
                column: "ZamowienieId",
                principalTable: "Zamowienia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
