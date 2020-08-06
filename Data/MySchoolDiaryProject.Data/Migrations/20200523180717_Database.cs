using Microsoft.EntityFrameworkCore.Migrations;

namespace MySchoolDiaryProject.Data.Migrations
{
    public partial class Database : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "SubjectTeachers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeachers_CourseId",
                table: "SubjectTeachers",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectTeachers_Courses_CourseId",
                table: "SubjectTeachers",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectTeachers_Courses_CourseId",
                table: "SubjectTeachers");

            migrationBuilder.DropIndex(
                name: "IX_SubjectTeachers_CourseId",
                table: "SubjectTeachers");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "SubjectTeachers");
        }
    }
}
