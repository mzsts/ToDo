using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Web.Migrations
{
    public partial class AddedIsSubItemProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ToDoItems",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<bool>(
                name: "IsSubItem",
                table: "ToDoItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSubItem",
                table: "ToDoItems");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ToDoItems",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40);
        }
    }
}
