namespace MySchoolDiaryProject.Services.Data
{
    using System.Collections.Generic;

    using MySchoolDiaryProject.Data.Models;

    public interface IGradeService
    {
        public IEnumerable<T> GetAll<T>(int studentId);

        public IEnumerable<Grade> GetAlGradesForStudent(int studentId);

        public IEnumerable<Grade> GetAlGradesForSubject(int subjectId);

        public IEnumerable<Grade> GetAlGradesForCourse(int courseId);

        public IEnumerable<Grade> GetAlGradesForSubjectAndCourse(int subjectId, int courseId);

        public T GetById<T>(int gradeId);

        public Grade GetById(int gradeId);
    }
}
