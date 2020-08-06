namespace MySchoolDiaryProject.Web.ViewModels.Grade
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Mapping;
    using MySchoolDiaryProject.Web.ViewModels.Subjects;

    public class GradeViewModel : IMapFrom<Grade>
    {
        public int Id { get; set; }

        [Range(2.00, 6.00)]
        public double Value { get; set; }

        public int StudentId { get; set; }

        public virtual Student Student { get; set; }

        public int? SubjectId { get; set; }

        public virtual Subject Subject { get; set; }

        public string SubjectName { get; set; }

        //public double Value { get; set; }

        //public int SubjectId{ get; set; }
    }
}
