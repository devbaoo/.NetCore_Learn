using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoApp.Migrations
{
    /// <inheritdoc />
    public partial class CreateSchool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SId",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "School",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_School", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_SId",
                table: "Students",
                column: "SId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_School_SId",
                table: "Students",
                column: "SId",
                principalTable: "School",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_School_SId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "School");

            migrationBuilder.DropIndex(
                name: "IX_Students_SId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "SId",
                table: "Students");
        }
    }
}
