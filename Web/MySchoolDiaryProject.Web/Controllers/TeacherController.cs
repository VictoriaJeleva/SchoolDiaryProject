namespace MySchoolDiaryProject.Web.Controllers
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MySchoolDiaryProject.Data;
    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Data;
    using MySchoolDiaryProject.Web.ViewModels.Course;
    using MySchoolDiaryProject.Web.ViewModels.CourseSubjectTeacher;
    using MySchoolDiaryProject.Web.ViewModels.Subjects;
    using MySchoolDiaryProject.Web.ViewModels.Teachers;

    public class TeacherController : BaseController
    {
        private const int ItemsPerPage = 3;
        private const int ItemsPerPageTeachersForCourse = 10;

        private readonly ApplicationDbContext db;
        private readonly ITeachersService teachersService;
        private readonly ISubjectsService subjectsService;
        private readonly IStudentsService studentsService;
        private readonly ISubjectTeachersService subjectTeachersService;
        private readonly ICoursesService coursesService;
        private readonly ICourseSubjectTeacherService courseSubjectTeacherService;

        public TeacherController(
            ApplicationDbContext db,
            ITeachersService teachersService,
            ISubjectsService subjectsService,
            IStudentsService studentsService,
            ISubjectTeachersService subjectTeachersService,
            ICoursesService coursesService,
            ICourseSubjectTeacherService courseSubjectTeacherService)
        {
            this.db = db;
            this.teachersService = teachersService;
            this.subjectsService = subjectsService;
            this.studentsService = studentsService;
            this.subjectTeachersService = subjectTeachersService;
            this.coursesService = coursesService;
            this.courseSubjectTeacherService = courseSubjectTeacherService;
        }

        public IActionResult AllTeachers(int page = 1)
        {
            var viewModel = new ListTeachersViewModel()
            {
                Teachers = this.teachersService.GetAll<TeacherInfoViewModel>(ItemsPerPage, (page - 1) * ItemsPerPage),
            };

            var count = this.teachersService.GetCount();
            viewModel.PagesCount = (int)Math.Ceiling((double)count / ItemsPerPage);
            viewModel.CurrentPage = page;

            return this.View(viewModel);
        }

        public IActionResult TeachersForCourse(int id, int page = 1)
        {
            var course = this.coursesService.GetById(id);

            var viewModel = new ListViewModel
            {
                CourseSubjectTeachers = this.courseSubjectTeacherService
                .GetAllForStudent<CourseSubjectTeachersViewModel>(course.Id, ItemsPerPageTeachersForCourse, (page - 1) * ItemsPerPageTeachersForCourse),
            };

            var count = this.courseSubjectTeacherService.GetCountForCourse(course.Id);
            viewModel.PagesCount = (int)Math.Ceiling((double)count / ItemsPerPageTeachersForCourse);
            viewModel.CurrentPage = page;
            int classTeacherId = viewModel.CourseSubjectTeachers.Select(x => x.TeacherId).First();
            viewModel.Teacher = this.teachersService.GetTeacherById(classTeacherId);

            return this.View(viewModel);
        }

        public IActionResult CreateTeacher()
        {
            var viewModel = new CreateTeacherInputViewModel();

            return this.View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateTeacher(CreateTeacherInputViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            Teacher teacher = new Teacher(input.Name, input.MiddleName, input.LastName, input.BirthDate, input.Gender, input.Phone, input.Address);

            using (this.db)
            {
                this.db.Teachers.Add(teacher);
                this.db.SaveChanges();
            }

            return this.RedirectToAction("AllTeachers");
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(string id, int page)
        {
            var teacherId = this.GetTeacherIdFromString(id);

            var viewModel = this.teachersService.GetTeacherById<TeacherInfoViewModel>(teacherId);

            return this.View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult Edit(TeacherInfoViewModel teacher, string id)
        {
            int currentPage = this.GetPagesCountFromString(id);
            int teacherId = this.GetTeacherIdFromString(id);

            teacher.Id = teacherId.ToString();

            if (!this.ModelState.IsValid)
            {
                return this.View(teacher);
            }

            var teacherToEdit = this.teachersService.GetTeacherById(int.Parse(teacher.Id));

            using (this.db)
            {
                teacherToEdit.Name = teacher.Name;
                teacherToEdit.MiddleName = teacher.MiddleName;
                teacherToEdit.LastName = teacher.LastName;
                teacherToEdit.BirthDate = teacher.BirthDate;
                teacherToEdit.Gender = teacher.Gender;
                teacherToEdit.Phone = teacher.Phone;
                teacherToEdit.Address = teacher.Address;

                this.db.SaveChanges();
                return this.RedirectToAction("AllTeachers", new { page = currentPage });
            }
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            var teacher = this.teachersService.GetTeacherById<DeleteChangeClassTeacherViewModel>(id);
            teacher.Teachers = this.teachersService.GetAllExceptOne<TeachersDropDownViewModel>(teacher.Id);
            teacher.Courses = this.coursesService.GetAllForClassTeacher<CoursesViewModel>(teacher.Id);
            //teacher.Course = this.coursesService.GetByClassTeacher(teacher.Id);
            //teacher.CourseId = teacher.Course.Id;

            return this.View(teacher);
        }

        [Authorize(Roles = "Administrator")]
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult Delete(DeleteChangeClassTeacherViewModel teacher)
        {
            var course = this.coursesService.GetByClassTeacher(teacher.Id);

            using (this.db)
            {
                var allSubjectTeachers = this.subjectTeachersService.GetAllSubjectsTeachersForTeacher(teacher.Id);
                var allCourseSubjectTeachers = this.courseSubjectTeacherService.GetAllSubjectsTeachersForTeacher(teacher.Id);

                foreach (var subjectTeacher in allSubjectTeachers)
                {
                    this.db.SubjectTeachers.Remove(subjectTeacher);
                }

                foreach (var courseSubjectTeachers in allCourseSubjectTeachers)
                {
                    this.db.CourseSubjectTeachers.Remove(courseSubjectTeachers);
                }

                var teacherToDelete = this.teachersService.GetTeacherById(teacher.Id);

                course.TeacherId = teacher.TeacherId;
                teacher.Courses = this.coursesService.GetAllForClassTeacher<CoursesViewModel>(teacher.Id);

                foreach (var coursee in teacher.Courses)
                {
                    this.db.Courses.Find(coursee.Id).TeacherId = teacher.TeacherId;
                }

                this.db.Teachers.Remove(teacherToDelete);

                this.db.SaveChanges();

                return this.RedirectToAction("AllTeachers");
            }
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult AddSubjectToTeacher()
        {
            int teacherId = this.GetRouteId();

            var subjectsTeachersForTeacher = this.subjectTeachersService.GetAllSubjectsTeachersForTeacher(teacherId);
            var subjects = this.subjectTeachersService.GetAllSubjectsNotForTeacher<SubjectDropDownListViewModel>(subjectsTeachersForTeacher);

            var input = new AddSubjectToTeacherViewModel
            {
                Subjects = subjects,
            };

            return this.View(input);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult AddSubjectToTeacher(AddSubjectToTeacherViewModel input)
        {
                var subjectIdToAdd = this.db.Subjects.Where(x => x.Id == input.Id).FirstOrDefault();

                var teacherId = this.GetRouteId();

                if (subjectIdToAdd == null)
                {
                    return this.RedirectToAction("AllTeachers", "Teacher");
                }

                using (this.db)
                {
                    this.db.Set<SubjectTeachers>().Add(new SubjectTeachers
                    {
                        SubjectId = subjectIdToAdd.Id,
                        Subject = this.subjectsService.GetById(subjectIdToAdd.Id),
                        TeacherId = teacherId,
                        Teacher = this.teachersService.GetTeacherById(teacherId),
                    });

                    this.db.SaveChanges();
                }
            
            return this.RedirectToAction("AllTeachers", "Teacher");
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult RemoveSubjectFromTeacher()
        {
            int teacherId = this.GetRouteId();

            var allSubjectsForTeacher = this.subjectTeachersService.GetAllSubjectsForTeacher<SubjectDropDownListViewModel>(teacherId);

            var viewModel = new AddSubjectToTeacherViewModel()
            {
                Subjects = allSubjectsForTeacher,
            };

            return this.View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult RemoveSubjectFromTeacher(AddSubjectToTeacherViewModel viewModel)
        {
            int teacheId = this.GetRouteId();

            var subject = this.db.Subjects.Where(x => x.Id == viewModel.Id).FirstOrDefault();

            if (subject == null)
            {
                return this.RedirectToAction("AllTeachers");
            }

            var subjectTeacherToDelete = this.db.SubjectTeachers.Where(x => x.SubjectId == subject.Id && x.TeacherId == teacheId)
                .FirstOrDefault();

            var teacherId = this.GetRouteId();

            if (subject == null)
            {
                return this.View("This Subject does not exist");
            }

            var allSubjectsTeachers = this.subjectTeachersService.GetAllSubjectsTeachersForTeacher(teacherId);
            var allCourseSubjectsTeachers = this.courseSubjectTeacherService.GetAllSubjectsTeachersForTeacher(teacheId);

            using (this.db)
            {
                foreach (var subjectTeacher in allSubjectsTeachers)
                {
                    if (subjectTeacher.SubjectId == subjectTeacherToDelete.SubjectId)
                    {
                        this.db.SubjectTeachers.Remove(subjectTeacher);
                    }
                }

                foreach (var courseSubjectTeacher in allCourseSubjectsTeachers)
                {
                    if (courseSubjectTeacher.SubjectId == subjectTeacherToDelete.SubjectId)
                    {
                        this.db.CourseSubjectTeachers.Remove(courseSubjectTeacher);
                    }
                }

                this.db.SaveChanges();

                return this.RedirectToAction("AllTeachers");
            }
        }

        public int GetRouteId()
        {
            var routeId = this.Url.ActionContext.RouteData.Values["id"];
            var stringId = routeId.ToString();
            string regexId = new string(stringId.ToCharArray().Where(c => char.IsDigit(c)).ToArray());
            int id = Convert.ToInt32(regexId);

            return id;
        }

        private int GetTeacherIdFromString(string id)
        {
            string[] splitWords = id.Split("-", StringSplitOptions.RemoveEmptyEntries);
            string[] splitTeacher = splitWords[0].Split(":", StringSplitOptions.RemoveEmptyEntries);
            int teacherId = int.Parse(splitTeacher[1]);

            return teacherId;
        }

        private int GetPagesCountFromString(string id)
        {
            string[] splitWords = id.Split("-", StringSplitOptions.RemoveEmptyEntries);
            string[] splitPage = splitWords[1].Split(":", StringSplitOptions.RemoveEmptyEntries);
            int pageCount = int.Parse(splitPage[1]);

            return pageCount;
        }
    }
}
