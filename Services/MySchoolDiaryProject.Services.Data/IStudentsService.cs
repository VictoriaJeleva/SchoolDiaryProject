namespace MySchoolDiaryProject.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MySchoolDiaryProject.Data.Models;

    public interface IStudentsService
    {
        public IEnumerable<T> GetAll<T>(int courseId, int? take = null, int skip = 0);

        public IEnumerable<Student> GetAll(int courseId);

        public IEnumerable<T> GetAll<T>(int? take = null, int skip = 0);

        public T GetById<T>(int studentId);

        public int GetCount();

        public int GetCountInCourse(int courseId);

        public Student GetStudentById(int studentId);

        public IEnumerable<T> GetAllSubjectsForStudent<T>(int studentId);

        public IEnumerable<Subject> GetAllSubjectsForStudent(int studentId);

        int GetIdSubjectByName(string subjectName);

        int? GetCourseIdByStudentId(int studentId);

        public T GetStudentViewModel<T>(int studentId);

        public List<StudentSubjects> FillStudentSubjectsCollection(List<Subject> subjectsToAdd, int studentId);

        //Task AddGradeToStudent(Subject subject, Grade grade);

        //Task DeleteCourseIdFromStudents(int courseId);
    }
}
