using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pioneer.Migrations
{
    /// <inheritdoc />
    public partial class AssignAdminUserToAllRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [security].[UserRoles] (UserId, RoleId) SELECT '101c7942-8c74-419a-9467-c07de4138c76', Id FROM [security].[Roles]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [security].[UserRoles] WHERE UserId = '101c7942-8c74-419a-9467-c07de4138c76'");
        }
    }
}
