#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Mapingway.Infrastructure.Persistence.Migrations.PostgresqlMigrations
{
    /// <inheritdoc />
    public partial class MultipleRefreshTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens");

            migrationBuilder.DeleteData(
                table: "RefreshTokenFamilies",
                keyColumn: "Id",
                keyValue: -1L);

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: ["RoleId", "UserId"],
                keyValues: [2, -1L]);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: -1L);

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RefreshTokens");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: ["Id", "Created", "Email", "FirstName", "LastName", "PasswordHash", "PasswordSalt", "Updated"],
                values: [1L, null, "admin.map@rambler.ru", "Admin", "Super", "ODrNkGKssc+CWOvKQhJAQQNMocAsUaJ73pBaIfIufy4=", "u4ya35ZFIvfkqC+ObHlNFQ==", null
                ]);

            migrationBuilder.InsertData(
                table: "RefreshTokenFamilies",
                columns: ["Id", "UserId"],
                values: [1L, 1L]);

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: ["RoleId", "UserId"],
                values: [2, 1L]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RefreshTokenFamilies",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: ["RoleId", "UserId"],
                keyValues: [2, 1L]);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "RefreshTokens",
                type: "bigint",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: ["Id", "Created", "Email", "FirstName", "LastName", "PasswordHash", "PasswordSalt", "Updated"],
                values: [-1L, null, "admin.map@rambler.ru", "Admin", "Super", "ODrNkGKssc+CWOvKQhJAQQNMocAsUaJ73pBaIfIufy4=", "u4ya35ZFIvfkqC+ObHlNFQ==", null
                ]);

            migrationBuilder.InsertData(
                table: "RefreshTokenFamilies",
                columns: ["Id", "UserId"],
                values: [-1L, -1L]);

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: ["RoleId", "UserId"],
                values: [2, -1L]);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
