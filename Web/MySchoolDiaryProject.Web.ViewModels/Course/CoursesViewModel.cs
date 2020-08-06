namespace MySchoolDiaryProject.Web.ViewModels.Course
{
    using System.Collections.Generic;

    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Mapping;

    public class CoursesViewModel : IMapFrom<Course>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ModifiedOn { get; set; }

        public string CreatedOn { get; set; }

        public string Description { get; set; }

        public int TeacherId { get; set; }

        public string Teacher { get; set; }

        public IEnumerable<StudentCourseViewModel> Students { get; set; }

        public IEnumerable<CourseSubjectsViewModel> CourseSubjectTeachers{ get; set; }

        public List<string> SubjectsNames { get; set; }

        public string UrlSeeStudentsGrades => $"/Student/StudentInfoGrades/{this.Id}";
    }
}
