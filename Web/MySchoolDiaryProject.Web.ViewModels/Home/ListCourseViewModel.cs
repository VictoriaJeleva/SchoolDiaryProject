namespace MySchoolDiaryProject.Web.ViewModels.Home
{

    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Mapping;

    public class ListCourseViewModel : IMapFrom<Course>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        //public string Url => $"/s/{this.Id}";

        public string Url => $"/s/{this.Name.Replace(' ', '-')}";
    }
}
