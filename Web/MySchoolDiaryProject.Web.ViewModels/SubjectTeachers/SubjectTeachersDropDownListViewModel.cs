
namespace MySchoolDiaryProject.Web.ViewModels.SubjectTeachers
{
    using System.Collections.Generic;

    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Mapping;

    public class SubjectTeachersDropDownListViewModel : IMapFrom<SubjectTeachers>
    {
        public IEnumerable<SubjectTeacherViewModel> SubjectTeachers { get; set; }

        //public int SubjectId { get; set; }

        //public Subject Subject { get; set; }

        //public int TeacherId { get; set; }

        //public Teacher Teacher { get; set; }
    }
}
