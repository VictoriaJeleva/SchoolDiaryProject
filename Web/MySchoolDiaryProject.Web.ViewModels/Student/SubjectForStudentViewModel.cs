namespace MySchoolDiaryProject.Web.ViewModels.Student
{

    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Mapping;

    public class SubjectForStudentViewModel : IMapFrom<Subject>, IMapFrom<StudentSubjects>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    //var config = new MapperConfiguration(cfg =>
    //{
    //    cfg.CreateMap<Product, ProductBase>()
    //     .ForMember(pob => pob.ProductOptionsBase, opts => opts.MapFrom(po => po.ProductOptions));
    //    cfg.CreateMap<ProductOption, ProductOptionBase>();
    //});
}
