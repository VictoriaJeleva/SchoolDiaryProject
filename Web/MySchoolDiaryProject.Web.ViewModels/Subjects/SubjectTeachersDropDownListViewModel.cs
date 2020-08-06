namespace MySchoolDiaryProject.Web.ViewModels.Subjects
{
    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Mapping;

    public class SubjectTeachersDropDownListViewModel : IMapFrom<SubjectTeachers>
    {
        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }

        public int TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }

        public string SubjectTeacher => $"{this.Subject.Name} - {this.Teacher.FullName}";
    }
}
