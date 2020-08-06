namespace MySchoolDiaryProject.Web.ViewModels.Student
{
    using System.Collections.Generic;

    public class ListStudentsViewModel
    {
        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public IEnumerable<AllInfoAboutStudentViewModel> Students { get; set; }
    }
}
