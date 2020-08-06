using Microsoft.EntityFrameworkCore.Migrations;

namespace MySchoolDiaryProject.Data.Migrations
{
    public partial class DbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseSubjectTeacher_Courses_CourseId",
                table: "CourseSubjectTeacher");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseSubjectTeacher_Subjects_SubjectId",
                table: "CourseSubjectTeacher");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseSubjectTeacher_Teachers_TeacherId",
                table: "CourseSubjectTeacher");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseSubjectTeacher",
                table: "CourseSubjectTeacher");

            migrationBuilder.RenameTable(
                name: "CourseSubjectTeacher",
                newName: "CourseSubjectTeachers");

            migrationBuilder.RenameIndex(
                name: "IX_CourseSubjectTeacher_TeacherId",
                table: "CourseSubjectTeachers",
                newName: "IX_CourseSubjectTeachers_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseSubjectTeacher_SubjectId",
                table: "CourseSubjectTeachers",
                newName: "IX_CourseSubjectTeachers_SubjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseSubjectTeachers",
                table: "CourseSubjectTeachers",
                columns: new[] { "CourseId", "SubjectId", "TeacherId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSubjectTeachers_Courses_CourseId",
                table: "CourseSubjectTeachers",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSubjectTeachers_Subjects_SubjectId",
                table: "CourseSubjectTeachers",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSubjectTeachers_Teachers_TeacherId",
                table: "CourseSubjectTeachers",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseSubjectTeachers_Courses_CourseId",
                table: "CourseSubjectTeachers");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseSubjectTeachers_Subjects_SubjectId",
                table: "CourseSubjectTeachers");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseSubjectTeachers_Teachers_TeacherId",
                table: "CourseSubjectTeachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseSubjectTeachers",
                table: "CourseSubjectTeachers");

            migrationBuilder.RenameTable(
                name: "CourseSubjectTeachers",
                newName: "CourseSubjectTeacher");

            migrationBuilder.RenameIndex(
                name: "IX_CourseSubjectTeachers_TeacherId",
                table: "CourseSubjectTeacher",
                newName: "IX_CourseSubjectTeacher_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseSubjectTeachers_SubjectId",
                table: "CourseSubjectTeacher",
                newName: "IX_CourseSubjectTeacher_SubjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseSubjectTeacher",
                table: "CourseSubjectTeacher",
                columns: new[] { "CourseId", "SubjectId", "TeacherId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSubjectTeacher_Courses_CourseId",
                table: "CourseSubjectTeacher",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSubjectTeacher_Subjects_SubjectId",
                table: "CourseSubjectTeacher",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSubjectTeacher_Teachers_TeacherId",
                table: "CourseSubjectTeacher",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
