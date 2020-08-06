namespace MySchoolDiaryProject.Web.ViewModels.Home
{
    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Mapping;
    using System.Collections.Generic;

    public class IndexModel : IMapFrom<Course>
    {
        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public IEnumerable<ListCourseViewModel> Names { get; set; }
    }
}
