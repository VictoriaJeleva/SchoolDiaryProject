namespace MySchoolDiaryProject.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.AspNetCore.Mvc;
    using MySchoolDiaryProject.Data;
    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Data;
    using MySchoolDiaryProject.Web.ViewModels.Attendance;
    using MySchoolDiaryProject.Web.ViewModels.Subjects;
    using ServiceStack;

    public class AttendanceController : BaseController
    {
        private readonly ISubjectsService subjectsService;
        private readonly IStudentsService studentsService;
        private readonly IStudentSubjectsService studentSubjectsService;
        private readonly IAttendancesService attendancesService;
        private readonly ApplicationDbContext db;

        public AttendanceController(
            ISubjectsService subjectsService,
            IStudentsService studentsService,
            IStudentSubjectsService studentSubjectsService,
            IAttendancesService attendancesService,
            ApplicationDbContext db)
        {
            this.subjectsService = subjectsService;
            this.studentsService = studentsService;
            this.studentSubjectsService = studentSubjectsService;
            this.attendancesService = attendancesService;
            this.db = db;
        }

        public System.Web.Mvc.JsonResult GetAttendances()
        {
            using (this.db)
            {
                var events = this.db.Attendances.ToList();
                return new System.Web.Mvc.JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public IActionResult Callendar()
        {
            var viewModel = new CallendarViewModel();

            return this.View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult AddAnAbsense()
        {
            int studentId = this.GetRouteId();

            var subjects = this.studentSubjectsService.GetAllSubjectsByStudentId<SubjectDropDownListViewModel>(studentId);
            var input = new AttendanceInputViewModel
            {
                StudentId = studentId,
                Subjects = subjects,
            };

            return this.View(input);
        }

        [Authorize(Roles = "Administrator")]
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult AddAnAbsense(AttendanceInputViewModel input)
        {
            bool isItNull = false;
            int studentId = this.GetRouteId();
            input.StudentId = studentId;
            input.Student = this.studentsService.GetStudentById(studentId);

            if (input.SubjectId == null)
            {
                input.SubjectId = 1;
                isItNull = true;
            }

            //if (!this.ModelState.IsValid)
            //{
            //    int student_Id = this.GetRouteId();
            //    var subjects = this.studentSubjectsService.GetAllSubjectsByStudentId<SubjectDropDownListViewModel>(student_Id);
            //    input.Subjects = subjects;

            //    return this.View(input);
            //}

            var student = this.studentsService.GetStudentById(studentId);
            input.Student = student;
            input.StudentId = student.Id;

            Attendance attendance = new Attendance();

            if (isItNull == false)
            {
                int? subjectId = input.SubjectId;
                var subject = this.subjectsService.GetById((int)subjectId);

                input.Subject = subject;
                input.SubjectId = subject.Id;

                attendance = this.attendancesService.CreateAttendance(input.Subject, input.Student, input.DateOfAbsense, input.Remark);
            }
            else
            {
                attendance.Student = student;
                attendance.StudentId = student.Id;
                attendance.Subject = null;
                attendance.SubjectId = null;
                attendance.DateOfAbsense = input.DateOfAbsense;
                attendance.Remark = input.Remark;
            }

            using (this.db)
            {
                this.db.Attendances.Add(attendance);
                this.db.SaveChanges();
            }

            return this.RedirectToAction("Info", "Student", new { id = student.Id });
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int id)
        {
            var viewModel = this.attendancesService.GetById<AttendaceToEditViewModel>(id);

            viewModel.Student = this.studentsService.GetStudentById(viewModel.StudentId);

            var subjectsStudent = this.studentSubjectsService.GetAllSubjectsByStudentId<SubjectDropDownListViewModel>(viewModel.StudentId);

            if (viewModel.SubjectId != null)
            {
                int subjectId = (int)viewModel.SubjectId;
                var subject = this.subjectsService.GetSubject(subjectId);
                viewModel.SubjectId = subject.Id;
                viewModel.Subject = subject;
                viewModel.Subjects = subjectsStudent;
            }

            viewModel.Subjects = subjectsStudent;

            return this.View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult Edit(AttendaceToEditViewModel viewModel)
        {
            int studentId = 0;

            using (this.db)
            {
                foreach (var attendanceDb in this.db.Attendances)
                {
                    if (attendanceDb.Id == viewModel.Id)
                    {
                        if (viewModel.SubjectId != null)
                        {
                            attendanceDb.SubjectId = viewModel.SubjectId;
                            attendanceDb.Subject = this.subjectsService.GetById((int)viewModel.SubjectId);
                        }
                        else
                        {
                            attendanceDb.SubjectId = null;
                            attendanceDb.Subject = null;
                        }

                        attendanceDb.DateOfAbsense = viewModel.DateOfAbsense;
                        attendanceDb.Remark = viewModel.Remark;
                        studentId = attendanceDb.StudentId;
                    }
                }

                this.db.SaveChanges();
            }

            return this.RedirectToAction("Info", "Student", new { id = studentId });
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            using (this.db)
            {
                var attendanceToDelete = this.attendancesService.GetById(id);

                return this.View(attendanceToDelete);
            }
        }

        [Authorize(Roles = "Administrator")]
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult Delete(Attendance attendanceToDelete)
        {
            attendanceToDelete.StudentId = this.attendancesService.GetStudentId(attendanceToDelete.Id);

            using (this.db)
            {
                this.db.Attendances.Remove(attendanceToDelete);
                this.db.SaveChanges();
            }

            return this.RedirectToAction("Info", "Student", new { id = attendanceToDelete.StudentId });
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
