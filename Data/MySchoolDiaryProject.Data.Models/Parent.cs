namespace MySchoolDiaryProject.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using MySchoolDiaryProject.Data.Common.Models;

    public class Parent : BaseModel<int>
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

        public int StudentId { get; set; }

        public virtual Student Student { get; set; }
    }
}
