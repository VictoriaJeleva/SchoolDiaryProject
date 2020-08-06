namespace MySchoolDiaryProject.Web.ViewModels.Home
{
    using MySchoolDiaryProject.Data.Models;

    public class IndexParentViewModel
    {
        public int StudentId { get; set; }

        public Student Student { get; set; }

        public int CourseId { get; set; }
    }
}
