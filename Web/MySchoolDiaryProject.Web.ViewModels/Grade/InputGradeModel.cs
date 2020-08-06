namespace MySchoolDiaryProject.Web.ViewModels.Grade
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class InputGradeModel
    {
        [Range(2.00, 6.00)]
        public double Grade { get; set; }

        public string SubjectName { get; set; }
    }
}
