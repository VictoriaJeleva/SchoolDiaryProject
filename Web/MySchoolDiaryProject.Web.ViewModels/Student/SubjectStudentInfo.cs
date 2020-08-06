namespace MySchoolDiaryProject.Web.ViewModels.Student
{
    using System.Collections.Generic;

    public class SubjectStudentInfo
    {
        public string Name { get; set; }

        public virtual ICollection<StudentInfoGradeViewModel> Grades { get; set; }
    }
}
