namespace MySchoolDiaryProject.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MySchoolDiaryProject.Data.Common.Repositories;
    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Mapping;

    public class StudentsService : IStudentsService
    {
        private readonly IRepository<Student> studentRepository;
        private readonly IRepository<Subject> subjectRepository;
        private readonly IRepository<StudentSubjects> studentSubjectsRepository;

        public StudentsService(
            IRepository<Student> studentRepository,
            IRepository<Subject> subjectRepository,
            IRepository<StudentSubjects> studentSubjectsRepository)
        {
            this.studentRepository = studentRepository;
            this.subjectRepository = subjectRepository;
            this.studentSubjectsRepository = studentSubjectsRepository;
        }

        public List<StudentSubjects> FillStudentSubjectsCollection(List<Subject> subjectsToAdd, int studentId)
        {
            List<StudentSubjects> collection = new List<StudentSubjects>();

            foreach (var subject in subjectsToAdd)
            {
                var student = this.studentRepository.All().Where(x => x.Id == studentId).FirstOrDefault();
                StudentSubjects item = new StudentSubjects();
                item.Subject = subject;
                item.SubjectId = subject.Id;
                item.Student = student;
                item.StudentId = studentId;
                collection.Add(item);
            }

            return collection;
        }

        public IEnumerable<T> GetAll<T>(int courseId, int? take = null, int skip = 0)
        {
            var query = this.studentRepository.All()
                .OrderBy(x => x.Name).ThenBy(x => x.MiddleName).ThenBy(x => x.LastName)
                .Where(x => x.CourseId == courseId).Select(x => x).Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public IEnumerable<Student> GetAll(int courseId)
        {
            return this.studentRepository.All().Where(x => x.CourseId == courseId).Select(x => x);
        }

        public IEnumerable<T> GetAll<T>(int? take = null, int skip = 0)
        {
            var query = this.studentRepository.All().OrderBy(x => x.Course.Name)
                .ThenBy(x => x.Name).ThenBy(x => x.MiddleName).ThenBy(x => x.LastName)
                .Select(x => x).Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public IEnumerable<T> GetAllStudentsForCourseById<T>(int courseId)
        {
            var students = this.studentRepository.All().Where(x => x.Id == courseId);

            return students.To<T>().ToList();
        }

        public IEnumerable<T> GetAllSubjectsForStudent<T>(int studentId)
        {
            //var all = this.studentRepository.All().Select(x => x.Subjects).ToList();
            var subjects = this.studentRepository.All().Where(x => x.Id == studentId).Select(x => x.Subjects).ToList();

            List<Subject> listOfSubjects = new List<Subject>();

            foreach (var item in subjects)
            {
                foreach (var subject in item)
                {
                    listOfSubjects.Add(subject.Subject);
                }
            }

            IQueryable query = listOfSubjects.AsQueryable();

            return query.To<T>().ToList();
        }

        public IEnumerable<Subject> GetAllSubjectsForStudent(int studentId)
        {
            var subjects = this.studentRepository.All().Where(x => x.Id == studentId).Select(x => x.Subjects).ToList();

            List<Subject> listOfSubjects = new List<Subject>();

            //foreach (var item in subjects)
            //{
            //    foreach (var subject in item)
            //    {
            //        listOfSubjects.Add(subject);
            //    }
            //}

            return listOfSubjects;
        }

        public T GetById<T>(int studentId)
        {
            var student = this.studentRepository.All().Where(x => x.Id == studentId).Select(x => x).To<T>().FirstOrDefault();

            return student;
        }

        public int GetCount()
        {
            return this.studentRepository.All().Count();
        }

        public int GetCountInCourse(int courseId)
        {
            return this.studentRepository.All().Count(x => x.CourseId == courseId);
        }

        public int? GetCourseIdByStudentId(int studentId)
        {
            var courseId = this.studentRepository.All().Where(x => x.Id == studentId).Select(x => x.CourseId).FirstOrDefault();

            return courseId;
        }

        public int GetIdSubjectByName(string subjectName)
        {
            int subjectId = this.subjectRepository.All().
                Where(x => x.Name == subjectName).Select(x => x.Id).FirstOrDefault();

            return subjectId;
        }

        public Student GetStudentById(int studentId)
        {
            return this.studentRepository.All().Where(x => x.Id == studentId).Select(x => x).FirstOrDefault();
        }

        public T GetStudentViewModel<T>(int studentId)
        {
            var student = this.studentRepository.All().Where(x => x.Id == studentId).Select(x => x).To<T>().FirstOrDefault();

            return student;
        }
    }
}
