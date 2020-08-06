namespace MySchoolDiaryProject.Web.ViewModels.Subjects
{
    using System.Collections.Generic;

    public class ListAllSubjectsViewModel
    {
        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public IEnumerable<SubjectInfoViewModel> Subjects { get; set; }
    }
}
