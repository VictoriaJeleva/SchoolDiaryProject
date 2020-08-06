namespace MySchoolDiaryProject.Web.ViewModels.Teachers
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Mapping;
    using MySchoolDiaryProject.Web.ViewModels.Course;

    public class DeleteChangeClassTeacherViewModel : IMapFrom<Teacher>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public int TeacherId { get; set; }

        public IEnumerable<CoursesViewModel> Courses { get; set; }

        public IEnumerable<TeachersDropDownViewModel> Teachers { get; set; }
    }
}
