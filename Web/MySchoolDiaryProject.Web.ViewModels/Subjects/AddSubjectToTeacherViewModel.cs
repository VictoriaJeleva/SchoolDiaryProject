using System;
using System.Collections.Generic;
using System.Text;

namespace MySchoolDiaryProject.Web.ViewModels.Subjects
{
    public class AddSubjectToTeacherViewModel
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<SubjectDropDownListViewModel> Subjects { get; set; }
    }
}
