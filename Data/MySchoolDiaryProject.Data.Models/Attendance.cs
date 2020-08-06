namespace MySchoolDiaryProject.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using MySchoolDiaryProject.Data.Common.Models;

    public class Attendance : BaseModel<int>
    {
        public Attendance()
        {
        }

        public Attendance(Subject subject, Student student, DateTime dateOfAbsense, string remark)
        {
            this.Subject = subject;
            this.SubjectId = subject.Id;
            this.Student = student;
            this.StudentId = student.Id;
            this.DateOfAbsense = dateOfAbsense;
            this.Remark = remark;
        }

        [Required]
        public int StudentId { get; set; }

        public virtual Student Student { get; set; }

        public int? SubjectId { get; set; }

        public virtual Subject Subject { get; set; }

        [Required]
        public DateTime DateOfAbsense { get; set; }

        public string Remark { get; set; }
    }
}
