namespace MySchoolDiaryProject.Data.Models
{
    public class CourseSubjects
    {
        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }
    }
}
