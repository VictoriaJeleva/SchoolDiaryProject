namespace MySchoolDiaryProject.Web.ViewModels.Teachers
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ListTeachersViewModel
    {
        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public IEnumerable<TeacherInfoViewModel> Teachers { get; set; }
    }
}
