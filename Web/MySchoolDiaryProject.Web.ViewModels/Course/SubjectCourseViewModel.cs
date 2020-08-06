namespace MySchoolDiaryProject.Web.ViewModels.Course
{
    using System.Collections;
    using System.Collections.Generic;

    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Mapping;

    public class SubjectCourseViewModel : IMapFrom<Subject>, IMapFrom<CourseSubjects>
    {
        public string Name { get; set; }
    }
}
