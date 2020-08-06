namespace MySchoolDiaryProject.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MySchoolDiaryProject.Data;
    using MySchoolDiaryProject.Data.Common.Repositories;
    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Services.Data;
    using MySchoolDiaryProject.Web.ViewModels;
    using MySchoolDiaryProject.Web.ViewModels.Home;
    using MySchoolDiaryProject.Web.ViewModels.Student;

    public class HomeController : BaseController
    {
        private const int ItemsPerPage = 12;

        private readonly ApplicationDbContext db;
        private readonly ICoursesService coursesService;
        private readonly IRepository<Course> courseRepository;
        private readonly IStudentsService studentsService;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(
            ApplicationDbContext db,
            ICoursesService coursesService,
            IRepository<Course> courseRepository,
            IStudentsService studentsService,
            UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.coursesService = coursesService;
            this.courseRepository = courseRepository;
            this.studentsService = studentsService;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index(int page = 1)
        {
            if (this.User.IsInRole("Administrator"))
            {
                var viewModel = new IndexModel
                {
                    Names = this.coursesService.GetAll<ListCourseViewModel>(ItemsPerPage, (page - 1) * ItemsPerPage),
                };

                var count = this.coursesService.GetCount();
                viewModel.PagesCount = (int)Math.Ceiling((double)count / ItemsPerPage);
                viewModel.CurrentPage = page;

                return this.View(viewModel);
            }
            else
            {
                var user = await this.userManager.FindByNameAsync(this.User.Identity.Name);
                var phoneNumber = int.Parse(user.PhoneNumber);

                var student = this.db.Students.Where(x => x.Id == phoneNumber).Select(x => x).FirstOrDefault();

                return this.RedirectToAction("IndexParent", "Home");
                //return this.RedirectToAction("Info", "Student", new { id = student.Id });
                //var viewModel = this.studentsService.GetStudentViewModel<StudentInfoViewModel>(student.Id);

                //return this.View(viewModel);
            }
        }

        public async Task<IActionResult> IndexParent()
        {
            var user = await this.userManager.FindByNameAsync(this.User.Identity.Name);
            var studentId = int.Parse(user.PhoneNumber);
            var student = this.studentsService.GetStudentById(studentId);

            var viewModel = new IndexParentViewModel
            {
                StudentId = studentId,
                Student = this.studentsService.GetStudentById(studentId),
                CourseId = student.CourseId,
            };

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
