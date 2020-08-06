using Microsoft.AspNetCore.Mvc;
using MySchoolDiaryProject.Data;
using MySchoolDiaryProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySchoolDiaryProject.Web.Areas.Administration.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext db;

        public CourseController(ApplicationDbContext db)
        {
            this.db = db;
        }
    }
}
