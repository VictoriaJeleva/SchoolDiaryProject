namespace MySchoolDiaryProject.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using MySchoolDiaryProject.Data.Common.Repositories;
    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Mapping;

    public class TeachersService : ITeachersService
    {
        private readonly IRepository<Teacher> teachersRepository;

        public TeachersService(IRepository<Teacher> teachersRepository)
        {
            this.teachersRepository = teachersRepository;
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.teachersRepository.All().OrderBy(x => x.Name).To<T>().ToList();
        }

        public IEnumerable<T> GetAll<T>(int? take = null, int skip = 0)
        {
            var query = this.teachersRepository.All().OrderBy(x => x.Name)
               .ThenBy(x => x.MiddleName).ThenBy(x => x.LastName)
               .Select(x => x).Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public IEnumerable<T> GetAllExceptOne<T>(int teacherId)
        {
            return this.teachersRepository.All().Where(x => x.Id != teacherId).OrderBy(x => x.Name).To<T>().ToList();
        }

        public string GetById<T>(int id)
        {
            var name = this.teachersRepository.All().Where(x => x.Id == id)
                .Select(x => x.Name + ' ' + x.MiddleName + ' ' + x.LastName)
                .FirstOrDefault();

            return name;
        }

        public int GetCount()
        {
            return this.teachersRepository.All().Count();
        }

        public Teacher GetTeacherByFullName(string name, string middleName, string lastName)
        {
            return this.teachersRepository.All().Where(x => x.Name == name && x.MiddleName == middleName && x.LastName == lastName).FirstOrDefault();
        }

        public Teacher GetTeacherById(int id)
        {
            return this.teachersRepository.All().FirstOrDefault(x => x.Id == id);
        }

        public T GetTeacherById<T>(int id)
        {
            var teacher = this.teachersRepository.All().Where(x => x.Id == id).Select(x => x).To<T>().FirstOrDefault();

            return teacher;
        }
    }
}
