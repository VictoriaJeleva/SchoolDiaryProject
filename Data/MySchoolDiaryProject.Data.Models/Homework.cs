namespace MySchoolDiaryProject.Data.Models
{
    using MySchoolDiaryProject.Data.Common.Models;

    public class Homework : BaseDeletableModel<int>
    {
        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

        public int GradeId { get; set; }

        public virtual Grade Grade { get; set; }

        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }
    }
}
