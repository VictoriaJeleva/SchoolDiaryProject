namespace MySchoolDiaryProject.Web.ViewModels.Course
{
    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Mapping;

    public class StudentCourseViewModel : IMapFrom<Student>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }
    }
}
