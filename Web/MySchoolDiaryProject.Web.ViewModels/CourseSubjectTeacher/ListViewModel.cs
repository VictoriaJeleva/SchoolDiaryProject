namespace MySchoolDiaryProject.Web.ViewModels.CourseSubjectTeacher
{
    using MySchoolDiaryProject.Data.Models;
    using System.Collections.Generic;

    public class ListViewModel
    {
        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public Teacher Teacher { get; set; }

        public IEnumerable<CourseSubjectTeachersViewModel> CourseSubjectTeachers { get; set; }
    }
}
