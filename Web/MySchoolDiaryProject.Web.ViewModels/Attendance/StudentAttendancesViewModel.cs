namespace MySchoolDiaryProject.Web.ViewModels.Attendance
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Mapping;

    public class StudentAttendancesViewModel : IMapFrom<Attendance>
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public virtual Student Student { get; set; }

        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }

        public DateTime DateOfAbsense { get; set; }

        public string Remark { get; set; }
    }
}
