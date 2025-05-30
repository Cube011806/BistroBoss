using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BistroBoss.Data.Migrations
{
    /// <inheritdoc />
    public partial class sposob_dostawy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SposobDostawy",
                table: "Zamowienia",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SposobDostawy",
                table: "Zamowienia");
        }
    }
}
