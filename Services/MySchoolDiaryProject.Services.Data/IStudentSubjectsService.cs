namespace MySchoolDiaryProject.Services.Data
{
    using System.Collections.Generic;

    using MySchoolDiaryProject.Data.Models;

    public interface IStudentSubjectsService
    {
        public IEnumerable<int> GetAllSubjectsIds(int studentId);

        public IEnumerable<T> GetAllSubjectsByStudentId<T>(int studentId);

        public ICollection<Subject> GetOnlySubjectsFromCollection(ICollection<StudentSubjects> collection);

        public IEnumerable<StudentSubjects> GetAllStudentSubjectsForSubjectId(int subjectId);

        public IEnumerable<StudentSubjects> GetAllStudentSubjectsForSubjectAndCourse(int subjectId, int courseId);

        public IEnumerable<StudentSubjects> GetAllStudentSubjectsForCourseId(int courseId);

        public IEnumerable<StudentSubjects> GetAllStudentSubjectsForStudentId(int stidentId);
    }
}
