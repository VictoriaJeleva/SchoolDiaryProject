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
    using MySchoolDiaryProject.Web.ViewModels.Grade;

    public class GradeController : BaseController
    {
        private readonly ApplicationDbContext db;
        private readonly IGradeService gradeService;
        private readonly IStudentsService studentsService;
        private readonly ICoursesService coursesService;

        public GradeController(
            ApplicationDbContext db,
            IGradeService gradeService,
            IStudentsService studentsService,
            ICoursesService coursesService)
        {
            this.db = db;
            this.gradeService = gradeService;
            this.studentsService = studentsService;
            this.coursesService = coursesService;
        }

        public IActionResult ShowGrades()
        {
            return this.View();
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int id)
        {
            var grade = this.gradeService.GetById(id);

            var viewModel = new GradeViewModel()
            {
                Id = grade.Id,
                Value = grade.Value,
                StudentId = grade.StudentId,
                SubjectId = grade.SubjectId,
            };

            return this.View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult Edit(GradeViewModel viewModel)
        {
            var gradeToEdit = this.gradeService.GetById<GradeViewModel>(viewModel.Id);
            var student = this.studentsService.GetStudentById(gradeToEdit.StudentId);
            var course = this.coursesService.GetById(student.CourseId);

            if (!this.ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            using (this.db)
            {
                foreach (var gradeDb in this.db.Grades)
                {
                    if (gradeDb.Id == gradeToEdit.Id)
                    {
                        gradeDb.Value = viewModel.Value;
                    }
                }

                this.db.SaveChanges();
            }

            return this.RedirectToAction("StudentInfoGrades", "Student", new { id = course.Id });
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            using (this.db)
            {
                var gradeToDelete = this.db.Grades.FirstOrDefault(s => s.Id == id);

                return this.View(gradeToDelete);
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult Delete(Grade grade)
        {
            var gradeToDelete = this.gradeService.GetById<GradeViewModel>(grade.Id);
            var student = this.studentsService.GetStudentById(gradeToDelete.StudentId);
            var course = this.coursesService.GetById(student.CourseId);

            using (this.db)
            {
                foreach (var gradeDb in this.db.Grades)
                {
                    if (gradeDb.Id == gradeToDelete.Id)
                    {
                        this.db.Grades.Remove(gradeDb);
                    }
                }

                this.db.SaveChanges();
            }

            return this.RedirectToAction("StudentInfoGrades", "Student", new { id = course.Id });
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
