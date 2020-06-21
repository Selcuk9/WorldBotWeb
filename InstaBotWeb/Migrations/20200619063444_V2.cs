using Microsoft.EntityFrameworkCore.Migrations;

namespace InstaBotWeb.Migrations
{
    public partial class V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTelegram_TelegramBots_BotId",
                table: "UserTelegram");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTelegram_DbUsers_UserId",
                table: "UserTelegram");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTelegram",
                table: "UserTelegram");

            migrationBuilder.RenameTable(
                name: "UserTelegram",
                newName: "UserTelegrams");

            migrationBuilder.RenameIndex(
                name: "IX_UserTelegram_BotId",
                table: "UserTelegrams",
                newName: "IX_UserTelegrams_BotId");

            migrationBuilder.AlterColumn<string>(
                name: "UsernameBots",
                table: "TelegramBots",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "TelegramBots",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTelegrams",
                table: "UserTelegrams",
                columns: new[] { "UserId", "BotId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserTelegrams_TelegramBots_BotId",
                table: "UserTelegrams",
                column: "BotId",
                principalTable: "TelegramBots",
                principalColumn: "TokenId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTelegrams_DbUsers_UserId",
                table: "UserTelegrams",
                column: "UserId",
                principalTable: "DbUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTelegrams_TelegramBots_BotId",
                table: "UserTelegrams");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTelegrams_DbUsers_UserId",
                table: "UserTelegrams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTelegrams",
                table: "UserTelegrams");

            migrationBuilder.RenameTable(
                name: "UserTelegrams",
                newName: "UserTelegram");

            migrationBuilder.RenameIndex(
                name: "IX_UserTelegrams_BotId",
                table: "UserTelegram",
                newName: "IX_UserTelegram_BotId");

            migrationBuilder.AlterColumn<string>(
                name: "UsernameBots",
                table: "TelegramBots",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "TelegramBots",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTelegram",
                table: "UserTelegram",
                columns: new[] { "UserId", "BotId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserTelegram_TelegramBots_BotId",
                table: "UserTelegram",
                column: "BotId",
                principalTable: "TelegramBots",
                principalColumn: "TokenId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTelegram_DbUsers_UserId",
                table: "UserTelegram",
                column: "UserId",
                principalTable: "DbUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
