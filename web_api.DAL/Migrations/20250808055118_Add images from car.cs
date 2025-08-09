using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web_api.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Addimagesfromcar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "Images",
                table: "Cars",
                type: "text[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Images",
                table: "Cars");
        }
    }
}
