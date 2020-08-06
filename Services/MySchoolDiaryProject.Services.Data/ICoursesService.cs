namespace MySchoolDiaryProject.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MySchoolDiaryProject.Data.Models;

    public interface ICoursesService
    {
        public IEnumerable<T> GetAllWithouPages<T>(int? count = null);

        public IEnumerable<T> GetAll<T>(int? take = null, int skip = 0);

        public IEnumerable<Subject> GetAllSubjectsForCourse(int courseId);

        public IEnumerable<T> GetAllForClassTeacher<T>(int teacherId);

        public void AddSubjectTeacherToCourse(int courseId, SubjectTeachers subjectTeacher);

        //public IEnumerable<T> GetAllSubjectTeachersForCourse<T>(int courseId);

        public T GetByName<T>(string name);

        public Course GetByClassTeacher(int teacherId);

        public int GetCount();

        public Course GetByName(string name);

        public Course GetById(int? id = null);

        public T GetById<T>(int id);

        public Task AddACourse(Course course);

        public Task ModifyCourse(Course course);

        public string GetName(int courseId);

        public void RemoveAStudentFromStudentsCollection(Student student);
    }
}
