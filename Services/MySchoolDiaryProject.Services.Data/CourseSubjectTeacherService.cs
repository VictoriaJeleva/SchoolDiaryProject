namespace MySchoolDiaryProject.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using MySchoolDiaryProject.Data.Common.Repositories;
    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Mapping;

    public class CourseSubjectTeacherService : ICourseSubjectTeacherService
    {
        private readonly IRepository<CourseSubjectTeacher> courseSubjectTeacherRepository;

        public CourseSubjectTeacherService(
            IRepository<CourseSubjectTeacher> courseSubjectTeacherRepository)
        {
            this.courseSubjectTeacherRepository = courseSubjectTeacherRepository;
        }

        public IEnumerable<T> GetAllForStudent<T>(int courseId, int? take = null, int skip = 0)
        {
            var query = this.courseSubjectTeacherRepository.All()
                .Where(x => x.CourseId == courseId).Select(x => x).Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public int GetCountForCourse(int courseId)
        {
            return this.courseSubjectTeacherRepository.All()
                .Where(x => x.CourseId == courseId).Select(x => x).Count();
        }

        public IEnumerable<Subject> GetAllSubjectsForCourse(int courseId)
        {
            return this.courseSubjectTeacherRepository.All().Where(x => x.CourseId == courseId).Select(x => x.Subject);
        }

        public IEnumerable<CourseSubjectTeacher> GetAllSubjectsTeachersForCourse(int courseId)
        {
            return this.courseSubjectTeacherRepository.All().Where(x => x.CourseId == courseId).Select(x => x);
        }

        public IEnumerable<CourseSubjectTeacher> GetAllSubjectsTeachersForSubject(int subjectId)
        {
            return this.courseSubjectTeacherRepository.All().Where(x => x.SubjectId == subjectId).Select(x => x);
        }

        public IEnumerable<CourseSubjectTeacher> GetAllSubjectsTeachersForSubjectAndCourse(int subjectId, int courseId)
        {
            return this.courseSubjectTeacherRepository.All().Where(x => x.SubjectId == subjectId && x.CourseId == courseId).Select(x => x);
        }

        public IEnumerable<CourseSubjectTeacher> GetAllSubjectsTeachersForTeacher(int teacherId)
        {
            return this.courseSubjectTeacherRepository.All().Where(x => x.TeacherId == teacherId).Select(x => x);
        }

        IEnumerable<T> ICourseSubjectTeacherService.GetAllForCourse<T>(int courseId)
        {
            var collection = this.courseSubjectTeacherRepository.All().Where(x => x.CourseId == courseId).Select(x => x);

            IQueryable query = collection.AsQueryable();

            return query.To<T>().ToList();
        }
    }
}
