using System;
using System.Collections.Generic;
using System.Text;

namespace MySchoolDiaryProject.Data.Models
{
    public class CourseSubjectTeacher
    {
        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }

        public int TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }
    }
}
