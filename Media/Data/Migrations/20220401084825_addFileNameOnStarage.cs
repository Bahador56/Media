using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Media.Data.Migrations
{
    public partial class addFileNameOnStarage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileNameOnStarage",
                table: "Files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileNameOnStarage",
                table: "Files");
        }
    }
}
