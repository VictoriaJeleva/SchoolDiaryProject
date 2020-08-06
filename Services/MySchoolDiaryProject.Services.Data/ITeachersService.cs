namespace MySchoolDiaryProject.Services.Data
{
    using System.Collections.Generic;

    using MySchoolDiaryProject.Data.Models;

    public interface ITeachersService
    {
        string GetById<T>(int id);

        public Teacher GetTeacherById(int id);

        public T GetTeacherById<T>(int id);

        public IEnumerable<T> GetAll<T>();

        public IEnumerable<T> GetAllExceptOne<T>(int teacherId);

        public int GetCount();

        public IEnumerable<T> GetAll<T>(int? take = null, int skip = 0);

        public Teacher GetTeacherByFullName(string name, string middleName, string lastName);
    }
}
