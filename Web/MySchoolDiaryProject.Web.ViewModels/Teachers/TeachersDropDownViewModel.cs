namespace MySchoolDiaryProject.Web.ViewModels.Teachers
{
    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Mapping;

    public class TeachersDropDownViewModel : IMapFrom<Teacher>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }
    }
}
