namespace MySchoolDiaryProject.Web.ViewModels.Course
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Web.ViewModels.Teachers;

    public class CourseInputViewModel
    {
        public int Id { get; set; }

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

        public Teacher Teacher { get; set; }

        public IEnumerable<TeachersDropDownViewModel> Teachers { get; set; }
    }
}
