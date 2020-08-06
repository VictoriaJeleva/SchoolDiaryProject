namespace MySchoolDiaryProject.Web.ViewModels.Subjects
{
    using System.Collections.Generic;

    using MySchoolDiaryProject.Data.Models;

    public class SubjectsInputViewModel
    {
        public int SubjectId { get; set; }

        public Subject Subject { get; set; }

        public int TeacherId { get; set; }

        public Teacher Teacher { get; set; }

        public string SubjectTeacher { get; set; }

        public IEnumerable<SubjectTeachersDropDownListViewModel> SubjectTeachers { get; set; }
    }
}
