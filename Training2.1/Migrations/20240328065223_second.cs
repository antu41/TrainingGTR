using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training2._1.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Departments_DptId1",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_DptId1",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "DptId1",
                table: "Teachers");

            migrationBuilder.RenameColumn(
                name: "DptId",
                table: "Teachers",
                newName: "Designation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Designation",
                table: "Teachers",
                newName: "DptId");

            migrationBuilder.AddColumn<int>(
                name: "DptId1",
                table: "Teachers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_DptId1",
                table: "Teachers",
                column: "DptId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Departments_DptId1",
                table: "Teachers",
                column: "DptId1",
                principalTable: "Departments",
                principalColumn: "Id");
        }
    }
}
