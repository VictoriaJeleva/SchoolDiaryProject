namespace MySchoolDiaryProject.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MySchoolDiaryProject.Data.Common.Models;

    public class Teacher : BaseDeletableModel<int>
    {
        public Teacher()
        {
            this.SubjectTeachers = new HashSet<SubjectTeachers>();
        }

        public Teacher(string name, string middleName, string lastName, string birthday, string gender, string phone, string address)
        {
            this.Name = name;
            this.MiddleName = middleName;
            this.LastName = lastName;
            this.BirthDate = birthday;
            this.Gender = gender;
            this.Phone = phone;
            this.Address = address;
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

        public string FullName => $"{this.Name} {this.MiddleName} {this.LastName}";

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

        //public int SubjectId { get; set; }

        //public virtual Subject Subject { get; set; }

        public virtual ICollection<SubjectTeachers> SubjectTeachers { get; set; }
    }
}
