namespace MySchoolDiaryProject.Services.Data
{
    using System.Collections.Generic;

    using MySchoolDiaryProject.Data.Models;

    public interface ICourseSubjectsService
    {
        IEnumerable<int> GetAllSubjectsIds(int courseId);

        public IEnumerable<T> GetAllSubjectsNotForCourse<T>(int courseId);

        public IEnumerable<Subject> GetAllSubjectsForCourse(int courseId);

        public IEnumerable<CourseSubjects> GetAllForCourse(int courseId);

        public IEnumerable<T> GetAllForCourse<T>(int courseId);

        public IEnumerable<CourseSubjects> GetAllSubjectsBySubjectId(int subjectId);

        public IEnumerable<CourseSubjects> GetAllSubjectsBySubjectAndCourse(int subjectId, int courseId);
    }
}
