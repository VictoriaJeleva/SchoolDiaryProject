﻿namespace MySchoolDiaryProject.Web.ViewModels.Student
{
    using System.Collections.Generic;

    public class ListOfStudentsInfoGrades
    {
        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public IEnumerable<StudentInfoGradeViewModel> Students { get; set; }
    }
}