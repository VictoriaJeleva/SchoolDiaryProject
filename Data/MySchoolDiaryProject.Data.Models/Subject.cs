namespace MySchoolDiaryProject.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MySchoolDiaryProject.Data.Common.Models;

    public class Subject : BaseDeletableModel<int>
    {
        public Subject()
        {
            this.Courses = new HashSet<CourseSubjects>();
            //this.Students = new HashSet<StudentSubjects>();
            this.SubjectTeachers = new HashSet<SubjectTeachers>();
        }

        public Subject(string name)
        {
            this.Name = name;
        }


        [Required]
        public string Name { get; set; }

        public virtual ICollection<CourseSubjects> Courses { get; set; }

        //public virtual ICollection<StudentSubjects> Students { get; set; }

        public virtual ICollection<SubjectTeachers> SubjectTeachers { get; set; }
    }
}
