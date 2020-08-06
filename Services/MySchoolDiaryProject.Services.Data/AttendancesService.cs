using MySchoolDiaryProject.Data.Common.Repositories;
using MySchoolDiaryProject.Data.Models;
using MySchoolDiaryProject.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySchoolDiaryProject.Services.Data
{
    public class AttendancesService : IAttendancesService
    {
        private readonly IRepository<Attendance> attendanceRepository;
        private readonly IStudentsService studentsService;
        private readonly ISubjectsService subjectsService;

        public AttendancesService(
            IRepository<Attendance> attendanceRepository,
            IStudentsService studentsService,
            ISubjectsService subjectsService)
        {
            this.attendanceRepository = attendanceRepository;
            this.studentsService = studentsService;
            this.subjectsService = subjectsService;
        }

        public Attendance CreateAttendance(Subject subject, Student student, DateTime dateOfAbsense, string remark)
        {
            Attendance attendance = new Attendance(subject, student, dateOfAbsense, remark);

            return attendance;
        }

        public IEnumerable<T> GetAbbsenseByStudentId<T>(int studentId, int? take = null, int skip = 0)
        {
            var query = this.attendanceRepository.All()
                .OrderByDescending(x => x.DateOfAbsense.Date)
                .Where(x => x.StudentId == studentId).Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public IEnumerable<Attendance> GetAllAtendancesForCourse(int courseId)
        {
            return this.attendanceRepository.All().Where(x => x.Student.CourseId == courseId).Select(x => x);
        }

        public IEnumerable<Attendance> GetAllAtendancesForStudent(int studentId)
        {
            return this.attendanceRepository.All().Where(x => x.StudentId == studentId).Select(x => x);
        }

        public IEnumerable<Attendance> GetAllAtendancesForSubject(int subjectId)
        {
            return this.attendanceRepository.All().Where(x => x.SubjectId == subjectId).Select(x => x);
        }

        public IEnumerable<Attendance> GetAllAtendancesForSubjectAndCourse(int subjectId, int courseId)
        {
            List<Attendance> list = new List<Attendance>();
            var students = this.studentsService.GetAll(courseId);

            foreach (var student in students)
            {
                var attendances = this.attendanceRepository.All().Where(x => x.SubjectId == subjectId && x.StudentId == student.Id).Select(x => x);

                if (attendances != null)
                {
                    foreach (var item in attendances)
                    {
                        list.Add(item);
                    }
                }
            }

            return list;
        }

        public T GetById<T>(int attendanceId)
        {
            var attendance = this.attendanceRepository.All().Where(x => x.Id == attendanceId).Select(x => x).To<T>().FirstOrDefault();

            //attendance.Student = this.studentsService.GetStudentById(attendance.StudentId);
            //if (attendance.SubjectId != 0)
            //{
            //    int subjectId = (int)attendance.SubjectId;
            //    attendance.Subject = this.subjectsService.GetById(subjectId);
            //}

            return attendance;
        }

        public Attendance GetById(int attendanceId)
        {
            return this.attendanceRepository.All().Where(x => x.Id == attendanceId).Select(x => x).FirstOrDefault();
        }

        public int GetCountByStudentId(int studentId)
        {
            return this.attendanceRepository.All().Count(x => x.StudentId == studentId);
        }

        public int GetStudentId(int attendanceId)
        {
            return this.attendanceRepository.All().Where(x => x.Id == attendanceId).Select(x => x.StudentId).FirstOrDefault();
        }
    }
}
