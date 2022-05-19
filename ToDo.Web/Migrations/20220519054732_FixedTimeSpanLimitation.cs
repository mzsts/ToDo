using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Web.Migrations
{
    public partial class FixedTimeSpanLimitation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletionTime",
                table: "ToDoItems");

            migrationBuilder.DropColumn(
                name: "EstimationTime",
                table: "ToDoItems");

            migrationBuilder.AddColumn<long>(
                name: "CompletionTimePeriodTicks",
                table: "ToDoItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "EstimationTimePeriodTicks",
                table: "ToDoItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletionTimePeriodTicks",
                table: "ToDoItems");

            migrationBuilder.DropColumn(
                name: "EstimationTimePeriodTicks",
                table: "ToDoItems");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "CompletionTime",
                table: "ToDoItems",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EstimationTime",
                table: "ToDoItems",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
