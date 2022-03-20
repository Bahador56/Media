using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Media.Data.Migrations
{
    public partial class indexunique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Folders_ParentId",
                table: "Folders");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Folders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_ParentId_Title",
                table: "Folders",
                columns: new[] { "ParentId", "Title" },
                unique: true,
                filter: "[ParentId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Folders_ParentId_Title",
                table: "Folders");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Folders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_ParentId",
                table: "Folders",
                column: "ParentId");
        }
    }
}
