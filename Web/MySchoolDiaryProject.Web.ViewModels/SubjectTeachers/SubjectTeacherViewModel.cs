namespace MySchoolDiaryProject.Web.ViewModels.SubjectTeachers
{
    using MySchoolDiaryProject.Services.Mapping;

    using MySchoolDiaryProject.Data.Models;

    public class SubjectTeacherViewModel : IMapFrom<SubjectTeachers>
    {
        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }

        public int TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }
    }
}
