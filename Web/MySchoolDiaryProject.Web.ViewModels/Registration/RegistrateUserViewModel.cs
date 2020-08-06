namespace MySchoolDiaryProject.Web.ViewModels.Registration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class RegistrateUserViewModel
    {
        public string Username { get; set; }

        [MinLength(6)]
        public string Password { get; set; }

        [MinLength(6)]
        public string ConfirmPassword { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Code { get; set; }
    }
}
