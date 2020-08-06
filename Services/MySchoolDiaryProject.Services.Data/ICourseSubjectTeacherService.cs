namespace MySchoolDiaryProject.Services.Data
{
    using System.Collections.Generic;

    using MySchoolDiaryProject.Data.Models;

    public interface ICourseSubjectTeacherService
    {
        public IEnumerable<T> GetAllForCourse<T>(int courseId);

        public IEnumerable<T> GetAllForStudent<T>(int courseId, int? take = null, int skip = 0);

        public int GetCountForCourse(int courseId);

        public IEnumerable<CourseSubjectTeacher> GetAllSubjectsTeachersForCourse(int courseId);

        public IEnumerable<CourseSubjectTeacher> GetAllSubjectsTeachersForSubject(int subjectId);

        public IEnumerable<CourseSubjectTeacher> GetAllSubjectsTeachersForSubjectAndCourse(int subjectId, int courseId);

        public IEnumerable<CourseSubjectTeacher> GetAllSubjectsTeachersForTeacher(int teacherId);

        public IEnumerable<Subject> GetAllSubjectsForCourse(int courseId);
    }
}
