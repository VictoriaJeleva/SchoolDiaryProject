namespace MySchoolDiaryProject.Web.ViewModels.Subjects
{
    using System.Collections.Generic;

    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Mapping;
    using MySchoolDiaryProject.Web.ViewModels.SubjectTeachers;

    public class SubjectInfoViewModel : IMapFrom<Subject>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int CourseIdd { get; set; }

        public virtual ICollection<SubjectTeacherViewModel> SubjectTeachers { get; set; }
    }
}
