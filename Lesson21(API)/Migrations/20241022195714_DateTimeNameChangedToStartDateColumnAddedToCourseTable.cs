using Microsoft.EntityFrameworkCore.Migrations;

namespace Lesson21_API_.Migrations
{
    public partial class DateTimeNameChangedToStartDateColumnAddedToCourseTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "Courses",
                newName: "StartDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Courses",
                newName: "DateTime");
        }
    }
}
