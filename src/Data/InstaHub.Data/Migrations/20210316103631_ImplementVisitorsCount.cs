namespace InstaHub.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ImplementVisitorsCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VisitorsCount",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisitorsCount",
                table: "Posts");
        }
    }
}
