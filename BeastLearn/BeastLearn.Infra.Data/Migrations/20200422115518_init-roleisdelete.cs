using Microsoft.EntityFrameworkCore.Migrations;

namespace BeastLearn.Infra.Data.Migrations
{
    public partial class initroleisdelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Roles",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Roles");
        }
    }
}
