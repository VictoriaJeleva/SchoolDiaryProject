namespace MySchoolDiaryProject.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MySchoolDiaryProject.Data.Common.Repositories;
    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Mapping;
    using MySchoolDiaryProject.Web.ViewModels.Subjects;

    public class SubjectTeachersService : ISubjectTeachersService
    {
        private readonly IRepository<SubjectTeachers> subjectTeachersRepository;
        private readonly IRepository<Subject> subjectRepository;
        private readonly ISubjectsService subjectsService;
        private readonly ITeachersService teachersService;

        public SubjectTeachersService(
            IRepository<SubjectTeachers> subjectTeachersRepository,
            IRepository<Subject> subjectRepository,
            ISubjectsService subjectsService,
            ITeachersService teachersService)
        {
            this.subjectTeachersRepository = subjectTeachersRepository;
            this.subjectRepository = subjectRepository;
            this.subjectsService = subjectsService;
            this.teachersService = teachersService;
        }

        public IEnumerable<T> GellAllSubjectTeachersNotInCollection<T>(IEnumerable<Subject> subjects)
        {
            List<SubjectTeachers> listToReturn = new List<SubjectTeachers>();

            var allsubjectTeachers = this.subjectTeachersRepository.All().Select(x => x);
            bool isItInCourse = false;

            foreach (var subject in subjects)
            {
                foreach (var subjectTeacher in allsubjectTeachers)
                {
                    if (subject.Id != subjectTeacher.SubjectId)
                    {
                        listToReturn.Add(new SubjectTeachers
                        {
                            SubjectId = subjectTeacher.SubjectId,
                            Subject = this.subjectsService.GetById(subjectTeacher.SubjectId),
                            TeacherId = subjectTeacher.TeacherId,
                            Teacher = this.teachersService.GetTeacherById(subjectTeacher.TeacherId),                       
                        });
                    }
                }
            }

            IQueryable query = listToReturn.AsQueryable();

            return query.To<T>().ToList();
        }

        public IEnumerable<SubjectTeachers> GetAllSubjectsTeachersForTeacher(int teacherId)
        {
            return this.subjectTeachersRepository.All().Where(x => x.TeacherId == teacherId);
        }

        public IEnumerable<T> GetAllSubjectsTeachersForTeacher<T>(int teacherId)
        {
            var subjectTeachers = this.subjectTeachersRepository.All().Where(x => x.TeacherId == teacherId);

            List<SubjectTeachers> list = new List<SubjectTeachers>();

            foreach (var subjectTeacher in subjectTeachers)
            {
                subjectTeacher.Subject = this.subjectsService.GetById(subjectTeacher.SubjectId);
                subjectTeacher.Teacher = this.teachersService.GetTeacherById(subjectTeacher.TeacherId);

                list.Add(subjectTeacher);
            }

            IQueryable query = list.AsQueryable();

            return query.To<T>().ToList();
        }

        public IEnumerable<T> GetAllSubjectsNotForTeacher<T>(IEnumerable<SubjectTeachers> subjectsForCourse)
        {
            var allSubjects = this.subjectRepository.All().OrderBy(x => x.Name).Select(x => x);

            List<Subject> subjectsToReturn = new List<Subject>();
            bool isItInCourse = false;

            foreach (var subject in allSubjects)
            {
                foreach (var subjectInCourse in subjectsForCourse)
                {
                    isItInCourse = false;
                    if (subject.Id == subjectInCourse.SubjectId)
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

        public IEnumerable<SubjectTeachers> GetAllSubjectsTeachers(int subjectId)
        {
            return this.subjectTeachersRepository.All().Where(x => x.SubjectId == subjectId).Select(x => x);
        }

        public IEnumerable<T> GetAllSubjectTeachersNotInCourse<T>(IEnumerable<CourseSubjectTeacher> allSubjectTeachersForCorse)
        {
            List<SubjectTeachers> subjectTeachersForCourseList = new List<SubjectTeachers>();

            foreach (var item in allSubjectTeachersForCorse)
            {
                subjectTeachersForCourseList.Add(new SubjectTeachers
                {
                    Subject = item.Subject,
                    SubjectId = item.SubjectId,
                    Teacher = item.Teacher,
                    TeacherId = item.TeacherId,
                });
            }

            var allSubjectsTeachers = this.subjectTeachersRepository.All().Select(x => x).OrderBy(x => x.Subject.Name);

            List<SubjectTeachers> subjectsTeachersToReturn = new List<SubjectTeachers>();
            bool isItInCourse = false;

            foreach (var subjectTeachers in allSubjectsTeachers)
            {
                foreach (var subjectTeachersInCourse in subjectTeachersForCourseList)
                {
                    isItInCourse = false;
                    if (subjectTeachers.SubjectId == subjectTeachersInCourse.SubjectId)
                    {
                        isItInCourse = true;
                        break;
                    }
                }

                if (isItInCourse == false)
                {
                    subjectTeachers.Subject = this.subjectsService.GetById(subjectTeachers.SubjectId);
                    subjectTeachers.Teacher = this.teachersService.GetTeacherById(subjectTeachers.TeacherId);
                    subjectsTeachersToReturn.Add(subjectTeachers);
                }
            }

            IQueryable query = subjectsTeachersToReturn.AsQueryable();

            return query.To<T>().ToList();
        }

        public IEnumerable<string> GetAllSubjectTeachersString(IEnumerable<SubjectTeachersDropDownListViewModel> subjectTeachers)
        {
            List<string> subjectTeachersToReturn = new List<string>();

            foreach (var item in subjectTeachers)
            {
                subjectTeachersToReturn.Add(item.SubjectTeacher);
            }

            // IQueryable query = subjectTeachersToReturn.AsQueryable();

            return subjectTeachersToReturn;
        }

        public IEnumerable<T> GetAllSubjectsForTeacher<T>(int teacherId)
        {
            var subjects = this.subjectTeachersRepository.All().Where(x => x.TeacherId == teacherId).Select(x => x);

            List<Subject> list = new List<Subject>();

            foreach (var subject in subjects)
            {
                subject.Subject = this.subjectsService.GetById(subject.SubjectId);

                list.Add(subject.Subject);
            }

            IQueryable query = list.AsQueryable();

            return query.To<T>().ToList();
        }
    }
}
