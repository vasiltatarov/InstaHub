using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyForum.Data.Migrations
{
    public partial class AddModelUserSavedPosts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserSavedPostId",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserSavedPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSavedPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSavedPosts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserSavedPostId",
                table: "Posts",
                column: "UserSavedPostId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSavedPosts_IsDeleted",
                table: "UserSavedPosts",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_UserSavedPosts_UserId",
                table: "UserSavedPosts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_UserSavedPosts_UserSavedPostId",
                table: "Posts",
                column: "UserSavedPostId",
                principalTable: "UserSavedPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_UserSavedPosts_UserSavedPostId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "UserSavedPosts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_UserSavedPostId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "UserSavedPostId",
                table: "Posts");
        }
    }
}
