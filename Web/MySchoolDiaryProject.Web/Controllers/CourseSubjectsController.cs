namespace MySchoolDiaryProject.Web.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using MySchoolDiaryProject.Data;
    using MySchoolDiaryProject.Data.Models;

    public class CourseSubjectsController : BaseController
    {
        private readonly ApplicationDbContext db;

        public CourseSubjectsController(ApplicationDbContext db)
        {
            this.db = db;
        }
    }
}
