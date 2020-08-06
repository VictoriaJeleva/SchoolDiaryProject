namespace MySchoolDiaryProject.Web.ViewModels.Subjects
{

    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Mapping;

    public class SubjectDropDownListViewModel : IMapFrom<Subject>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
