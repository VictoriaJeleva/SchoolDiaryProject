namespace MySchoolDiaryProject.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MySchoolDiaryProject.Data.Common.Models;

    public class Student : BaseDeletableModel<int>
    {
        public Student()
        {
            this.Subjects = new HashSet<StudentSubjects>();
            this.SubjectsNames = new HashSet<Subject>();
            this.Grades = new HashSet<Grade>();
            this.Attendances = new HashSet<Attendance>();
        }

        public Student(string name, string middleName, string lastName, string birthday, string gender, string phone, string address, Course course)
        {
            this.Name = name;
            this.MiddleName = middleName;
            this.LastName = lastName;
            this.BirthDate = birthday;
            this.Gender = gender;
            this.Phone = phone;
            this.Address = address;
            this.CourseId = course.Id;
            this.Course = course;
        }

        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        public string MiddleName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        public string LastName { get; set; }

        [Required]
        public string BirthDate { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [MaxLength(10)]
        [RegularExpression("08[0-9]{8}")]
        public string Phone { get; set; }

        [Required]
        [MinLength(6)]
        [RegularExpression("[A-Z a-z 0-9 ]*")]
        public string Address { get; set; }

        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

        public string PhotoPath { get; set; }

        //public int ImageId { get; set; }

        //public virtual Image Image { get; set; }

        public virtual ICollection<StudentSubjects> Subjects { get; set; }

        public virtual ICollection<Subject> SubjectsNames { get; set; }

        public virtual ICollection<Grade> Grades { get; set; }

        public virtual ICollection<Attendance> Attendances { get; set; }
    }
}
