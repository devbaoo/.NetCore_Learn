using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoApp.Migrations
{
    /// <inheritdoc />
    public partial class addDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AssignmentScore",
                table: "CourseStudent",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FinalScore",
                table: "CourseStudent",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PracticalScore",
                table: "CourseStudent",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignmentScore",
                table: "CourseStudent");

            migrationBuilder.DropColumn(
                name: "FinalScore",
                table: "CourseStudent");

            migrationBuilder.DropColumn(
                name: "PracticalScore",
                table: "CourseStudent");
        }
    }
}
