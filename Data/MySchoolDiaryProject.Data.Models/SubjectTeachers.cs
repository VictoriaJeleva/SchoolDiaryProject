namespace MySchoolDiaryProject.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class SubjectTeachers
    {
        public SubjectTeachers()
        {

        }

        public SubjectTeachers(Subject subject, Teacher teacher)
        {
            this.SubjectId = subject.Id;
            this.Subject = subject;
            this.TeacherId = teacher.Id;
            this.Teacher = teacher;
        }

        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }

        public int TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }

        public string SubjectTeacher => $"{this.Subject.Name} - {this.Teacher.FullName}";
    }
}
