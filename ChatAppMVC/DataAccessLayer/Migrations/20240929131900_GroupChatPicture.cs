using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAppMVC.Migrations
{
    /// <inheritdoc />
    public partial class GroupChatPicture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChatPictureUrl",
                table: "Chats",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatPictureUrl",
                table: "Chats");
        }
    }
}
