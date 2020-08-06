namespace MySchoolDiaryProject.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using MySchoolDiaryProject.Data;
    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Data;
    using MySchoolDiaryProject.Web.ViewModels.Attendance;
    using MySchoolDiaryProject.Web.ViewModels.Course;
    using MySchoolDiaryProject.Web.ViewModels.Grade;
    using MySchoolDiaryProject.Web.ViewModels.Student;
    using MySchoolDiaryProject.Web.ViewModels.Subjects;
    using ServiceStack;

    public class StudentController : BaseController
    {
        private const int ItemsPerPage = 5;

        private readonly IStudentsService studentsService;
        private readonly ISubjectsService subjectService;
        private readonly ICoursesService courseService;
        private readonly IGradeService gradeService;
        private readonly ICourseSubjectsService courseSubjectsService;
        private readonly IStudentSubjectsService studentSubjectsService;
        private readonly IAttendancesService attendancesService;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly ApplicationDbContext db;

        public StudentController(
            IStudentsService studentsService,
            ISubjectsService subjectService,
            ICoursesService courseService,
            IGradeService gradeService,
            ICourseSubjectsService courseSubjectsService,
            IStudentSubjectsService studentSubjectsService,
            IAttendancesService attendancesService,
            IHostingEnvironment hostingEnvironment,
            ApplicationDbContext db)
        {
            this.studentsService = studentsService;
            this.subjectService = subjectService;
            this.courseService = courseService;
            this.gradeService = gradeService;
            this.courseSubjectsService = courseSubjectsService;
            this.studentSubjectsService = studentSubjectsService;
            this.attendancesService = attendancesService;
            this.hostingEnvironment = hostingEnvironment;
            this.db = db;
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult AllStudents(int page = 1)
        {
            var viewModel = new ListStudentsViewModel()
            {
                Students = this.studentsService.GetAll<AllInfoAboutStudentViewModel>(ItemsPerPage, (page - 1) * ItemsPerPage),
            };

            var count = this.studentsService.GetCount();
            viewModel.PagesCount = (int)Math.Ceiling((double)count / ItemsPerPage);
            viewModel.CurrentPage = page;

            return this.View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult CreateStudent()
        {
            var courses = this.courseService.GetAllWithouPages<CourseDropDownViewModel>();

            var viewModel = new CreateStudentInputViewModel
            {
                Courses = courses,
            };

            return this.View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult CreateStudent(CreateStudentInputViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var courses = this.courseService.GetAllWithouPages<CourseDropDownViewModel>();
                input.Courses = courses;
                return this.View(input);
            }

            var course = this.courseService.GetById(input.CourseId);

            Student student = new Student(input.Name, input.MiddleName, input.LastName, input.BirthDate, input.Gender, input.Phone, input.Address, course);
            List<Subject> subjectsToAdd = this.subjectService.GetAllSubjectsInCourse(course.Id);

            student.Subjects = this.studentsService.FillStudentSubjectsCollection(subjectsToAdd, student.Id);
            student.SubjectsNames = this.subjectService.GetAllSubjectNameInCourses(student.Subjects);

            string uniqueFileName = string.Empty;

            if (input.Photo != null)
            {
                string uploadsFolder = Path.Combine(this.hostingEnvironment.WebRootPath, "css\\StudentImages");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + input.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                input.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                student.PhotoPath = uniqueFileName;
            }

            using (this.db)
            {
                this.db.Students.Add(student);
                this.db.SaveChanges();
            }

            return this.RedirectToAction("ByName", "Course", new { course.Name });
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int id = 0)
        {
            var viewModel = this.studentsService.GetById<AllInfoAboutStudentViewModel>(id);
            viewModel.Courses = this.courseService.GetAllWithouPages<CourseDropDownViewModel>();

            return this.View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult Edit(AllInfoAboutStudentViewModel viewModel, string action, int page)
        {
            var studentToEdit = this.studentsService.GetStudentById(viewModel.Id);

            using (this.db)
            {
                studentToEdit.Name = viewModel.Name;
                studentToEdit.MiddleName = viewModel.MiddleName;
                studentToEdit.LastName = viewModel.LastName;
                studentToEdit.BirthDate = viewModel.BirthDate;
                studentToEdit.Gender = viewModel.Gender;
                studentToEdit.Phone = viewModel.Phone;
                studentToEdit.Address = viewModel.Address;

                if (viewModel.Photo != null)
                {
                    string uploadsFolder = Path.Combine(this.hostingEnvironment.WebRootPath, "css\\StudentImages");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    viewModel.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                    studentToEdit.PhotoPath = uniqueFileName;
                }

                if (studentToEdit.CourseId != viewModel.CourseId)
                {
                    this.courseService.RemoveAStudentFromStudentsCollection(studentToEdit);

                    studentToEdit.CourseId = viewModel.CourseId;
                    studentToEdit.Course = viewModel.Course;

                    var subjects = this.db.StudentSubjects.Where(x => x.StudentId == studentToEdit.Id);

                    if (subjects != null)
                    {
                        foreach (var subject in subjects)
                        {
                            this.db.StudentSubjects.Remove(subject);
                        }
                    }

                    this.db.SaveChanges();

                    List<Subject> subjectsToAdd = this.subjectService.GetAllSubjectsInCourse(studentToEdit.CourseId);

                    if (subjectsToAdd.Count > 1)
                    {
                        studentToEdit.Subjects = this.studentsService.FillStudentSubjectsCollection(subjectsToAdd, studentToEdit.Id);
                        studentToEdit.SubjectsNames = this.studentSubjectsService.GetOnlySubjectsFromCollection(studentToEdit.Subjects);
                    }
                }

                studentToEdit.CourseId = viewModel.CourseId;
                studentToEdit.Course = viewModel.Course;

                if (!this.ModelState.IsValid)
                {
                    viewModel.Courses = this.courseService.GetAllWithouPages<CourseDropDownViewModel>();
                    return this.View(viewModel);
                }

                this.db.SaveChanges();

                if (action == "Edit")
                {
                    return this.RedirectToAction("Info", new { id = studentToEdit.Id });
                }

                return this.RedirectToAction("AllStudents" , new { pageToReturn = page} );
            }
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            using (this.db)
            {
                var studentToDelete = this.db.Students.FirstOrDefault(s => s.Id == id);

                return this.View(studentToDelete);
            }
        }

        [Authorize(Roles = "Administrator")]
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult Delete(Student student)
        {
            using (this.db)
            {
                var studentToDelete = this.db.Students.Where(c => c.Id == student.Id).Select(x => x).FirstOrDefault();
                var course = this.courseService.GetById(studentToDelete.CourseId);
                this.courseService.RemoveAStudentFromStudentsCollection(studentToDelete);

                var allGrades = this.gradeService.GetAlGradesForStudent(studentToDelete.Id);
                var allSubjectsStudents = this.studentSubjectsService.GetAllStudentSubjectsForStudentId(studentToDelete.Id);
                var allStudentAttendances = this.attendancesService.GetAllAtendancesForStudent(studentToDelete.Id);

                foreach (var grade in allGrades)
                {
                    this.db.Grades.Remove(grade);
                }

                foreach (var subjectStudents in allSubjectsStudents)
                {
                    this.db.StudentSubjects.Remove(subjectStudents);
                }

                foreach (var studentAttendance in allStudentAttendances)
                {
                    this.db.Attendances.Remove(studentAttendance);
                }

                this.db.SaveChanges();

                this.db.Students.Remove(studentToDelete);
                this.db.SaveChanges();

                return this.RedirectToAction("ByName", "Course", new { name = course.Name });
            }
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult DeletePicture(int id)
        {
            var student = this.studentsService.GetById<StudentInfoViewModel>(id);

            return this.View(student);
        }

        [Authorize(Roles = "Administrator")]
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult DeletePicture(StudentInfoViewModel student)
        {
            using (this.db)
            {
                var studentToDelete = this.db.Students.Where(c => c.Id == student.Id).Select(x => x).FirstOrDefault();

                this.db.Students.Find(student.Id).PhotoPath = null;

                this.db.SaveChanges();

                return this.RedirectToAction("Info", new { id = student.Id });
            }
        }

        public IActionResult StudentInfoGrades(int id, int page = 1)
        {
            var students = this.studentsService.GetAll(id);
            var subjectsIds = this.courseSubjectsService.GetAllSubjectsIds(id);

            var viewModel = new ListOfStudentsInfoGrades
            {
                Students = this.studentsService.GetAll<StudentInfoGradeViewModel>(id, ItemsPerPage, (page - 1) * ItemsPerPage),
            };

            foreach (var student in viewModel.Students)
            {
                int studentId = student.Id;
                var subjectsIdsSS = this.studentSubjectsService.GetAllSubjectsIds(studentId);

                student.Grades = this.gradeService.GetAll<GradeViewModel>(studentId).OrderBy(x => x.SubjectId);
                student.SubjectsNames = this.subjectService.GetAllSubjectsInCourses(subjectsIdsSS).OrderBy(x => x.Id).ToList();
            }

            var count = this.studentsService.GetCountInCourse(id);

            if (count == 0)
            {
                count = 1;
            }

            viewModel.PagesCount = (int)Math.Ceiling((double)count / ItemsPerPage);
            viewModel.CurrentPage = page;

            return this.View(viewModel);
        }

        public IActionResult OneStudentGrade(int id)
        {
            var viewModel = this.studentsService.GetById<StudentInfoGradeViewModel>(id);

            var subjectsIds = this.studentSubjectsService.GetAllSubjectsIds(viewModel.Id);

            viewModel.Grades = this.gradeService.GetAll<GradeViewModel>(viewModel.Id).OrderBy(x => x.SubjectId);
            viewModel.SubjectsNames = this.subjectService.GetAllSubjectsInCourses(subjectsIds).OrderBy(x => x.Id).ToList();

            return this.View(viewModel);
        }

        public IActionResult Info(int id, int page = 1)
        {
            var viewModel = this.studentsService.GetStudentViewModel<StudentInfoViewModel>(id);
            viewModel.StudentAttendances = this.attendancesService.GetAbbsenseByStudentId<StudentAttendancesViewModel>(viewModel.Id, ItemsPerPage, (page - 1) * ItemsPerPage);
            //var count = 11;
            //((count - 1) / 5 + 1);
            var count = this.attendancesService.GetCountByStudentId(viewModel.Id);
            viewModel.PagesCount = (int)Math.Ceiling((double)count / ItemsPerPage);
            viewModel.CurrentPage = page;

            return this.View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult AddGrade(string id)
        {
            int studentId = this.GetStudentIdFromString(id);

            var subjects = this.studentSubjectsService.GetAllSubjectsByStudentId<SubjectDropDownListViewModel>(studentId);

            GradeInputViewModel input = new GradeInputViewModel
            {
                Subjects = subjects,
            };

            return this.View(input);
        }

        [Authorize(Roles = "Administrator")]
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult AddGrade(GradeInputViewModel input, string id)
        {
            int studentId = this.GetStudentIdFromString(id);

            if (!this.ModelState.IsValid)
            {
                input.Subjects = this.studentSubjectsService.GetAllSubjectsByStudentId<SubjectDropDownListViewModel>(studentId);
                return this.View(input);
            }

            var subject = this.subjectService.GetById(input.SubjectId);

            int currentPage = this.GetPagesCountFromString(id);
            int? courseId = this.studentsService.GetCourseIdByStudentId(studentId);

            if (subject == null)
            {
                return this.RedirectToAction("StudentInfoGrades", new { @id = courseId, page = currentPage });
            }

            int subjectId = subject.Id;

            using (this.db)
            {
                this.db.Grades.Add(new Grade
                {
                    Value = input.Value,
                    StudentId = studentId,
                    SubjectId = subjectId,
                    SubjectName = subject.Name,
                });

                this.db.SaveChanges();
            }

            return this.RedirectToAction("StudentInfoGrades", new { @id = courseId, page = currentPage } );
        }

        public int GetRouteId()
        {
            var routeId = this.Url.ActionContext.RouteData.Values["id"];
            var stringId = routeId.ToString();
            string regexId = new string(stringId.ToCharArray().Where(c => char.IsDigit(c)).ToArray());
            int id = Convert.ToInt32(regexId);

            return id;
        }

        public int GetId()
        {
            var url = this.Url.Action("StudentInfoGrades", "Grade", null, this.Request.Scheme);

            var lastChar = url.Substring(url.Length - 1);

            int id = Convert.ToInt32(lastChar);

            return id;
        }

        private int GetStudentIdFromString(string id)
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
