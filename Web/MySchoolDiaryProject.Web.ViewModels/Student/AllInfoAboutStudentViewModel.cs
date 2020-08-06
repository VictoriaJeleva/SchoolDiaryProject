namespace MySchoolDiaryProject.Web.ViewModels.Student
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;
    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Mapping;
    using MySchoolDiaryProject.Web.ViewModels.Course;

    public class AllInfoAboutStudentViewModel : IMapFrom<Student>
    {
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string MiddleName { get; set; }

        [Required]
        [MinLength(2)]
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

        public IFormFile Photo { get; set; }

        public string PhotoPath { get; set; }

        public IEnumerable<CourseDropDownViewModel> Courses { get; set; }

        public virtual ICollection<StudentSubjects> Subjects { get; set; }

        public virtual ICollection<Subject> SubjectsNames { get; set; }
    }
}
