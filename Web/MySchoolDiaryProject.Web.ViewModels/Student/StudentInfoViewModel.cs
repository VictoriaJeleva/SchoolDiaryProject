namespace MySchoolDiaryProject.Web.ViewModels.Student
{
    using System.Collections.Generic;

    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Mapping;
    using MySchoolDiaryProject.Web.ViewModels.Attendance;

    public class StudentInfoViewModel : IMapFrom<Student>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string BirthDate { get; set; }

        public string Gender { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string PhotoPath { get; set; }

        public string DateOfFirstDayInSchool { get; set; }

        public int CourseId { get; set; }

        public virtual Course Courses { get; set; }

        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public IEnumerable<StudentAttendancesViewModel> StudentAttendances { get; set; }
    }
}
