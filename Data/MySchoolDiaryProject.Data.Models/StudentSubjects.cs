using System;
using System.Collections.Generic;
using System.Text;

namespace MySchoolDiaryProject.Data.Models
{
    public class StudentSubjects
    {
        public int StudentId { get; set; }

        public virtual Student Student { get; set; }

        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }
    }
}
