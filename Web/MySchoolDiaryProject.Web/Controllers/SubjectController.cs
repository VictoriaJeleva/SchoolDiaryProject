namespace MySchoolDiaryProject.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MySchoolDiaryProject.Data;
    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Data;
    using MySchoolDiaryProject.Web.ViewModels.Subjects;
    using ServiceStack;

    public class SubjectController : BaseController
    {
        private const int ItemsPerPage = 5;

        private readonly ApplicationDbContext db;
        private readonly ISubjectsService subjectsService;
        private readonly ICoursesService coursesService;
        private readonly IStudentSubjectsService studentSubjectsService;
        private readonly ICourseSubjectsService courseSubjectsService;
        private readonly IAttendancesService attendancesService;
        private readonly ISubjectTeachersService subjectTeachersService;
        private readonly ICourseSubjectTeacherService courseSubjectTeacherService;
        private readonly IGradeService gradeService;
        private readonly ICourseSubjectsService courseSubjectsService1;

        public SubjectController(
            ApplicationDbContext db,
            ISubjectsService subjectsService,
            ICoursesService coursesService,
            IStudentSubjectsService studentSubjectsService,
            ICourseSubjectsService courseSubjectsService,
            IAttendancesService attendancesService,
            ISubjectTeachersService subjectTeachersService,
            ICourseSubjectTeacherService courseSubjectTeacherService,
            IGradeService gradeService)
        {
            this.db = db;
            this.subjectsService = subjectsService;
            this.coursesService = coursesService;
            this.studentSubjectsService = studentSubjectsService;
            this.courseSubjectsService = courseSubjectsService;
            this.attendancesService = attendancesService;
            this.subjectTeachersService = subjectTeachersService;
            this.courseSubjectTeacherService = courseSubjectTeacherService;
            this.gradeService = gradeService;
        }

        public IActionResult AllSubjects(int page = 1)
        {
            var subjects = this.subjectsService.GetAll<SubjectInfoViewModel>(ItemsPerPage, (page - 1) * ItemsPerPage);

            var viewModel = new ListAllSubjectsViewModel()
            {
                Subjects = subjects,
            };

            var count = this.subjectsService.GetCount();
            viewModel.PagesCount = (int)Math.Ceiling((double)count / ItemsPerPage);
            viewModel.CurrentPage = page;

            return this.View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            var viewModel = new CreateSubjectInputViewModel();

            return this.View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult Create(CreateSubjectInputViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            Subject subject = new Subject(input.Name);

            using (this.db)
            {
                this.db.Subjects.Add(subject);
                this.db.SaveChanges();
            }

            return this.RedirectToAction("AllSubjects");
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(string id)
        {
            int subjectId = this.GetSubjectIdFromString(id);

            var subjectToEdit = this.subjectsService.GetById(subjectId);

            var viewModel = this.subjectsService.GetById<SubjectInfoViewModel>(subjectId);

            return this.View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult Edit(SubjectInfoViewModel subject, string id)
        {
            int subjectId = this.GetSubjectIdFromString(id);
            int currentPage = this.GetPagesCountFromString(id);

            subject.Id = subjectId.ToString();

            if (!this.ModelState.IsValid)
            {
                return this.View(subject);
            }

            using (this.db)
            {
                var teacherToEdit = this.subjectsService.GetById(subjectId);

                teacherToEdit.Name = subject.Name;

                this.db.SaveChanges();
                return this.RedirectToAction("AllSubjects", new { page = currentPage });
            }
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteFromCourse(string id, int cId)
        {
            string[] splitIds = id.Split("-", StringSplitOptions.RemoveEmptyEntries);
            int subjectId = int.Parse(splitIds[0]);
            int courseId = int.Parse(splitIds[1]);

            var subject = this.subjectsService.GetById<SubjectInfoViewModel>(subjectId);
            subject.CourseIdd = courseId;

            return this.View(subject);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult DeleteFromCourse(string id)
        {
            string[] splitIds = id.Split("-", StringSplitOptions.RemoveEmptyEntries);
            int subjectId = int.Parse(splitIds[0]);
            int courseId = int.Parse(splitIds[1]);

            using (this.db)
            {
                var subjectToDelete = this.subjectsService.GetById(subjectId);

                var allStudentsSubjects = this.studentSubjectsService.GetAllStudentSubjectsForSubjectAndCourse(subjectToDelete.Id, courseId);
                var allCourseSubjects = this.courseSubjectsService.GetAllSubjectsBySubjectAndCourse(subjectToDelete.Id, courseId);
                var allAttendances = this.attendancesService.GetAllAtendancesForSubjectAndCourse(subjectToDelete.Id, courseId);
                var allCourseSubjectTeachers = this.courseSubjectTeacherService.GetAllSubjectsTeachersForSubjectAndCourse(subjectToDelete.Id, courseId);
                var allGradesForSubject = this.gradeService.GetAlGradesForSubjectAndCourse(subjectToDelete.Id, courseId);

                foreach (var attendance in allAttendances)
                {
                    this.db.Attendances.Remove(attendance);
                }

                foreach (var courseSubject in allCourseSubjects)
                {
                    this.db.CourseSubjects.Remove(courseSubject);
                }

                foreach (var studentSubject in allStudentsSubjects)
                {
                    this.db.StudentSubjects.Remove(studentSubject);
                }

                foreach (var courseSubjectTeacher in allCourseSubjectTeachers)
                {
                    this.db.CourseSubjectTeachers.Remove(courseSubjectTeacher);
                }

                foreach (var grade in allGradesForSubject)
                {
                    this.db.Grades.Remove(grade);
                }

                this.db.SaveChanges();
            }

            return this.RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            using (this.db)
            {
                var subjectToDelete = this.db.Subjects.FirstOrDefault(s => s.Id == id);

                return this.View(subjectToDelete);
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult Delete(Subject subject)
        {
            using (this.db)
            {
                var subjectToDelete = this.db.Subjects.Where(c => c.Id == subject.Id).Select(x => x).FirstOrDefault();

                var allStudentsSubjects = this.studentSubjectsService.GetAllStudentSubjectsForSubjectId(subjectToDelete.Id);
                var allCourseSubjects = this.courseSubjectsService.GetAllSubjectsBySubjectId(subjectToDelete.Id);
                var allAttendances = this.attendancesService.GetAllAtendancesForSubject(subjectToDelete.Id);
                var allCourseSubjectTeachers = this.courseSubjectTeacherService.GetAllSubjectsTeachersForSubject(subjectToDelete.Id);
                var allGradesForSubject = this.gradeService.GetAlGradesForSubject(subjectToDelete.Id);
                var allSubjectTeachers = this.subjectTeachersService.GetAllSubjectsTeachers(subjectToDelete.Id);

                foreach (var attendance in allAttendances)
                {
                    this.db.Attendances.Remove(attendance);
                }

                foreach (var courseSubject in allCourseSubjects)
                {
                    this.db.CourseSubjects.Remove(courseSubject);
                }

                foreach (var studentSubject in allStudentsSubjects)
                {
                    this.db.StudentSubjects.Remove(studentSubject);
                }

                foreach (var courseSubjectTeacher in allCourseSubjectTeachers)
                {
                    this.db.CourseSubjectTeachers.Remove(courseSubjectTeacher);
                }

                foreach (var grade in allGradesForSubject)
                {
                    this.db.Grades.Remove(grade);
                }

                foreach (var subjectTeacher in allSubjectTeachers)
                {
                    this.db.SubjectTeachers.Remove(subjectTeacher);
                }

                this.db.Subjects.Remove(subjectToDelete);

                this.db.SaveChanges();
            }

            return this.RedirectToAction("AllSubjects");
        }

        public int GetRouteId()
        {
            var routeId = this.Url.ActionContext.RouteData.Values["id"];
            var stringId = routeId.ToString();
            string regexId = new string(stringId.ToCharArray().Where(c => char.IsDigit(c)).ToArray());
            int id = Convert.ToInt32(regexId);

            return id;
        }

        public int GetId(string action, string controller)
        {
            var url = this.Url.Action(action, controller, null, this.Request.Scheme);

            var lastChar = url.Substring(url.Length - 1);

            int id = Convert.ToInt32(lastChar);

            return id;
        }

        private int GetSubjectIdFromString(string id)
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
