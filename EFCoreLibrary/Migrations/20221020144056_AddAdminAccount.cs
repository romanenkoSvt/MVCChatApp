using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using EFCoreLibrary.Models;
using System.Text;

#nullable disable

namespace EFCoreLibrary.Migrations
{
    public partial class AddAdminAccount : Migration
    {
        private const string ADMIN_USER_GUID = "4b8ed887-92d7-478b-9fbb-c9d7ca3a476f";
        private const string ADMIN_ROLE_GUID = "8e135b04-bd14-440d-bcf9-3d374be0705e";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            var passwordHash = hasher.HashPassword(null, "!Admin123");

            StringBuilder sb = new();
            sb.AppendLine("INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, " +
                "ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, ChatName, FirstName, IsBanned, LastName)");
            sb.AppendLine($"VALUES ('{ADMIN_USER_GUID}', 'admin@chatapp.com', 'ADMIN@CHATAPP.COM', 'admin@chatapp.com', 'ADMIN@CHATAPP.COM', 0, '{passwordHash}', ''," +
                $" '', 0, 0, 0, 0, 'Admin', 'Admin', 0, 'Admin')");

            migrationBuilder.Sql(sb.ToString());
            migrationBuilder.Sql($"INSERT INTO AspNetRoles(Id, Name, NormalizedName) VALUES ('{ADMIN_ROLE_GUID}', 'Administrator', 'ADMINISTRATOR')");
            migrationBuilder.Sql($"INSERT INTO AspNetUserRoles(UserId, RoleId) VALUES ('{ADMIN_USER_GUID}', '{ADMIN_ROLE_GUID}')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM AspNetUserRoles WHERE UserId = '{ADMIN_USER_GUID}';");
            migrationBuilder.Sql($"DELETE FROM AspNetRoles WHERE Id = '{ADMIN_ROLE_GUID}';");
            migrationBuilder.Sql($"DELETE FROM AspNetUsers WHERE Id = '{ADMIN_USER_GUID}';");
        }
    }
}
