using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAppMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddUserChatPicture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChatPictureUrl",
                table: "ChatUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatPictureUrl",
                table: "ChatUsers");
        }
    }
}
