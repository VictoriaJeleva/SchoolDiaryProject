namespace MySchoolDiaryProject.Web.ViewModels.Subjects
{
    using System.ComponentModel.DataAnnotations;

    public class CreateSubjectInputViewModel
    {
        [Required]
        [MinLength(1)]
        public string Name { get; set; }
    }
}
