namespace MySchoolDiaryProject.Web.ViewModels.Grade
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Mapping;
    using MySchoolDiaryProject.Web.ViewModels.Subjects;

    public class GradeInputViewModel
    {
        [Range(2.00, 6.00)]
        public double Value { get; set; }

        [Required]
        public int SubjectId { get; set; }

        public IEnumerable<SubjectDropDownListViewModel> Subjects { get; set; }
    }
}
