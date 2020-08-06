namespace MySchoolDiaryProject.Services.Data
{
    using System.Collections.Generic;

    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Web.ViewModels.Subjects;

    public interface ISubjectTeachersService
    {
        public IEnumerable<SubjectTeachers> GetAllSubjectsTeachersForTeacher(int teacherId);

        public IEnumerable<T> GetAllSubjectsTeachersForTeacher<T>(int teacherId);

        public IEnumerable<T> GetAllSubjectsForTeacher<T>(int teacherId);

        public IEnumerable<SubjectTeachers> GetAllSubjectsTeachers(int subjectId);

        public IEnumerable<T> GetAllSubjectsNotForTeacher<T>(IEnumerable<SubjectTeachers> subjectTeachers);

        public IEnumerable<T> GetAllSubjectTeachersNotInCourse<T>(IEnumerable<CourseSubjectTeacher> allSubjectTeachersForCorse);

        public IEnumerable<string> GetAllSubjectTeachersString(IEnumerable<SubjectTeachersDropDownListViewModel> subjectTeachers);

        public IEnumerable<T> GellAllSubjectTeachersNotInCollection<T>(IEnumerable<Subject> subjects);
    }
}
