namespace MySchoolDiaryProject.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using MySchoolDiaryProject.Data.Common.Models;

    public class Grade : BaseModel<int>
    {
        [Required]
        public double Value { get; set; }

        public int StudentId { get; set; }

        public virtual Student Student { get; set; }

        public int? SubjectId { get; set; }

        public virtual Subject Subject { get; set; }

        public string SubjectName { get; set; }
    }
}
