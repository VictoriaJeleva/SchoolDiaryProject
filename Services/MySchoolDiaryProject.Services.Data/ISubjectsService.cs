namespace MySchoolDiaryProject.Services.Data
{
    using System.Collections.Generic;

    using MySchoolDiaryProject.Data.Models;

    public interface ISubjectsService
    {
        IEnumerable<int> GetAllSubjectsInCoursesIds(int id);

        //List<int> GetAllSubjectsIdsForCourseById<T>(int courseId, IEnumerable<int> allsubjectsId);

        IEnumerable<T> GetAllSubjectsForCourseById<T>(IEnumerable<int> subjectsId);

        IEnumerable<Subject> GetAllSubjectsByIds(IEnumerable<int> subjectsId);

        IEnumerable<T> GetAll<T>(int? take = null, int skip = 0);

        public Subject GetById(int subjectId);

        public T GetById<T>(int subjectId);

        public int GetCount();

        List<string> GetAllSubjectNameInCourses(IEnumerable<int> subjectInCoursesIds);

        public ICollection<Subject> GetAllSubjectNameInCourses(ICollection<StudentSubjects> subjects);

        public IEnumerable<Subject> GetAllSubjectsInCourses(IEnumerable<int> subjectInCoursesIds);

        public IEnumerable<T> GetAllSubjectsNotInCourse<T>(IEnumerable<Subject> allSubjectsForCorse);

        public List<Subject> GetAllSubjectsInCourse(int courseId);

        Subject GetASubjectByName(string subjectName);

        public Subject GetSubject(int id);
    }
}
