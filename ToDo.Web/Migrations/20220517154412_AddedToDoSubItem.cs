using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Web.Migrations
{
    public partial class AddedToDoSubItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSubItem",
                table: "ToDoItems");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "ToDoItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "ToDoItems");

            migrationBuilder.AddColumn<bool>(
                name: "IsSubItem",
                table: "ToDoItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
