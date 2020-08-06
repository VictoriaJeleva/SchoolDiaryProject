namespace MySchoolDiaryProject.Web
{
    using Microsoft.AspNetCore.Mvc;
    using MySchoolDiaryProject.Data;
    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Web.ViewModels.Registration;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class RegistrationController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly ApplicationDbContext db;

        public RegistrationController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult RegisterUser()
        {
            var input = new RegistrateUserViewModel();

            return this.View(input);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult RegisterUser(RegistrateUserViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            using (this.db)
            {
                ApplicationUser user = new ApplicationUser();

                user.UserName = input.Email;
                user.Email = input.Email;
                user.PhoneNumber = input.Code;

                user.PasswordHash = input.Password;
                this.db.Users.Add(user);

                this.db.SaveChanges();
            }

            return this.RedirectToPage("https://localhost:44319/Identity/Account/Login");
        }
    }
}
