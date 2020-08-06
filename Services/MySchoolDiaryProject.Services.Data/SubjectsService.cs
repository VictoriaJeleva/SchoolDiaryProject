namespace MySchoolDiaryProject.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using MySchoolDiaryProject.Data.Common.Repositories;
    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Mapping;

    public class SubjectsService : ISubjectsService
    {
        private readonly IRepository<Subject> subjectRepository;
        private readonly IRepository<Course> courseRepository;
        private readonly IRepository<CourseSubjects> courseSubjectsRepository;

        public SubjectsService(
            IRepository<Subject> subjectRepository,
            IRepository<Course> courseRepository,
            IRepository<CourseSubjects> courseSubjectsRepository)
        {
            this.subjectRepository = subjectRepository;
            this.courseRepository = courseRepository;
            this.courseSubjectsRepository = courseSubjectsRepository;
        }

        public IEnumerable<T> GetAllSubjectsForCourseById<T>(IEnumerable<int> subjectsId)
        {
            var subjectsName = Enumerable.Empty<Subject>().AsQueryable();

            foreach (var subjectId in subjectsId)
            {
               subjectsName = this.subjectRepository.All().Where(x => x.Id == subjectId);
            }

            return subjectsName.To<T>().ToList();
        }

        public IEnumerable<int> GetAllSubjectsInCoursesIds(int id)
        {
            var allSubjectsInCoursesIds = this.courseSubjectsRepository.All().Where(x => x.CourseId == id).Select(x => x.SubjectId);

            return allSubjectsInCoursesIds;
        }

        public List<string> GetAllSubjectNameInCourses(IEnumerable<int> subjectInCoursesIds)
        {
            List<string> subjectsNames = new List<string>();

            foreach (var subjectId in subjectInCoursesIds)
            {
                var subject = this.subjectRepository.All().Where(x => x.Id == subjectId).Select(x => x.Name)
                    .OrderBy(x => x).FirstOrDefault();

                if (subject != null)
                {
                    subjectsNames.Add(subject);
                }
            }

            return subjectsNames;
        }

        public IEnumerable<Subject> GetAllSubjectsInCourses(IEnumerable<int> subjectInCoursesIds)
        {
            List<Subject> subjectsNames = new List<Subject>();

            foreach (var subjectId in subjectInCoursesIds)
            {
                var subject = this.subjectRepository.All().Where(x => x.Id == subjectId).Select(x => x).FirstOrDefault();

                if (subject != null)
                {
                    subjectsNames.Add(subject);
                }
            }

            return subjectsNames;
        }

        public Subject GetASubjectByName(string subjectName)
        {
            var subject = this.subjectRepository.All().Where(x => x.Name == subjectName).FirstOrDefault();

            return subject;
        }

        public IEnumerable<T> GetAll<T>(Student student)
        {
            return this.subjectRepository.All().To<T>().ToList();
        }

        public IEnumerable<T> GetAll<T>(int? take = null, int skip = 0)
        {
            var query = this.subjectRepository.All().OrderBy(x => x.Name).Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public IEnumerable<Subject> GetAllSubjectsByIds(IEnumerable<int> subjectsId)
        {
            List<Subject> list = new List<Subject>();

            foreach (var subjectId in subjectsId)
            {
                var subject = this.subjectRepository.All().Where(x => x.Id == subjectId).FirstOrDefault();

                list.Add(subject);
            }

            return list;
        }

        public IEnumerable<T> GetAllSubjectsNotInCourse<T>(IEnumerable<Subject> allSubjectsForCorse)
        {
            var allSubjects = this.subjectRepository.All().OrderBy(x => x.Name);

            List<Subject> subjectsToReturn = new List<Subject>();
            bool isItInCourse = false;

            foreach (var subject in allSubjects)
            {
                foreach (var subjectInCourse in allSubjectsForCorse)
                {
                    isItInCourse = false;
                    if (subject.Id == subjectInCourse.Id)
                    {
                        isItInCourse = true;
                        break;
                    }
                }

                if (isItInCourse == false)
                {
                    subjectsToReturn.Add(subject);
                }
            }

            IQueryable query = subjectsToReturn.AsQueryable();

            return query.To<T>().ToList();
        }

        public List<Subject> GetAllSubjectsInCourse(int courseId)
        {
            var subjects = this.courseSubjectsRepository.All().Where(x => x.CourseId == courseId).Select(x => x.Subject);

            return subjects.ToList();
        }

        public ICollection<Subject> GetAllSubjectNameInCourses(ICollection<StudentSubjects> subjects)
        {

            List<Subject> subjectsNames = new List<Subject>();

            foreach (var subject in subjects)
            {
                var subjectToAdd = this.subjectRepository.All().Where(x => x.Id == subject.SubjectId).Select(x => x).FirstOrDefault();

                if (subject != null)
                {
                    subjectsNames.Add(subjectToAdd);
                }
            }

            return subjectsNames.ToList();
        }

        public Subject GetById(int subjectId)
        {
            return this.subjectRepository.All().Where(x => x.Id == subjectId).FirstOrDefault();
        }

        public Subject GetSubject(int id)
        {
            return this.subjectRepository.All().Where(x => x.Id == id).Select(x => x).FirstOrDefault();
        }

        public int GetCount()
        {
            return this.subjectRepository.All().Count();
        }

        public T GetById<T>(int subjectId)
        {
            var subject = this.subjectRepository.All().Where(x => x.Id == subjectId).Select(x => x).To<T>().FirstOrDefault();

            return subject;
        }
    }
}
