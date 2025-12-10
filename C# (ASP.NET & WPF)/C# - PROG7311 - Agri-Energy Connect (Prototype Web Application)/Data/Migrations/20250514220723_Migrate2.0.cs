using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prog7311_POE_Part2.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migrate20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserID",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserID",
                table: "Products");
        }
    }
}
