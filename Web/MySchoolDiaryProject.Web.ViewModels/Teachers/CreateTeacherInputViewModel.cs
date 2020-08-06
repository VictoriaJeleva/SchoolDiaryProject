namespace MySchoolDiaryProject.Web.ViewModels.Teachers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class CreateTeacherInputViewModel
    {
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
    }
}
