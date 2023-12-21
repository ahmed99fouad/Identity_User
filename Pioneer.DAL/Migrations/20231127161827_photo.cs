using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pioneer.Migrations
{
    /// <inheritdoc />
    public partial class photo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "NationalPicture",
                schema: "security",
                table: "Users",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PresonalPicture",
                schema: "security",
                table: "Users",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NationalPicture",
                schema: "security",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PresonalPicture",
                schema: "security",
                table: "Users");
        }
    }
}
