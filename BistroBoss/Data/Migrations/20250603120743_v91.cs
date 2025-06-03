using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BistroBoss.Data.Migrations
{
    /// <inheritdoc />
    public partial class v91 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Koszyki_AspNetUsers_UzytkownikId",
                table: "Koszyki");

            migrationBuilder.AddForeignKey(
                name: "FK_Koszyki_AspNetUsers_UzytkownikId",
                table: "Koszyki",
                column: "UzytkownikId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Koszyki_AspNetUsers_UzytkownikId",
                table: "Koszyki");

            migrationBuilder.AddForeignKey(
                name: "FK_Koszyki_AspNetUsers_UzytkownikId",
                table: "Koszyki",
                column: "UzytkownikId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
