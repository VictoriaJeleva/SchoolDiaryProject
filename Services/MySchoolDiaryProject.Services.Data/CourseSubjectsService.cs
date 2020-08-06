namespace MySchoolDiaryProject.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using MySchoolDiaryProject.Data.Common.Repositories;
    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Mapping;

    public class CourseSubjectsService : ICourseSubjectsService
    {
        private readonly IRepository<CourseSubjects> courseSubjectsRepository;

        public CourseSubjectsService(IRepository<CourseSubjects> courseSubjectsRepository)
        {
            this.courseSubjectsRepository = courseSubjectsRepository;
        }

        public IEnumerable<T> GetAllForCourse<T>(int courseId)
        {
            return this.courseSubjectsRepository.All().Where(x => x.Course.Id == courseId).Select(x => x).To<T>().ToList();
        }

        public IEnumerable<CourseSubjects> GetAllForCourse(int courseId)
        {
            return this.courseSubjectsRepository.All().Where(x => x.CourseId == courseId).Select(x => x);
        }

        public IEnumerable<CourseSubjects> GetAllSubjectsBySubjectAndCourse(int subjectId, int courseId)
        {


            return this.courseSubjectsRepository.All().Where(x => x.SubjectId == subjectId && x.CourseId == courseId).Select(x => x);
        }

        public IEnumerable<CourseSubjects> GetAllSubjectsBySubjectId(int subjectId)
        {
            return this.courseSubjectsRepository.All().Where(x => x.SubjectId == subjectId).Select(x => x);
        }

        public IEnumerable<Subject> GetAllSubjectsForCourse(int courseId)
        {
            var subjects = this.courseSubjectsRepository.All().Where(x => x.CourseId == courseId)
                .Select(x => x.Subject).OrderBy(x => x.Name);

            return subjects;
        }

        public IEnumerable<int> GetAllSubjectsIds(int courseId)
        {
            var subjectsIds = this.courseSubjectsRepository.All().Where(x => x.CourseId == courseId).Select(x => x.SubjectId);

            return subjectsIds;
        }

        public IEnumerable<T> GetAllSubjectsNotForCourse<T>(int courseId)
        {
            var subjects = this.courseSubjectsRepository.All().Where(x => x.CourseId != courseId)
                .Select(x => x.Subject).Distinct().To<T>().ToList();

            return subjects;
        }
    }
}
