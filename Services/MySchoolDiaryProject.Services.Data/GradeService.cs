using MySchoolDiaryProject.Data.Common.Repositories;
using MySchoolDiaryProject.Data.Models;
using MySchoolDiaryProject.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySchoolDiaryProject.Services.Data
{
    public class GradeService : IGradeService
    {
        private readonly IRepository<Student> studentRepository;
        private readonly IRepository<Grade> gradeRepository;
        private readonly IStudentsService studentsService;

        public GradeService(
            IRepository<Student> studentRepository,
            IRepository<Grade> gradeRepository,
            IStudentsService studentsService)
        {
            this.studentRepository = studentRepository;
            this.gradeRepository = gradeRepository;
            this.studentsService = studentsService;
        }

        public IEnumerable<Grade> GetAlGradesForCourse(int courseId)
        {
            List<Grade> list = new List<Grade>();
            var students = this.studentsService.GetAll(courseId);

            foreach (var student in students)
            {
                var grades = this.gradeRepository.All().Where(x => x.StudentId == student.Id).Select(x => x);

                if (grades != null)
                {
                    foreach (var item in grades)
                    {
                        list.Add(item);
                    }
                }
            }

            return list;
        }

        public IEnumerable<Grade> GetAlGradesForStudent(int studentId)
        {
            return this.gradeRepository.All().Where(x => x.StudentId == studentId).Select(x => x);
        }

        public IEnumerable<Grade> GetAlGradesForSubject(int subjectId)
        {
            return this.gradeRepository.All().Where(x => x.SubjectId == subjectId).Select(x => x);
        }

        public IEnumerable<Grade> GetAlGradesForSubjectAndCourse(int subjectId, int courseId)
        {
            List<Grade> list = new List<Grade>();
            var students = this.studentsService.GetAll(courseId);

            foreach (var student in students)
            {
                var grades = this.gradeRepository.All().Where(x => x.SubjectId == subjectId && x.StudentId == student.Id).Select(x => x);

                if (grades != null)
                {
                    foreach (var item in grades)
                    {
                        list.Add(item);
                    }
                }
            }

            return list;
        }

        public IEnumerable<T> GetAll<T>(int studentId)
        {
            var c = this.gradeRepository.All().Where(x => x.StudentId == studentId).Select(x => x).To<T>().ToList();

            return this.gradeRepository.All().Where(x => x.StudentId == studentId).Select(x => x).To<T>().ToList();
        }

        public T GetById<T>(int gradeId)
        {
            var grade = this.gradeRepository.All().Where(x => x.Id == gradeId).To<T>().FirstOrDefault();

            return grade;
        }

        public Grade GetById(int gradeId)
        {
            return this.gradeRepository.All().Where(x => x.Id == gradeId).FirstOrDefault();
        }
    }
}
