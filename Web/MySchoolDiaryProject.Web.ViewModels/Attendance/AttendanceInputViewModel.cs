namespace MySchoolDiaryProject.Web.ViewModels.Attendance
{
    using System;
    using System.Collections.Generic;

    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Web.ViewModels.Subjects;

    public class AttendanceInputViewModel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }

        public virtual Student Student { get; set; }

        public int? SubjectId { get; set; }

        public virtual Subject Subject { get; set; }

        public DateTime DateOfAbsense { get; set; }

        public string Remark { get; set; }

        public IEnumerable<SubjectDropDownListViewModel> Subjects { get; set; }
    }
}
