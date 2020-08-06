using MySchoolDiaryProject.Data.Common.Repositories;
using MySchoolDiaryProject.Data.Models;
using MySchoolDiaryProject.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MySchoolDiaryProject.Services.Data
{
    public class StudentSubjectsService : IStudentSubjectsService
    {
        private readonly IRepository<StudentSubjects> studentSubjectsRepository;
        private readonly IStudentsService studentsService;

        public StudentSubjectsService(
            IRepository<StudentSubjects> studentSubjectsRepository,
            IStudentsService studentsService)
        {
            this.studentSubjectsRepository = studentSubjectsRepository;
            this.studentsService = studentsService;
        }

        public IEnumerable<StudentSubjects> GetAllStudentSubjectsForCourseId(int courseId)
        {
            return this.studentSubjectsRepository.All().Where(x => x.Student.CourseId == courseId).Select(x => x);
        }

        public IEnumerable<StudentSubjects> GetAllStudentSubjectsForStudentId(int stidentId)
        {
            var collection = this.studentSubjectsRepository.All().Select(x => x).Where(x => x.StudentId == stidentId);

            return collection;
        }

        public IEnumerable<StudentSubjects> GetAllStudentSubjectsForSubjectAndCourse(int subjectId, int courseId)
        {
            List<StudentSubjects> list = new List<StudentSubjects>();
            var students = this.studentsService.GetAll(courseId);

            foreach (var student in students)
            {
                var studentSubject = this.studentSubjectsRepository.All().Select(x => x).Where(x => x.SubjectId == subjectId && x.StudentId == student.Id);

                if (studentSubject != null)
                {
                    foreach (var item in studentSubject)
                    {
                        list.Add(item);
                    }
                }
            }

            return list;
        }

        public IEnumerable<StudentSubjects> GetAllStudentSubjectsForSubjectId(int subjectId)
        {
            var collection = this.studentSubjectsRepository.All().Select(x => x).Where(x => x.SubjectId == subjectId);

            return collection;
        }

        public IEnumerable<T> GetAllSubjectsByStudentId<T>(int studentId)
        {
            var subjects = this.studentSubjectsRepository.All().Where(x => x.StudentId == studentId).Select(x => x.Subject).OrderBy(x => x.Name).To<T>();

            return subjects;
        }

        public IEnumerable<int> GetAllSubjectsIds(int studentId)
        {
            var subjectsIds = this.studentSubjectsRepository.All().Where(x => x.StudentId == studentId).Select(x => x.SubjectId);

            return subjectsIds;
        }

        public ICollection<Subject> GetOnlySubjectsFromCollection(ICollection<StudentSubjects> collection)
        {
            ICollection<Subject> subjects = new Collection<Subject>();

            foreach (var item in collection)
            {
                subjects.Add(item.Subject);
            }

            return subjects;
        }
    }
}
