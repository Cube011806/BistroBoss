using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BistroBoss.Data.Migrations
{
    /// <inheritdoc />
    public partial class v10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Koszyki_ZamowienieId",
                table: "Koszyki");

            migrationBuilder.AlterColumn<int>(
                name: "ZamowienieId",
                table: "Koszyki",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Koszyki_ZamowienieId",
                table: "Koszyki",
                column: "ZamowienieId",
                unique: true,
                filter: "[ZamowienieId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Koszyki_ZamowienieId",
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

            migrationBuilder.CreateIndex(
                name: "IX_Koszyki_ZamowienieId",
                table: "Koszyki",
                column: "ZamowienieId",
                unique: true);
        }
    }
}
