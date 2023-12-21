using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pioneer.Migrations
{
    /// <inheritdoc />
    public partial class addadminuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [security].[Users] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [ProfilePicture]) VALUES (N'8ac5f263-becd-453e-8b71-770805ee9a40', N'admin', N'ADMIN', N'admin@test.com', N'ADMIN@TEST.COM', 0, N'AQAAAAIAAYagAAAAEJp4sqnuyEqsGW9LKT02yNGujR6tFW9TforPBpayIDIdRd96adOFzpD/F4wQDgaEJg==', N'BJ2QW3AD42IWRKPCEPSDPCYJAGSHQDLC', N'b5b46501-0c77-45cc-ab1f-f845b60a6259', NULL, 0, 0, NULL, 1, 0, N'ahmed', N'fouad', null)\r\n");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [security].[Users] WHERE Id = '101c7942-8c74-419a-9467-c07de4138c76'");


        }
    }
}
