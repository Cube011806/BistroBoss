using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BistroBoss.Data.Migrations
{
    /// <inheritdoc />
    public partial class v89 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Opinie_ZamowienieId",
                table: "Opinie");

            migrationBuilder.CreateIndex(
                name: "IX_Opinie_ZamowienieId",
                table: "Opinie",
                column: "ZamowienieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Opinie_ZamowienieId",
                table: "Opinie");

            migrationBuilder.CreateIndex(
                name: "IX_Opinie_ZamowienieId",
                table: "Opinie",
                column: "ZamowienieId",
                unique: true);
        }
    }
}
