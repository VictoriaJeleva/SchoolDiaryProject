namespace MySchoolDiaryProject.Web.ViewModels.Course
{
    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Mapping;

    public class CourseSubjectsViewModel : IMapFrom<CourseSubjectTeacher>
    {
        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }

        public int TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }
    }

    //public class CourseSubjectsViewModel : IMapFrom<CourseSubjects>
    //{
    //    public int CourseId { get; set; }

    //    public virtual Course Course { get; set; }

    //    public int SubjectId { get; set; }

    //    public virtual Subject Subject { get; set; }
    //}
}
