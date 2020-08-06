namespace MySchoolDiaryProject.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using MySchoolDiaryProject.Data.Models;

    public interface IAttendancesService
    {
        public Attendance CreateAttendance(Subject subject, Student student, DateTime dateOfAbsense, string remark);

        public IEnumerable<Attendance> GetAllAtendancesForSubject(int subjectId);

        public IEnumerable<Attendance> GetAllAtendancesForSubjectAndCourse(int subjectId, int courseId);

        public IEnumerable<Attendance> GetAllAtendancesForStudent(int studentId);

        public IEnumerable<Attendance> GetAllAtendancesForCourse(int courseId);

        public IEnumerable<T> GetAbbsenseByStudentId<T>(int studentId, int? take = null, int skip = 0);

        public int GetCountByStudentId(int studentId);

        public T GetById<T>(int attendanceId);

        public Attendance GetById(int attendanceId);

        public int GetStudentId(int attendanceId);
    }
}
