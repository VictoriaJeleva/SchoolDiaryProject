namespace MySchoolDiaryProject.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MySchoolDiaryProject.Data.Common.Models;

    public class Course : BaseDeletableModel<int>
    {
        public Course()
        {
            this.Students = new HashSet<Student>();
            //this.Subjects = new HashSet<CourseSubjects>();
            this.SubjectNames = new HashSet<Subject>();
            //this.SubjectTeachers = new HashSet<SubjectTeachers>();
            this.CourseSubjectTeachers = new HashSet<CourseSubjectTeacher>();
        }

        public Course(string name, string description, Teacher teacher)
        {
            this.Name = name;
            this.Description = description;
            this.TeacherId = teacher.Id;
            this.Teacher = teacher;
        }

        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        public string Name { get; set; }

        [MaxLength(40)]
        public string Description { get; set; }

        //[Required]
        //public int SubjectId { get; set; }

        //public virtual Subject Subject { get; set; }

        public int TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }

        public virtual ICollection<Student> Students { get; set; }

        //ublic virtual ICollection<CourseSubjects> Subjects { get; set; }

        public virtual ICollection<Subject> SubjectNames { get; set; }

        //public virtual ICollection<SubjectTeachers> SubjectTeachers { get; set; }

        public virtual ICollection<CourseSubjectTeacher> CourseSubjectTeachers { get; set; }
    }
}
