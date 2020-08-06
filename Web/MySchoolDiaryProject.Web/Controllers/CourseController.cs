namespace MySchoolDiaryProject.Web.Controllers
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MySchoolDiaryProject.Data;
    using MySchoolDiaryProject.Data.Common.Repositories;
    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Data;
    using MySchoolDiaryProject.Web.ViewModels.Course;
    using MySchoolDiaryProject.Web.ViewModels.Subjects;
    using MySchoolDiaryProject.Web.ViewModels.Teachers;
    using ServiceStack;

    public class CourseController : BaseController
    {
        private readonly ICoursesService coursesService;
        private readonly ITeachersService teachersService;
        private readonly ISubjectsService subjectsService;
        private readonly ISubjectTeachersService subjectTeachersService;
        private readonly ICourseSubjectsService courseSubjectsService;
        private readonly ICourseSubjectTeacherService courseSubjectTeacherService;
        private readonly IStudentSubjectsService studentSubjectsService;
        private readonly IAttendancesService attendancesService;
        private readonly IGradeService gradeService;
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public CourseController(
            ICoursesService coursesService,
            ITeachersService teachersService,
            ISubjectsService subjectsService,
            ISubjectTeachersService subjectTeachersService,
            ICourseSubjectsService courseSubjectsService,
            ICourseSubjectTeacherService courseSubjectTeacherService,
            IStudentSubjectsService studentSubjectsService,
            IAttendancesService attendancesService,
            IGradeService gradeService,
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager)
        {
            this.coursesService = coursesService;
            this.teachersService = teachersService;
            this.subjectsService = subjectsService;
            this.subjectTeachersService = subjectTeachersService;
            this.courseSubjectsService = courseSubjectsService;
            this.courseSubjectTeacherService = courseSubjectTeacherService;
            this.studentSubjectsService = studentSubjectsService;
            this.attendancesService = attendancesService;
            this.gradeService = gradeService;
            this.db = db;
            this.userManager = userManager;
        }

        [Authorize(Roles = "Administrator")]
        [Authorize]
        public IActionResult ByName(string name)
        {
            var viewModel = this.coursesService.GetByName<CoursesViewModel>(name);

            var teacherName = this.teachersService.GetById<CoursesViewModel>(viewModel.TeacherId);
            viewModel.Teacher = teacherName;

            int id = viewModel.Id;

            var allCourseSubjectTeachers = this.courseSubjectTeacherService.GetAllForCourse<CourseSubjectsViewModel>(id);

            viewModel.CourseSubjectTeachers = allCourseSubjectTeachers;

            return this.View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            var teachers = this.teachersService.GetAll<TeachersDropDownViewModel>();

            CourseInputViewModel input = new CourseInputViewModel
            {
                Teachers = teachers,
            };

            return this.View(input);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult Create(CourseInputViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var teachers = this.teachersService.GetAll<TeachersDropDownViewModel>();
                input.Teachers = teachers;
                return this.View(input);
            }

            var teacher = this.teachersService.GetTeacherById(input.TeacherId);

            Course course = new Course(input.Name, input.Description, teacher);

            using (this.db)
            {
                this.db.Courses.Add(course);
                this.db.SaveChanges();
            }

            return this.RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int id = 0)
        {
            using (this.db)
            {
                var courseToEdit = this.db.Courses.FirstOrDefault(c => c.Id == id);
                var teachers = this.teachersService.GetAll<TeachersDropDownViewModel>();

                CourseInputViewModel input = new CourseInputViewModel
                {
                    Id = id,
                    Name = courseToEdit.Name,
                    Description = courseToEdit.Description,
                    Teachers = teachers,
                };

                return this.View(input);
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult Edit(CourseInputViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var teachers = this.teachersService.GetAll<TeachersDropDownViewModel>();
                input.Teachers = teachers;

                return this.View(input);
            }

            var courseToEdit = this.coursesService.GetById(input.Id);

            using (this.db)
            {
                courseToEdit.Name = input.Name;
                courseToEdit.Description = input.Description;
                courseToEdit.TeacherId = input.TeacherId;
                courseToEdit.Teacher = this.teachersService.GetTeacherById(input.TeacherId);

                this.db.SaveChanges();
            }

            return this.RedirectToAction("ByName", new { name = input.Name });
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            using (this.db)
            {
                var courseToDelete = this.db.Courses.FirstOrDefault(c => c.Id == id);

                return this.View(courseToDelete);
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult Delete(Course course)
        {
            using (this.db)
            {
                var course2 = course;
                var courseToDelete = this.db.Courses.Where(c => c.Id == course.Id).Select(x => x).FirstOrDefault();
                var allstudents = this.db.Students.Where(x => x.CourseId == course.Id).Select(x => x);
                var allcourseSubjectTeachers = this.courseSubjectTeacherService.GetAllSubjectsTeachersForCourse(course.Id);
                var allStudentSubjects = this.studentSubjectsService.GetAllStudentSubjectsForCourseId(course.Id);
                var allAttendaces = this.attendancesService.GetAllAtendancesForCourse(course.Id);
                var allCourseSubjects = this.courseSubjectsService.GetAllForCourse(course.Id);
                var allGrades = this.gradeService.GetAlGradesForCourse(course.Id);

                foreach (var courseSubjectTeacher in allcourseSubjectTeachers)
                {
                    if (courseSubjectTeacher.CourseId == course.Id)
                    {
                        this.db.Remove(courseSubjectTeacher);
                    }
                }

                foreach (var studentSubject in allStudentSubjects)
                {
                    this.db.StudentSubjects.Remove(studentSubject);
                }

                foreach (var attendance in allAttendaces)
                {
                    this.db.Attendances.Remove(attendance);
                }

                foreach (var courseSubject in allCourseSubjects)
                {
                    this.db.CourseSubjects.Remove(courseSubject);
                }

                foreach (var grade in allGrades)
                {
                    this.db.Grades.Remove(grade);
                }

                foreach (var student in allstudents)
                {
                    if (student.CourseId == course.Id)
                    {
                        this.db.Remove(student);
                    }
                }

                this.db.Courses.Remove(courseToDelete);
                this.db.SaveChanges();
            }

            return this.RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult AddSubject()
        {
            int courseId = this.GetRouteId();

            var courseSubjectTeachersForCourse = this.courseSubjectTeacherService.GetAllSubjectsTeachersForCourse(courseId);
            var subjectTeachers = this.subjectTeachersService.GetAllSubjectTeachersNotInCourse<SubjectTeachersDropDownListViewModel>(courseSubjectTeachersForCourse);

            //var subjectsInCourse = this.courseSubjectTeacherService.GetAllSubjectsForCourse(courseId);
            //var subjectTeachers = this.subjectTeachersService.GellAllSubjectTeachersNotInCollection<SubjectTeachersDropDownListViewModel>(subjectsInCourse);

            var input = new SubjectsInputViewModel
            {
                SubjectTeachers = subjectTeachers,
            };

            return this.View(input);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult AddSubject(SubjectsInputViewModel input)
        {
            var courseId = this.GetRouteId();
            var course = this.coursesService.GetById(courseId);

            string[] parts = input.SubjectTeacher.Split(" - ", StringSplitOptions.RemoveEmptyEntries);
            string[] teacherNames = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            Subject subject = this.subjectsService.GetASubjectByName(parts[0]);

            if (subject != null)
            {
                Teacher teacher = this.teachersService.GetTeacherByFullName(teacherNames[0], teacherNames[1], teacherNames[2]);

                var subjectToAdd = this.db.Subjects.Where(x => x.Id == subject.Id).Select(x => x).FirstOrDefault();
                var teacherToAdd = this.db.Teachers.Where(x => x.Id == teacher.Id).Select(x => x).FirstOrDefault();

                if (subjectToAdd == null)
                {
                    return this.View("This Subject does not exist");
                }

                if (teacherToAdd == null)
                {
                    return this.View("This teacher does not exist");
                }

                using (this.db)
                {
                    SubjectTeachers subjectTeacher = new SubjectTeachers(subject, teacher);

                    this.db.Set<CourseSubjects>().Add(new CourseSubjects
                    {
                        CourseId = courseId,
                        Course = this.coursesService.GetById(courseId),
                        SubjectId = subjectToAdd.Id,
                        Subject = this.subjectsService.GetById(subjectToAdd.Id),
                    });

                    this.db.Set<CourseSubjectTeacher>().Add(new CourseSubjectTeacher
                    {
                        CourseId = courseId,
                        Course = this.coursesService.GetById(courseId),
                        SubjectId = subjectToAdd.Id,
                        Subject = this.subjectsService.GetById(subjectToAdd.Id),
                        TeacherId = teacher.Id,
                        Teacher = teacher,
                    });

                    //foreach (var course in this.db.Courses)
                    //{
                    //    if (course.Id == courseId)
                    //    {
                    //        if (!course.SubjectTeachers.Contains(subjectTeacher))
                    //        {
                    //            course.SubjectTeachers.Add(subjectTeacher);
                    //        }
                    //        break;
                    //    }
                    //}

                    //foreach (var subjectDb in this.db.Subjects)
                    //{
                    //    if (subjectDb.Id == subjectToAdd.Id)
                    //    {
                    //        subjectDb.SubjectTeachers.Add(subjectTeacher);
                    //        break;
                    //    }
                    //}

                    //foreach (var teacherDb in this.db.Teachers)
                    //{
                    //    if (teacherDb.Id == teacherToAdd.Id)
                    //    {
                    //        teacherDb.SubjectTeachers.Add(subjectTeacher);
                    //        break;
                    //    }
                    //}

                    foreach (var student in this.db.Students)
                    {
                        if (student.CourseId == courseId)
                        {
                            student.Subjects.Add(new StudentSubjects
                            {
                                StudentId = student.Id,
                                Student = student,
                                SubjectId = subjectToAdd.Id,
                                Subject = this.subjectsService.GetById(subjectToAdd.Id),
                            });
                        }
                    }

                    this.db.SaveChanges();
                }
            }

            return this.RedirectToAction("ByName", new { name = course.Name });
        }

        public int GetRouteId()
        {
            var routeId = this.Url.ActionContext.RouteData.Values["id"];
            var stringId = routeId.ToString();
            string regexId = new string(stringId.ToCharArray().Where(c => char.IsDigit(c)).ToArray());
            int id = Convert.ToInt32(regexId);

            return id;
        }
    }
}
