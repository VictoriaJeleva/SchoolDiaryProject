namespace MySchoolDiaryProject.Web.ViewModels.Student
{
    using System.Collections.Generic;

    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Mapping;
    using MySchoolDiaryProject.Web.ViewModels.Grade;

    public class StudentInfoGradeViewModel : IMapFrom<Student>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string BirthDate { get; set; }

        public string Gender { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string DateOfFirstDayInSchool { get; set; }

        public int CourseId { get; set; }

        public IEnumerable<GradeViewModel> Grades { get; set; }

        public IEnumerable<StudentSubjects> Subjects { get; set; }

        public List<Subject> SubjectsNames { get; set; }
    }
}
