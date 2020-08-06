namespace MySchoolDiaryProject.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MySchoolDiaryProject.Data.Common.Repositories;
    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Mapping;

    public class CoursesService : ICoursesService
    {
        private readonly IRepository<Course> coursesRepository;
        private readonly ISubjectsService subjectsService;
        private readonly ITeachersService teachersService;
        private readonly IRepository<CourseSubjectTeacher> courseSubjectTeacherRepository;

        public CoursesService(
            IRepository<Course> coursesRepository,
            ISubjectsService subjectsService,
            ITeachersService teachersService,
            IRepository<CourseSubjectTeacher> courseSubjectTeacherRepository)
        {
            this.coursesRepository = coursesRepository;
            this.subjectsService = subjectsService;
            this.teachersService = teachersService;
            this.courseSubjectTeacherRepository = courseSubjectTeacherRepository;
        }

        public async Task AddACourse(Course course)
        {
            await this.coursesRepository.AddAsync(course);

            await this.coursesRepository.SaveChangesAsync();
        }

        public void AddSubjectTeacherToCourse(int courseId, SubjectTeachers subjectTeacher)
        {
           //var c = this.coursesRepository.All().Where(x => x.Id == courseId).Select(x => x.SubjectTeachers.Add(subjectTeacher));
        }

        public IEnumerable<T> GetAllWithouPages<T>(int? count = null)
        {
            IQueryable<Course> query =
                this.coursesRepository.All().OrderBy(x => x.Name);

            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return query.To<T>().ToList();
        }

        public IEnumerable<T> GetAll<T>(int? take = null, int skip = 0)
        {
            var query = this.coursesRepository.All().OrderBy(x => x.Name)
                .Select(x => x).Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public IEnumerable<Subject> GetAllSubjectsForCourse(int courseId)
        {
            //x => x.SubjectNames
            var subjects = this.coursesRepository.All().Where(x => x.Id == courseId).Select(x => x.CourseSubjectTeachers.Select(s => s.Subject)).ToList();

            List<Subject> listOfSubjects = new List<Subject>();

            foreach (var item in subjects)
            {
                foreach (var subject in item)
                {
                    listOfSubjects.Add(subject);
                }
            }

            return listOfSubjects.OrderBy(x => x.Name);
        }

        //public IEnumerable<SubjectTeachers> GetAllSubjectsTeachersForCourse(int courseId)
        //{
        //    var subjectTeachers = this.coursesRepository.All().Select(x => x).Where(x => x.Id == courseId).ToList();

        //    List<SubjectTeachers> listOfSubjectsTeachers = new List<SubjectTeachers>();

        //    foreach (var item in subjectTeachers)
        //    {
        //        foreach (var subjectTeacher in item.SubjectTeachers)
        //        {
        //            subjectTeacher.Subject = this.subjectsService.GetById(subjectTeacher.SubjectId);
        //            subjectTeacher.Teacher = this.teachersService.GetTeacherById(subjectTeacher.TeacherId);
        //            listOfSubjectsTeachers.Add(subjectTeacher);
        //        }
        //    }

        //    return listOfSubjectsTeachers.OrderBy(x => x.Subject.Name);
        //}

        //public IEnumerable<T> GetAllSubjectTeachersForCourse<T>(int courseId)
        //{
        //    var subjectTeachers = this.coursesRepository.All().Where(x => x.Id == courseId).Select(x => x.SubjectTeachers).FirstOrDefault();

        //    IQueryable query = subjectTeachers.AsQueryable();

        //    return query.To<T>().ToList();
        //}

        public Course GetById(int? id = null)
        {
            var course = this.coursesRepository.All()
                .Where(x => x.Id == id).FirstOrDefault();

            return course;
        }

        public T GetById<T>(int id)
        {
            var course = this.coursesRepository.All().Where(x => x.Id == id)
               .To<T>().FirstOrDefault();

            return course;
        }

        public T GetByName<T>(string name)
        {
            string replacedName = string.Empty;

            replacedName = name.Replace('-', ' ');

            var course = this.coursesRepository.All().Where(x => x.Name == replacedName)
                .To<T>().FirstOrDefault();

            return course;
        }

        public Course GetByName(string name)
        {
            var course = this.coursesRepository.All().Where(x => x.Name == name).FirstOrDefault();

            return course;
        }

        public int GetCount()
        {
            return this.coursesRepository.All().Count();
        }

        public Task ModifyCourse(Course course)
        {
            var courseToChange = this.coursesRepository.All()
                .FirstOrDefault(x => x.Id == course.Id);

            return null;
        }

        public void RemoveAStudentFromStudentsCollection(Student student)
        {
            this.coursesRepository.All().Where(x => x.Id == student.CourseId).Select(x => x.Students.Remove(student));
        }

        public Course GetByClassTeacher(int teacherId)
        {
            return this.coursesRepository.All().Where(x => x.TeacherId == teacherId).Select(x => x).FirstOrDefault();
        }

        public IEnumerable<T> GetAllForClassTeacher<T>(int teacherId)
        {
            return this.coursesRepository.All().Where(x => x.TeacherId == teacherId).Select(x => x).To<T>().ToList();
        }

        public string GetName(int courseId)
        {
            return this.coursesRepository.All().Where(x => x.Id == courseId).Select(x => x.Name).FirstOrDefault();
        }
    }
}
