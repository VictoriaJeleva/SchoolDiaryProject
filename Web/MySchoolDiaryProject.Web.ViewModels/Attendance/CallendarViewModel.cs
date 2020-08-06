namespace MySchoolDiaryProject.Web.ViewModels.Attendance
{
    using System;

    using MySchoolDiaryProject.Data.Models;

    public class CallendarViewModel
    {
        public int StudentId { get; set; }

        public virtual Student Student { get; set; }

        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

        public DateTime DateOfAbsense { get; set; }

        public bool Status { get; set; }

        public string Remark { get; set; }
    }
}
