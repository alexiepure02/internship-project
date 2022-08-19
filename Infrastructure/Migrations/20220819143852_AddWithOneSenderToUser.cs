using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddWithOneSenderToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_UserID",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_UserID",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Messages");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_IDReceiver",
                table: "Messages",
                column: "IDReceiver");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_IDSender",
                table: "Messages",
                column: "IDSender");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_IDReceiver",
                table: "Messages",
                column: "IDReceiver",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_IDSender",
                table: "Messages",
                column: "IDSender",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_IDReceiver",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_IDSender",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_IDReceiver",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_IDSender",
                table: "Messages");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserID",
                table: "Messages",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_UserID",
                table: "Messages",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID");
        }
    }
}
