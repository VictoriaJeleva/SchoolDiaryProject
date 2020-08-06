namespace MySchoolDiaryProject.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MySchoolDiaryProject.Data.Models;

    public class AdminController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<ApplicationRole> userManager;

        public AdminController(
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationRole> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Test()
        {
            var result = await this.roleManager.CreateAsync(new ApplicationRole
            {
                Name = "Admin",
            });

            var user = await this.userManager.GetUserAsync(this.User);
            await this.userManager.AddToRoleAsync(user, "Admin");

            return this.Json(result);
        }
    }
}
